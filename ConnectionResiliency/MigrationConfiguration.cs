using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgahClassLibrary;

namespace ConnectionResiliency
{
    public class MigrationConfiguration : DbMigrationsConfiguration<TestDbModel>
    {
        public MigrationConfiguration()
        {
            AutomaticMigrationDataLossAllowed = true;
            AutomaticMigrationsEnabled = true;
        }
    }
   
}
