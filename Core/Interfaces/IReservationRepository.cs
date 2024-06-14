using System.Collections.Generic; 
using System.Threading.Tasks; 
using Core.Entities; 

namespace Core.Interfaces 
{
    public interface IReservationRepository
    {
        Task<IEnumerable<Reservation>> GetAllAsync(); // Tüm rezervasyonları asenkron olarak al.
        Task<Reservation> GetByIdAsync(int id); // Belirtilen kimliğe sahip rezervasyonu asenkron olarak al.
        Task AddAsync(Reservation reservation); // Yeni bir rezervasyon ekle.
        Task UpdateAsync(Reservation reservation); // Mevcut bir rezervasyonu güncelle.
        Task DeleteAsync(int id); // Belirtilen kimliğe sahip rezervasyonu sil.
    }
}
