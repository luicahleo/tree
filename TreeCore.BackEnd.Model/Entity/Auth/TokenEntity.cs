using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeCore.BackEnd.Model.Entity.Auth
{
    public class TokenEntity : BaseEntity
    {
        public readonly int UserId;
        public string AuthToken;
        public readonly DateTime IssuedOn;
        public readonly DateTime ExpiresOn;

        public TokenEntity(int userId, string authToken, DateTime issuedOn, DateTime expiresOn)
        {
            UserId = userId;
            AuthToken = authToken;
            IssuedOn = issuedOn;
            ExpiresOn = expiresOn;
        }
    }
}
