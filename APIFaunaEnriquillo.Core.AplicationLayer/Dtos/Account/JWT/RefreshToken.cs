﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIFaunaEnriquillo.Core.AplicationLayer.Dtos.Account.JWT
{
    internal class RefreshToken
    {
        public string UserId { get; set; }
        public string? Token { get; set; }
        public DateTime Expired { get; set; }
        public bool IsExpired => DateTime.UtcNow > Expired;
        public DateTime Created { get; set; }
        public DateTime Revoked { get; set; }
        public string? ReplacedByToken { get; set; }
        public bool IsActive => Revoked == null && !IsExpired;

    }
}
