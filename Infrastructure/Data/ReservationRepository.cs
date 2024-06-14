using Core.Entities; 
using Core.Interfaces;
using Microsoft.EntityFrameworkCore; 
using System.Collections.Generic; 
using System.Threading.Tasks;

namespace Infrastructure.Data 
{
    public class ReservationRepository : IReservationRepository // IReservationRepository arayüzünü uygulayan sınıf.
    {
        private readonly AppDbContext _context; // Veri tabanı bağlamını temsil eden özel alan.

        public ReservationRepository(AppDbContext context) // AppDbContext bağımlılığını alan kurucu metot.
        {
            _context = context;
        }

        public async Task<IEnumerable<Reservation>> GetAllAsync() // Tüm rezervasyonları asenkron olarak al.
        {
            return await _context.Reservations.ToListAsync();
        }

        public async Task<Reservation> GetByIdAsync(int id) // Belirtilen kimliğe sahip rezervasyonu asenkron olarak al.
        {
            return await _context.Reservations.FindAsync(id);
        }

        public async Task AddAsync(Reservation reservation) // Yeni bir rezervasyon ekle.
        {
            await _context.Reservations.AddAsync(reservation);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Reservation reservation) // Mevcut bir rezervasyonu güncelle.
        {
            _context.Reservations.Update(reservation);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id) // Belirtilen kimliğe sahip rezervasyonu sil.
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation != null)
            {
                _context.Reservations.Remove(reservation);
                await _context.SaveChangesAsync();
            }
        }
    }
}
