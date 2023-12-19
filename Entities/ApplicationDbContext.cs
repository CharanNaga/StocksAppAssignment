using Microsoft.EntityFrameworkCore;
namespace Entities
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions options):base(options)
        {

        }
        public virtual DbSet<BuyOrder> BuyOrders { get; set; }
        public virtual DbSet<SellOrder> SellOrders { get;set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //binding with Dbsets with tables
            modelBuilder.Entity<BuyOrder>().ToTable(nameof(BuyOrders));
            modelBuilder.Entity<SellOrder>().ToTable(nameof(SellOrders));
        }
    }
}
