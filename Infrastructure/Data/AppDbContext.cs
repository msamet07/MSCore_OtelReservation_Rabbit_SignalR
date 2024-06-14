using Core.Entities;
using Microsoft.EntityFrameworkCore; 

namespace Infrastructure.Data 
{
    public class AppDbContext : DbContext // Entity Framework Core'un DbContext sınıfından türeyen sınıf.
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { } // Yapılandırma seçeneklerini alan ve üst sınıfa ileten kurucu metot.

        public DbSet<Reservation> Reservations { get; set; } // Rezervasyonlar için DbSet.
    }
}
