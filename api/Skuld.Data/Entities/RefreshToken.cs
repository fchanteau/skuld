using System;
using System.Collections.Generic;

#nullable disable

namespace Skuld.Data.Entities
{
    public partial class RefreshToken
    {
        public decimal RefreshTokenId { get; set; }
        public string Value { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ExpiredDate { get; set; }
        public decimal UserId { get; set; }

        public virtual User User { get; set; }
    }
}
