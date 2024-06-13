using System;

namespace Core.Entities
{
    public class Reservation
    {
        public int Id { get; set; }
        public string GuestName { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int RoomNumber { get; set; }
    }
}
