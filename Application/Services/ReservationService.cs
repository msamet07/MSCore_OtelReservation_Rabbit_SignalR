
using Core.Entities;
using Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ReservationService
    {
        private readonly IReservationRepository _repository;

        public ReservationService(IReservationRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<Reservation>> GetAllReservationsAsync()
        {
            return _repository.GetAllAsync();
        }

        public Task<Reservation> GetReservationByIdAsync(int id)
        {
            return _repository.GetByIdAsync(id);
        }

        public Task AddReservationAsync(Reservation reservation)
        {
            return _repository.AddAsync(reservation);
        }

        public Task UpdateReservationAsync(Reservation reservation)
        {
            return _repository.UpdateAsync(reservation);
        }

        public Task DeleteReservationAsync(int id)
        {
            return _repository.DeleteAsync(id);
        }
    }
}

