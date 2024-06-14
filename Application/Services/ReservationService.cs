using Core.Entities; 
using Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    // ReservationService sınıfı, rezervasyon işlemleri için servis katmanını temsil eder.
    public class ReservationService
    {
        // Rezervasyon verilerini yönetmek için IReservationRepository türünde bir alan tanımlıyorum.
        private readonly IReservationRepository _repository;

        // Constructor, IReservationRepository bağımlılığını enjeksiyon yoluyla alır.
        public ReservationService(IReservationRepository repository)
        {
            _repository = repository;
        }

        // Tüm rezervasyonları getirir.
        public Task<IEnumerable<Reservation>> GetAllReservationsAsync()
        {
            return _repository.GetAllAsync();
        }

        // Belirli bir ID'ye sahip rezervasyonu  getirir.
        public Task<Reservation> GetReservationByIdAsync(int id)
        {
            return _repository.GetByIdAsync(id);
        }

        // Yeni bir rezervasyon ekler.
        public Task AddReservationAsync(Reservation reservation)
        {
            return _repository.AddAsync(reservation);
        }

        // Mevcut bir rezervasyonu  günceller.
        public Task UpdateReservationAsync(Reservation reservation)
        {
            return _repository.UpdateAsync(reservation);
        }

        // Belirli bir ID'ye sahip rezervasyonu siler.
        public Task DeleteReservationAsync(int id)
        {
            return _repository.DeleteAsync(id);
        }
    }
}
