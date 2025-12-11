using Cooperativa.App.Domain.Data;
using Cooperativa.App.Domain.Enum;
using Cooperativa.App.Domain.Model.People;
using Cooperativa.App.Domain.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Cooperativa.App.Domain.Model.Prestamos;
using Microsoft.EntityFrameworkCore;
using Cooperativa.App.Utilidades;
using Cooperativa.App.Domain.Model.Entidad;
using Microsoft.AspNetCore.Components.Forms;
using static Cooperativa.App.Domain.Model.Entidad.Nota;
using Mapster;

namespace Cooperativa.App.CRUD
{
    public class NotaCrud
    {
        public class Crear
        {
            public class Command : IRequest<AppResult>
            {
                public Guid? PrestamoId { get; set; }
                public Guid? PersonaId { get; set; }
                public TipoNota Tipo { get; set; }
                public string Titulo { get; set; }
                public string Contenido { get; set; }
            }

            public class CommandHandler : IRequestHandler<Command, AppResult>
            {
                private readonly CooperativaDbContext _context;

                public CommandHandler(CooperativaDbContext context)
                {
                    _context = context;
                }

                public async Task<AppResult> Handle(Command cmd, CancellationToken cancellationToken)
                {
                    try
                    {
                        var createdBy = new Guid("70E11ECF-657F-4AE8-A431-08DBA69C704A");

                        if(cmd.PersonaId != null && cmd.PersonaId != Guid.Empty)
                        {
                            var persona = await _context.Persona.Where(x => x.Id == cmd.PersonaId && !x.IsSoftDeleted).FirstOrDefaultAsync();
                            persona.ThrowIfNull("Persona no existe");
                        }
                        else if(cmd.PrestamoId != null && cmd.PrestamoId != Guid.Empty)
                        {
                            var prestamo = await _context.Prestamo.Where(x => x.Id == cmd.PrestamoId && !x.IsSoftDeleted).FirstOrDefaultAsync();
                            prestamo.ThrowIfNull("Prestamo no existe");
                        }
                        else
                        {
                            throw new Exception("Debe existir un elemento asociado para esta nota");
                        }

                        EstadoNota estado = new EstadoNota();


                        if(cmd.Tipo == TipoNota.Informativa)
                        {
                            estado = EstadoNota.Informativa;
                        }
                        else
                        {
                            estado = EstadoNota.Pendiente;
                        }

                        var newNota = Nota.New(cmd.PrestamoId, cmd.PersonaId, cmd.Titulo, cmd.Contenido, cmd.Tipo, estado, createdBy);


                        await _context.Nota.AddAsync(newNota);
                        await _context.SaveChangesAsync();

                        return AppResult.New(true, "Nota creada exitosamente");
                    }
                    catch (Exception ex)
                    {
                        return AppResult.New(false, ex.Message);
                    }
                }
            }
        }


        
        public class Eliminar
        {
            public class Command : IRequest<AppResult>
            {
                public List<Guid> Ids { get; set; }
                public Guid? UsuarioId { get; set; }
            }

            public class CommandHandler : IRequestHandler<Command, AppResult>
            {
                private readonly CooperativaDbContext _context;

                public CommandHandler(CooperativaDbContext context)
                {
                    _context = context;
                }

                public async Task<AppResult> Handle(Command cmd, CancellationToken cancellationToken)
                {
                    try
                    {
                        var createdBy = new Guid("70E11ECF-657F-4AE8-A431-08DBA69C704A");
                        if(cmd.UsuarioId == null || cmd.UsuarioId == Guid.Empty)
                        {
                            cmd.UsuarioId = createdBy;
                        }


                        var ids = cmd.Ids.Distinct().ToList();

                        var notas = await _context.Nota.Where(x => ids.Contains(x.Id) && !x.IsSoftDeleted).ToListAsync();

                        if(ids.Count() != notas.Count())
                        {
                            for(int i=0; i< ids.Count; i++)
                            {
                                var id = ids.ElementAt(i);
                                var existe = notas.Where(x => x.Id == id).Any();

                                if(!existe)
                                {
                                    throw new Exception($"No existe Nota con Id: {id}");
                                }
                            }
                        }


                        foreach(var nota in notas)
                        {
                            nota.Eliminar();
                            nota.ModifiedBy = (Guid)cmd.UsuarioId;
                        }


                        await _context.SaveChangesAsync();

                        return AppResult.New(true, $"{notas.Count()} Nota eliminadas exitosamente");
                    }
                    catch (Exception ex)
                    {
                        return AppResult.New(false, ex.Message);
                    }
                }
            }
        }




        public class AtenderNotaPendiente
        {
            public class Command : IRequest<AppResult>
            {
                public Guid Id { get; set; }
                public string Detalle { get; set; }
                public Guid? UsuarioId { get; set; }
            }

            public class CommandHandler : IRequestHandler<Command, AppResult>
            {
                private readonly CooperativaDbContext _context;

                public CommandHandler(CooperativaDbContext context)
                {
                    _context = context;
                }

                public async Task<AppResult> Handle(Command cmd, CancellationToken cancellationToken)
                {
                    try
                    {
                        var createdBy = new Guid("70E11ECF-657F-4AE8-A431-08DBA69C704A");
                        if (cmd.UsuarioId == null || cmd.UsuarioId == Guid.Empty)
                        {
                            cmd.UsuarioId = createdBy;
                        }

                        var nota = await _context.Nota.Where(x => x.Id == cmd.Id && !x.IsSoftDeleted).FirstOrDefaultAsync();
                        nota.ThrowIfNull("No se encontro Registro de Nota");

                        if(nota.Estado != EstadoNota.Pendiente)
                        {
                            throw new Exception("Nota no se encuentra en estado Pendiente de atender");
                        }

                        nota.Estado = EstadoNota.Resuelta;
                        nota.Estado_Descripcion = EstadoNotaDescripcion.GetEstadoTexto((int)EstadoNota.Resuelta);
                        nota.Detalle = cmd.Detalle;
                        nota.ModifiedBy = (Guid)cmd.UsuarioId;
                        nota.ModifiedDate = DateTime.Now;

                        await _context.SaveChangesAsync();

                        return AppResult.New(true, $"Nota atendida exitosamente");
                    }
                    catch (Exception ex)
                    {
                        return AppResult.New(false, ex.Message);
                    }
                }
            }
        }





        public class GetByPrestamosId
        {
            public class NotaRes
            {
                public Guid PrestamoId { get; set; }
                public List<NotaVm> Notas { get; set; }
            }

            public class Command : IRequest<List<NotaRes>>
            {
                public List<Guid> PrestamosId { get; set; }
            }

            public class CommandHandler : IRequestHandler<Command, List<NotaRes>>
            {
                private readonly CooperativaDbContext _context;

                public CommandHandler(CooperativaDbContext context)
                {
                    _context = context;
                }

                public async Task<List<NotaRes>> Handle(Command cmd, CancellationToken cancellationToken)
                {
                    var notas = await _context.Nota.Where(x => cmd.PrestamosId.Contains((Guid)x.PrestamoId))
                        .ProjectToType<NotaVm>()
                        .ToListAsync();

                    List<NotaRes> res = new List<NotaRes>();
                    var prestamosIds = notas.Select(X => X.PrestamoId).Distinct().ToList();

                    for(int i = 0; i < prestamosIds.Count; i++)
                    {
                        var prestamoId = prestamosIds.ElementAt(i);

                        var notasPrestamo = notas.Where(x => x.PrestamoId == prestamoId).ToList();

                        var newRes = new NotaRes
                        {
                            PrestamoId = (Guid)prestamoId,
                            Notas = notasPrestamo
                        };

                        res.Add(newRes);
                    }

                    return res;
                }
            }
        }




        public class GetByPersonasId
        {
            public class NotaRes
            {
                public Guid PersonaId { get; set; }
                public List<NotaVm> Notas { get; set; }
            }

            public class Command : IRequest<List<NotaRes>>
            {
                public List<Guid> PersonasId { get; set; }
            }

            public class CommandHandler : IRequestHandler<Command, List<NotaRes>>
            {
                private readonly CooperativaDbContext _context;

                public CommandHandler(CooperativaDbContext context)
                {
                    _context = context;
                }

                public async Task<List<NotaRes>> Handle(Command cmd, CancellationToken cancellationToken)
                {
                    var notas = await _context.Nota.Where(x => cmd.PersonasId.Contains((Guid)x.PersonaId))
                        .ProjectToType<NotaVm>()
                        .ToListAsync();

                    List<NotaRes> res = new List<NotaRes>();
                    var personasIds = notas.Select(X => X.PersonaId).Distinct().ToList();

                    for (int i = 0; i < personasIds.Count; i++)
                    {
                        var personaId = personasIds.ElementAt(i);

                        var notasPrestamo = notas.Where(x => x.PersonaId == personaId).ToList();

                        var newRes = new NotaRes
                        {
                            PersonaId = (Guid)personaId,
                            Notas = notasPrestamo
                        };
                        res.Add(newRes);
                    }


                    return res;
                }
            }
        }




        public class GetTiposNota
        {
            public class Command : IRequest<List<TipoNotaVm>>
            {

            }

            public class CommandHandler : IRequestHandler<Command, List<TipoNotaVm>>
            {
                private readonly CooperativaDbContext _context;

                public CommandHandler(CooperativaDbContext context)
                {
                    _context = context;
                }

                public async Task<List<TipoNotaVm>> Handle(Command cmd, CancellationToken cancellationToken)
                {
                    // Enumeramos todos los valores del enum
                    var tipos = Enum.GetValues(typeof(TipoNota))
                        .Cast<TipoNota>()
                        .Select(t => new TipoNotaVm
                        {
                            Id = (int)t,
                            Tipo_Descripcion = TipoNotaDescripcion.GetEstadoTexto((int)t)
                        })
                        .ToList();

                    return tipos;
                }
            }
        }




























        public class TipoNotaVm
        {
            public int Id { get; set; }
            public string Tipo_Descripcion { get; set; }
        }

    }
}
