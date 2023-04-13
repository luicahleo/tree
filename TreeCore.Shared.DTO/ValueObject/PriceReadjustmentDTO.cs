using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TreeCore.Shared.DTO.Utilities;
using TreeCore.Shared.ROP;
using TreeCore.Shared.Utilities.Enum;

namespace TreeCore.Shared.DTO.ValueObject
{
    public class PriceReadjustmentDTO : BaseDTO
    {
        [Required]
        public string Type { get; set; }
        public string CodeInflation { get; set; }
        public float? FixedAmount { get; set; }
        public float? FixedPercentage { get; set; }
        public float? Frequency { get; set; }

        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? NextDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        public PriceReadjustmentDTO()
        {
            map = new Dictionary<string, string>();
            map.Add(nameof(Type).ToLower(), "tipo");
            map.Add(nameof(CodeInflation).ToLower(), "inflacionid");
            map.Add(nameof(FixedAmount).ToLower(), "cantidadfija");
            map.Add(nameof(FixedPercentage).ToLower(), "porcentajefijo");
            map.Add(nameof(Frequency).ToLower(), "periodicidad");
            map.Add(nameof(EndDate).ToLower(), "fechafinreajuste");
            map.Add(nameof(StartDate).ToLower(), "fechainicioreajuste");
            map.Add(nameof(NextDate).ToLower(), "fechaproximareajuste");
        }

        public abstract class Readjustment : EnumerationDTO
        {
            protected Readjustment(string displayName) : base(displayName)
            {
            }

            public static readonly Readjustment PCI = new PCIType();
            public static readonly Readjustment FixedAmount = new FixedAmountType();
            public static readonly Readjustment FixedPercentege = new FixedPercentegeType();
            public static readonly Readjustment WithoutIncrements = new WithoutIncrementsType();

            public abstract List<Error> ValidateObject(PriceReadjustmentDTO prices);

            #region PCI
            private class PCIType : Readjustment
{
                public PCIType()
                    : base(PReadjustment.sPCI)
                { }

                public override List<Error> ValidateObject(PriceReadjustmentDTO prices)
                {
                    List<Error> lErrors = new List<Error>();

                    if (prices.StartDate == null)
                    {
                        lErrors.Add(Error.Create("Start date not found."));
                    }

                    if (prices.NextDate == null)
                    {
                        lErrors.Add(Error.Create("Next date not found."));
                    }

                    if (prices.CodeInflation == null)
                    {
                        lErrors.Add(Error.Create("Code inflation not found."));
                    }

                    return lErrors;
                }
            }
            #endregion

            #region FIXED AMOUNT
            private class FixedAmountType : Readjustment
            {
                public FixedAmountType()
                    : base(PReadjustment.sFixedAmount)
                { }

                public override List<Error> ValidateObject(PriceReadjustmentDTO prices)
                {
                    List<Error> lErrors = new List<Error>();

                    if (prices.StartDate == null)
                    {
                        lErrors.Add(Error.Create("Start date not found."));
                    }

                    if (prices.NextDate == null)
                    {
                        lErrors.Add(Error.Create("Next date not found."));
                    }

                    if (prices.EndDate == null)
                    {
                        lErrors.Add(Error.Create("End date not found."));
                    }

                    if (prices.Frequency == null || prices.Frequency == 0)
                    {
                        lErrors.Add(Error.Create("Cadence not found."));
                    }

                    if (prices.FixedAmount == null || prices.FixedAmount == 0)
                    {
                        lErrors.Add(Error.Create("Fixed amount not found."));
                    }

                    return lErrors;
                }
            }
            #endregion

            #region FIXED PERCENTEGE
            private class FixedPercentegeType : Readjustment
            {
                public FixedPercentegeType()
                    : base(PReadjustment.sFixedPercentege)
                { }

                public override List<Error> ValidateObject(PriceReadjustmentDTO prices)
                {
                    List<Error> lErrors = new List<Error>();

                    if (prices.StartDate == null)
                    {
                        lErrors.Add(Error.Create("Start date not found."));
                    }

                    if (prices.NextDate == null)
                    {
                        lErrors.Add(Error.Create("Next date not found."));
                    }

                    if (prices.EndDate == null)
                    {
                        lErrors.Add(Error.Create("End date not found."));
                    }

                    if (prices.Frequency == null || prices.Frequency == 0)
                    {
                        lErrors.Add(Error.Create("Cadence not found."));
                    }

                    if (prices.FixedPercentage == null || prices.FixedPercentage == 0)
                    {
                        lErrors.Add(Error.Create("Fixed percentege not found."));
                    }

                    return lErrors;
                }
            }
            #endregion

            #region WITHOUT INCREMENTS
            private class WithoutIncrementsType : Readjustment
            {
                public WithoutIncrementsType()
                    : base(PReadjustment.sWithoutIncrements)
                { }

                public override List<Error> ValidateObject(PriceReadjustmentDTO prices)
                {
                    List<Error> lErrors = new List<Error>();
                    return lErrors;
                }
            }
            #endregion
        }
    }
}
