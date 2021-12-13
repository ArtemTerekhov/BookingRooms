namespace BookingRooms.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public int RoomId { get; set; }
    }
}
