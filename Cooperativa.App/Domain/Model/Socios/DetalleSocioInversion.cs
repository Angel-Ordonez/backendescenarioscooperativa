using Cooperativa.App.Domain.Model.Prestamos;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cooperativa.App.Domain.Model.Socios
{
    public class DetalleSocioInversion : EntityBasic
    {

        #region foreigkey
        public virtual SocioInversion SocioInversion { get; set; }
        public Guid SocioInversionId { get; set; }
        public virtual Prestamo Prestamo { get; set; }
        public Guid PrestamoId { get; set; }

        public virtual GananciaDetalleSocio GananciaDetalleSocio { get; set; }
        public Guid? GananciaDetalleSocioId { get; set; }
        #endregion


        #region properties
        public decimal Habia { get; set; }
        public decimal SePresto { get; set; }
        public decimal Quedan { get; set; }
        public decimal PorcentajeEnPrestamo { get; set; }                   //Que porcentaje ocupo en cantidad dentro del prestamo
        public decimal CantidadPagadaDePrestamo { get; set; }           //La cantidad de prestamo que ha pagado el cliente basado en capital
        public decimal Ganancia { get; set; }
        
        #endregion



        #region Methods

        public static DetalleSocioInversion New (Guid socioInversionId, Guid prestamoId,  decimal habia, decimal sePresto, decimal quedan, Guid createdBy)
        {
            var newDetalle = new DetalleSocioInversion
            {
                SocioInversionId = socioInversionId,
                PrestamoId = prestamoId,
                Quedan = quedan,
                Habia = habia,
                SePresto = sePresto,
                CreatedBy = createdBy,
                Enabled = true,
                CreatedDate = DateTime.Now,
            };

            return newDetalle;
        }

        #endregion


        #region nav
        public virtual ICollection<MovimientoDetalleSocio> MovimientosDetalleSocio { get; set; }
        //public virtual ICollection<GananciaDetalleSocio> GananciasDetalle  { get; set; }        //De aqui solo se toma la ultima....  lo termine haciendo como llave foranea... sustituire

        public DetalleSocioInversion()
        {
           MovimientosDetalleSocio= new HashSet<MovimientoDetalleSocio>();
        }

        #endregion


    }
}
