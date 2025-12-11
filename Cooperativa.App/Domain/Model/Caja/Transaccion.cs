using Cooperativa.App.Domain.Enum;
using Cooperativa.App.Domain.Model.People;
using Cooperativa.App.Domain.Model.Socios;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cooperativa.App.Domain.Model.Caja
{
    public class Transaccion : EntityBasic
    {

        public int NumeroTransaccion { get; set; }          //Seria por año
        public string Referencia { get; set; }              //Seria el Correlativo Ingreso o de Egreso
        public TipoTransaccion TipoTransaccion { get; set; }
        public decimal Monto { get; set; }
        public string Moneda_Descripcion { get; set; }
        public decimal SaldoCajaEnElMomento { get; set; }
        public decimal SaldoQuedaEnCaja { get; set; }

        #region foreigkey
        public virtual Caja Caja { get; set; }
        public Guid? CajaId { get; set; }
        public virtual Ingreso Ingreso { get; set; }
        public Guid? IngresoId { get; set; }
        public virtual Egreso Egreso { get; set; }
        public Guid? EgresoId { get; set; }


        public virtual CuentaBancaria CuentaBancariaOrigen { get; set; }
        public Guid? CuentaBancariaOrigenId { get; set; }
        public virtual CuentaBancaria CuentaBancariaDestino { get; set; }   //Que pasa si icupa dinero de efectivo y BAC para completar una cantidad? Simplemente mueve el dinero para un sola cuenta
        public Guid? CuentaBancariaDestinoId { get; set; }
        //Lo que estoy pensando es que qui serian dos cuentas bancarias... De donde proviene y la que recibe
        #endregion


        #region publicMethods
        public static Transaccion New( int numeroTransaccion, string referencia, decimal monto, Guid? cuentaBancariaId, Guid createdby)
        {
            var newTransaccion = new Transaccion
            {
                NumeroTransaccion = numeroTransaccion,
                Referencia = referencia,
                Monto = monto,
                //CuentaBancariaId = cuentaBancariaId,
                Moneda_Descripcion = "HNL",


                CreatedBy = createdby,
                CreatedDate = DateTime.Now,
                IsSoftDeleted = false,
                Enabled = true
            };

            return newTransaccion;
        }

        public static Transaccion New(int numeroTransaccion, string referencia, decimal monto, Guid? cuentaBancariaOrigenId, Guid? cuentaBancariaDestinoId, Guid createdby)
        {
            var newTransaccion = new Transaccion
            {
                NumeroTransaccion = numeroTransaccion,
                Referencia = referencia,
                Monto = monto,
                CuentaBancariaOrigenId = cuentaBancariaOrigenId,
                CuentaBancariaDestinoId = cuentaBancariaDestinoId,
                Moneda_Descripcion = "HNL",


                CreatedBy = createdby,
                CreatedDate = DateTime.Now,
                IsSoftDeleted = false,
                Enabled = true
            };

            return newTransaccion;
        }
        #endregion


    }
}
