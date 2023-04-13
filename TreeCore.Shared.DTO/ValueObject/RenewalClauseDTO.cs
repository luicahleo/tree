using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TreeCore.Shared.Utilities.Enum;
using TreeCore.Shared.DTO.Utilities;
using TreeCore.Shared.ROP;


namespace TreeCore.Shared.DTO.ValueObject
{
    public class RenewalClauseDTO : BaseDTO
    {

        
        public string Type { get; set; }
        public int? Frequencymonths { get; set; }
        public int? TotalRenewalNumber { get; set;}


        [Editable(false)] public int? CurrentRenewalNumber { get; set;}
        public int? NotificationNumberDays { get; set; }
        [Editable(false)] public DateTime? RenewalExpirationDate { get; set; }

         public DateTime? Renewalnotificationdate { get; set; }

        [Editable(false)] public DateTime? ContractExpirationDate { get; set; }

       
        public RenewalClauseDTO()
        {
            map = new Dictionary<string, string>();
            map.Add(nameof(Type).ToLower(), "tipo");
            map.Add(nameof(Frequencymonths).ToLower(), "CadenciaProrrogaAutomatica");
            map.Add(nameof(TotalRenewalNumber).ToLower(), "NumProrrogasMaximas");
            map.Add(nameof(CurrentRenewalNumber).ToLower(), "NumProrrogasConsumidas");
            map.Add(nameof(NotificationNumberDays).ToLower(), "NumDiasNotificaciones");
            map.Add(nameof(RenewalExpirationDate).ToLower(), "FechaFinContratoAuxiliar");
            map.Add(nameof(Renewalnotificationdate).ToLower(), "FechaRegeneracionPrevisiones");
            map.Add(nameof(ContractExpirationDate).ToLower(), "fechaefectivainContrato");

        }

        public abstract class Renewal : EnumerationDTO
        {
            protected Renewal(string displayName) : base(displayName)
            {
            }

            public static readonly Renewal Optional = new OptionalType();
            public static readonly Renewal Auto = new AutoType();
            public static readonly Renewal FixedPercentege = new PreviousNegotiation();
           

            public abstract List<Error> ValidateObject(RenewalClauseDTO renewal);

            #region OPTIONAL
            private class OptionalType : Renewal
            {
                public OptionalType()
                    : base(RenewalReadjustment.sOptional)
                { }

                public override List<Error> ValidateObject(RenewalClauseDTO renewal)
                {
                    List<Error> lErrors = new List<Error>();

                    if (renewal.Frequencymonths == null)
                    {
                        lErrors.Add(Error.Create("Frequency month not found."));
                    }

                    if (renewal.TotalRenewalNumber == null)
                    {
                        lErrors.Add(Error.Create("Total Renewal Number not found."));
                    }

                    if (renewal.CurrentRenewalNumber == null)
                    {
                        lErrors.Add(Error.Create("Current Renewal Number not found."));
                    }

                    if (renewal.RenewalExpirationDate == null)
                    {
                        lErrors.Add(Error.Create("Renewal Date not found."));
                    }
                    if (renewal.ContractExpirationDate == null)
                    {
                        lErrors.Add(Error.Create("Expiration Date not found."));
                    }

                    return lErrors;
                }
            }
            #endregion

            #region AUTO
            private class AutoType : Renewal
            {
                public AutoType()
                    : base(RenewalReadjustment.sAuto)
                { }

                public override List<Error> ValidateObject(RenewalClauseDTO renewal)
                {
                    List<Error> lErrors = new List<Error>();

                    if (renewal.Frequencymonths == null)
                    {
                        lErrors.Add(Error.Create("Frequency month not found."));
                    }

                    if (renewal.TotalRenewalNumber == null)
                    {
                        lErrors.Add(Error.Create("Total Renewal Number not found."));
                    }

                    if (renewal.CurrentRenewalNumber == null)
                    {
                        lErrors.Add(Error.Create("Current Renewal Number not found."));
                    }

                    if (renewal.RenewalExpirationDate == null)
                    {
                        lErrors.Add(Error.Create("Renewal Date not found."));
                    }
                    if (renewal.ContractExpirationDate == null)
                    {
                        lErrors.Add(Error.Create("Expiration Date not found."));
                    }

                    return lErrors;
                }
            }
            #endregion

            #region PREVIOUS NEGOTIATION
            private class PreviousNegotiation : Renewal
            {
                public PreviousNegotiation()
                    : base(RenewalReadjustment.sPreviousNegotiation)
                { }

                public override List<Error> ValidateObject(RenewalClauseDTO prices)
                {
                    List<Error> lErrors = new List<Error>();

                    
                    return lErrors;
                }
            }
            #endregion

            
        }
    }
}

