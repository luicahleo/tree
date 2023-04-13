using TreeCore.BackEnd.Model.Entity.General;

namespace TreeCore.BackEnd.Model.Entity.Companies
{
    public class CompanyAssignedPaymentMethodsEntity: BaseEntity
    {
        public int? EntidadMetodoPagoID;
        public CompanyEntity? CoreCompany;
        public bool Defecto;
        public PaymentMethodsEntity CoreMetodosPagos;

        public CompanyAssignedPaymentMethodsEntity(int? entidadMetodoPagoID, CompanyEntity? coreCompany,
            PaymentMethodsEntity coreMetodosPagos, bool defecto)
        {
            EntidadMetodoPagoID = entidadMetodoPagoID;
            CoreCompany = coreCompany;
            CoreMetodosPagos = coreMetodosPagos;
            Defecto = defecto;
        }

        protected CompanyAssignedPaymentMethodsEntity() { }

        public static CompanyAssignedPaymentMethodsEntity UpdateId(CompanyAssignedPaymentMethodsEntity PaymentMethodsAssigned, int id) =>
            new CompanyAssignedPaymentMethodsEntity(id, PaymentMethodsAssigned.CoreCompany, PaymentMethodsAssigned.CoreMetodosPagos, PaymentMethodsAssigned.Defecto);
    }
}
