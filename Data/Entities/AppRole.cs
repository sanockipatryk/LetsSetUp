﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LetsSetUp.Data.Entities
{

    public class AppRole : IdentityRole<int>
    {
        public AppRole() { }
        public AppRole(string name)
        {
            Name = name;
        }
    }
}
