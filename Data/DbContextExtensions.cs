using LetsSetUp.Data.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LetsSetUp.Data
{
    public static class DbContextExtensions
    {
        public static RoleManager<AppRole> RoleManager { get; set; }

        public static UserManager<AppUser> UserManager { get; set; }
        public static void EnsureSeeded(this LetsSetUpContext context)
        {
            AddRoles(context);
            AddUsers(context);
            AddCategories(context);
            AddEvents(context);
        }

        private static void AddUsers(LetsSetUpContext context)
        {
            if (UserManager.FindByEmailAsync("admin@admin.pl").
           GetAwaiter().GetResult() == null)
            {
                var user = new AppUser
                {
                    FirstName = "admin",
                    LastName = "admin",
                    UserName = "admin@admin.pl",
                    Email = "admin@admin.pl",
                    City = "Krosno",
                    EmailConfirmed = true,
                    LockoutEnabled = false
                };
                UserManager.CreateAsync(user, "Q!w2e3").GetAwaiter().GetResult();
            }
            var admin = UserManager.FindByEmailAsync("admin@admin.pl").GetAwaiter().GetResult();
            if (UserManager.IsInRoleAsync(admin, "Admin").GetAwaiter().GetResult() == false)
            {
                UserManager.AddToRoleAsync(admin, "Admin");
            }
        }

        private static void AddRoles(LetsSetUpContext context)
        {
            if (RoleManager.RoleExistsAsync("Admin").GetAwaiter()
            .GetResult() == false)
            {
                RoleManager.CreateAsync(new
                AppRole("Admin")).GetAwaiter().GetResult();
            }
            if (RoleManager.RoleExistsAsync("Customer")
            .GetAwaiter().GetResult() == false)
            {
                RoleManager.CreateAsync(new
                AppRole("Customer")).GetAwaiter().GetResult();
            }
        }

        private static void AddCategories(LetsSetUpContext context)
        {
            if (context.Categories.Any() == false)
            {
                var categories = new List<string>()
                {"Piłka nożna", "Koszykówka", "Siatkówka", "Piłka ręczna", "Bieganie", "Jazda na rowerze", "Jazda na rolkach", "Szachy", "Jazda na łyżwach", "Hokej", "Tenis", "Tenis stołowy", "Pływanie"};
                categories.ForEach(c => context.Add(new Category
                {
                    Name = c
                }));
                context.SaveChanges();
            }
        }

        private static void AddEvents(LetsSetUpContext context)
        {
            if (context.Events.Any() == false)
            {
                var events = new List<Event>()
                {
                    new Event
                        {
                        Name = "Amatorski mecz piłki nożnej",
                        Slug = "amatorski-mecz-pilki-noznej",
                        Street = "",
                        DateCreated = DateTime.Now,
                        DateStarts = DateTime.Now,
                        DateEnds = DateTime.Now,
                        CategoryId = 1,
                        CreatedBy = 1,
                        PeopleNeeded = 10,
                        City = "Krosno",
                        ObjectName = "Mosir",
                        EventDescription = "amatorski mecz piłki nożnej na sali mosir krosno"
                        }
                        };
                context.Events.AddRange(events);
                context.SaveChanges();
            }
        }


    }
}
