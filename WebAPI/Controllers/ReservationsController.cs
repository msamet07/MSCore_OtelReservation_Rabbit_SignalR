
using Application.Services;
using Core.Entities;
using Infrastructure.Messaging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Hubs;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly ReservationService _service;
        private readonly IHubContext<ReservationHub> _hubContext;
        private readonly IMessageService _messageService;

        public ReservationsController(ReservationService service, IHubContext<ReservationHub> hubContext, IMessageService messageService)
        {
            _service = service;
            _hubContext = hubContext;
            _messageService = messageService;
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

            // SignalR ile gerçek zamanlı bildirim gönderme
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", $"New reservation created: {reservation.GuestName}, Room: {reservation.RoomNumber}");

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

