using Cooperativa.App.Domain.Data;
using Cooperativa.App.Domain.Model;
using Cooperativa.App.Engine;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Cooperativa.App.CRUD.SocioInversionCrud;

namespace Cooperativa.App.CRUD
{
    public class NotificacionCrud
    {

        public class EnviarCorreo
        {

            public class CommandCorreoGamil : IRequest<AppResult>
            {
                public string Titulo { get; set; }
                public string Body { get; set; }
                public List<string> correos { get; set; }
            }
            public class CommandHandlerSocioInversionBySocioAndAnio : IRequestHandler<CommandCorreoGamil, AppResult>
            {
                private readonly CooperativaDbContext _context;

                public CommandHandlerSocioInversionBySocioAndAnio(CooperativaDbContext context)
                {
                    _context = context;
                }

                public async Task<AppResult> Handle(CommandCorreoGamil command, CancellationToken cancellationToken)
                {

                    var lista = await _context.Prestamo.ToListAsync();

                    try
                    {

                        // Construye el cuerpo del correo en formato HTML
                        string body = "";
                        body += "<tr><td> " + "Hola Estimado colaborador." + "</td></tr>";
                        body += "<tr><td> " + "Se le notifica que las siguientes gestiones se han visto afectadas por evento: " + "Huelgas " + " en Sitio: " + "Aduana el Amatillo HN" + "</td></tr>";

                        body += "<table style='border-collapse: collapse; width: 70%; border: 1px solid #ccc;'>";
                        body += "<tr><th style='border: 1px solid #ccc; padding: 5px;'>Codigo</th><th style='border: 1px solid #ccc; padding: 5px;'>Nombre del Cliente</th></tr>";

                        foreach (var dato in lista)
                        {
                            body += "<tr><td style='border: 1px solid #ccc; padding: 3px;'>" + dato.CodigoPrestamo + "</td><td style='border: 1px solid #ccc; padding: 3px;'>" + dato.ClienteNombre + "</td></tr>";
                        }

                        body += "</table>";
                        body += "<br><br>";
                        body += "<tr><td> " + "Este correo electrónico es una notificación automática." + "</td></tr>";
                        body += "<tr><td> " + "Saludos cordiales." + "</td></tr>";




                        NotificacionesEngine.CrearCorreo(command.correos, body, command.Titulo);

                        return AppResult.New(true, "Fnciono");
                    }
                    catch (Exception ex) 
                    {
                        return AppResult.New(false, ex.Message);
                    }

                }
            }
        }

















    }
}
