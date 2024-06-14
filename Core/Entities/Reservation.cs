using System; 

namespace Core.Entities
{
    // Reservation sınıfı, bir rezervasyonu temsil eder.
    public class Reservation
    {
        // ID
        public int Id { get; set; }

        // Misafirin adı.
        public string GuestName { get; set; }

        // Giriş tarihi.
        public DateTime CheckInDate { get; set; }

        // Çıkış tarihi.
        public DateTime CheckOutDate { get; set; }

        // Oda numarası.
        public int RoomNumber { get; set; }
    }
}
