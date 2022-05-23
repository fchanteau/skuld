﻿using System;
using System.Collections.Generic;

#nullable disable

namespace Skuld.Data.Entities
{
    public partial class User
    {
        public User()
        {
            RefreshTokens = new HashSet<RefreshToken>();
        }

        public decimal UserId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Role { get; set; }

        public virtual ICollection<RefreshToken> RefreshTokens { get; set; }
    }
}
