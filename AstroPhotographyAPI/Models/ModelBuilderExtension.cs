using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AstroPhotographyAPI.Models
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DBPhoto>().HasData(
                new DBPhoto
                {
                    Id = 1,
                    PhotoName = "M42 Orion Nebula",
                    PhotoLocation = "Anza OCA Site",
                    PhotoPath = "",
                    Description = "11.23.2019 - 33m Exposure time",
                    MainCamera = "Canon EOS T7i",
                    MainTelescope = "Orion ED80 CF Triplet",
                    GuideCamera = "QHY5L-II",
                    GuideScope = "Orion 50mm Mini",
                    Filters = "None",
                    Other = "Orion .8x Focal Reducer",
                    Mount = "Skywatcher EQ6r-Pro"
                });
        }
    }
}
