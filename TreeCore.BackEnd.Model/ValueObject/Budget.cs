using System;
using System.Collections.Generic;
using TreeCore.BackEnd.Model.Entity.General;

namespace TreeCore.BackEnd.Model.ValueObject
{
    public class Budget : BaseValueObject
    {
        public CurrencyEntity BudgetMoneda { get; set; }
        public float BudgetValor { get; set; }

        public Budget(CurrencyEntity moneda, float valor)
        {
            this.BudgetMoneda = moneda;
            this.BudgetValor = valor;
        }

        protected Budget() { }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return BudgetMoneda;
            yield return BudgetValor;
        }
    }
}
