using Application.Services; 
using Core.Entities; 
using Core.Interfaces; 
using Infrastructure.Messaging; 
using Infrastructure.Email; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR; 
using System.Collections.Generic; 
using WebAPI.Hubs;
using System.Threading.Tasks;

namespace WebAPI.Controllers 
{
    [Route("api/[controller]")] // API rotası
    [ApiController] // API kontrolcüsü 
    public class ReservationsController : ControllerBase // MVC kontrolcüsü sınıfı.
    {
        private readonly ReservationService _service; // Rezervasyon servisi.
        private readonly IHubContext<ReservationHub> _hubContext; // SignalR hub'ı.
        private readonly IMessageService _messageService; // Mesajlaşma servisi.
        private readonly IEmailService _emailService; // E-posta servisi.
        private readonly IReservationRepository _reservationRepository; // Rezervasyon repository'si.

        // Constructor
        public ReservationsController(ReservationService service, IHubContext<ReservationHub> hubContext, IMessageService messageService, IEmailService emailService, IReservationRepository reservationRepository)
        {
            _service = service;
            _hubContext = hubContext;
            _messageService = messageService;
            _emailService = emailService;
            _reservationRepository = reservationRepository;
        }

        // GET: api/reservations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reservation>>> GetReservations()
        {
            return Ok(await _service.GetAllReservationsAsync());
        }

        // GET: api/reservations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Reservation>> GetReservation(int id)
        {
            var reservation = await _service.GetReservationByIdAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }

            return Ok(reservation);
        }

        // POST: api/reservations
        [HttpPost]
        public async Task<ActionResult> PostReservation(Reservation reservation)
        {
            await _service.AddReservationAsync(reservation);

            // RabbitMQ'ya mesaj gönderme
            _messageService.SendMessage($"New reservation created: {reservation.GuestName}, Room: {reservation.RoomNumber}");

            // SignalR ile gerçek zamanlı bildirim gönderme
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", $"New reservation created: {reservation.GuestName}, Room: {reservation.RoomNumber}");

            // E-posta gönderme
            await _emailService.SendEmailAsync("recipient@example.com", "New Reservation", $"A new reservation has been created for {reservation.GuestName} in room {reservation.RoomNumber}.");

            return CreatedAtAction(nameof(GetReservation), new { id = reservation.Id }, reservation);
        }

        // PUT: api/reservations/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReservation(int id, Reservation reservation)
        {
            if (id != reservation.Id)
            {
                return BadRequest();
            }

            await _service.UpdateReservationAsync(reservation);
            return NoContent();
        }

        // DELETE: api/reservations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReservation(int id)
        {
            await _service.DeleteReservationAsync(id);
            return NoContent();
        }
    }
}
