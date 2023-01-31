﻿using Microsoft.AspNetCore.Identity;

namespace FirstProject.AuthAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
    }
}
