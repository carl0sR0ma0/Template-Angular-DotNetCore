using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Template.Domain.Entities;

namespace Template.Data.Extensions
{
    public static class ModelBuilderExtension
    {
        public static ModelBuilder SeedData(this ModelBuilder builder)
        {
            builder.Entity<User>()
                .HasData(
                    new User { Id = Guid.Parse("2952167c-35db-4113-be60-581df67eefab"), Name = "User Default", Email = "userdafult@template.com", DateCreated = new DateTime(2020,2,2), IsDeleted = false, DateUpdated = null }
                );

            return builder;
        }
    }
}
