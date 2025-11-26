using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLPDemo.Database
{
    public class ApplicationDbContext :DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            // Database seeding or configuration can be done here

        }
        public DbSet<tblProperty> tblproperty { get; set; }
        public DbSet<tblVillageInfo> tblvillageinfo { get; set; }
    }
}
