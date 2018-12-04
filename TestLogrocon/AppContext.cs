using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Threading.Tasks;

namespace TestLogrocon
{
    class AppContext : DbContext
    {
        public AppContext() : base("TestLogrocon.Properties.Settings.storedbConnectionString")
        {

        }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Order> Orders { get; set; }
    }
}
