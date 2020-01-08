using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace HealthHerbs
{
    class HerbsContext : DbContext
    {
        public HerbsContext() : base("DBConnection") { }
        public DbSet<Herbs> Herbs { get; set; }
    }
    public class Herbs
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public byte[] Photo { get; set; }
    }
}
