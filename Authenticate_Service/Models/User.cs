﻿using System;
using System.Collections.Generic;

namespace Authenticate_Service.Models
{
    public partial class User
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? LastName { get; set; }
        public string? ProfilePict { get; set; }
        public bool? Status { get; set; }
        public int? RoleId { get; set; }
        public string? UserName { get; set; }
        public bool? EmailConfirmed { get; set; }
        public string? VerificationCode { get; set; }

        public virtual Role? Role { get; set; }
    }
}
