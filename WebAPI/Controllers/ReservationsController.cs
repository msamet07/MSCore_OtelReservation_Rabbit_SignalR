using Application.Services;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Messaging;
using Infrastructure.Email;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Hubs;


namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationService _service;
        private readonly IMessageService _messageService;
        private readonly IEmailService _emailService;
        private readonly IReservationRepository _reservationRepository;

        public ReservationsController(IReservationService service, IMessageService messageService, IEmailService emailService, IReservationRepository reservationRepository)
        {
            _service = service;
            _messageService = messageService;
            _emailService = emailService;
            _reservationRepository = reservationRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reservation>>> GetReservations()
        {
            return Ok(await _service.GetAllReservationsAsync());
        }

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

        [HttpPost]
        public async Task<ActionResult> PostReservation(Reservation reservation)
        {
            await _service.AddReservationAsync(reservation);

            // RabbitMQ'ya mesaj gönderme
            _messageService.SendMessage($"New reservation created: {reservation.GuestName}, Room: {reservation.RoomNumber}");

            // E-posta gönderme
            await _emailService.SendEmailAsync("recipient@example.com", "New Reservation", $"A new reservation has been created for {reservation.GuestName} in room {reservation.RoomNumber}.");

            return CreatedAtAction(nameof(GetReservation), new { id = reservation.Id }, reservation);
        }

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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReservation(int id)
        {
            await _service.DeleteReservationAsync(id);
            return NoContent();
        }
    }
}
