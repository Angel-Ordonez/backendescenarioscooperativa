using Cooperativa.App.Domain.Enum;
using Cooperativa.App.Domain.Model.Prestamos;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cooperativa.App.Domain.Model.People
{
    public class Cliente : Persona
    {

        #region propiedades
        public bool PrestamoActivo { get; set; }
        public int? CantidadPrestamos { get; set; }
        public string RecomendadoPor { get; set; }
        public string Nota { get; set; }
        public bool ClienteBueno { get; set; }
        public EstadoPersona Estado { get; set; }
        #endregion


        #region nav
        public virtual ICollection<Prestamo> Prestamos { get; set; }

        #endregion


        #region
        public Cliente()
        {
            Prestamos = new HashSet<Prestamo>();
        }
        #endregion






        #region publicMethods
        public static Cliente New(string nombre, string apellido, string tipoIdentificacion, string identificacion, DateTime fechaNacimiento, string estadoCivil, string genero, string lugarTrabajo, string ocupacion, string recomendadopor, string Observacion,
            string pais, string ciudad, string direccion, string correo, string telefono, string telefono2, string otrocontacto, string rtn, string codigoPersona, Guid createdby)
        {
            var cliente = new Cliente
            {
                Nombre = nombre,
                Apellido = apellido,
                TipoIdentificacion = tipoIdentificacion,
                Identificacion = identificacion,
                Pais = pais,
                Ciudad = ciudad,
                Direccion = direccion,
                Correo = correo,
                Telefono = telefono,
                Telefono2 = telefono2,
                OtroContacto = otrocontacto,
                RTN = rtn,
                Observacion = Observacion,
                CreatedBy = createdby,
                CreatedDate = DateTime.Now,
                TieneUsuario = false,
                PrestamoActivo = false,
                ClienteBueno = false,
                FechaNacimiento = fechaNacimiento,
                Edad = DateTime.Now.Year - fechaNacimiento.Year,
                EstadoCivil = estadoCivil,
                Genero = genero,
                LugarTrabajo = lugarTrabajo,
                Ocupacion = ocupacion,
                RecomendadoPor = recomendadopor,
                Nota = null,
                Estado = EstadoPersona.Activo,
                CodigoPersona = codigoPersona,

                IsSoftDeleted = false,
                Enabled = true
            };



            return cliente;
        }
        #endregion


    }
}
