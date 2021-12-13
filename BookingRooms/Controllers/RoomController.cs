using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BookingRooms.Models;

namespace BookingRooms.Controllers
{
    [ApiController]
    public class RoomController : ControllerBase
    {
        ApplicationContext db;

        public RoomController(ApplicationContext context)
        {
            db = context;

        }

        [HttpGet("/api/getrooms")]
        public async Task<IActionResult> GetRooms()
        {
            IEnumerable<Room> rooms = await db.Rooms.ToListAsync();

            if (rooms != null)
            {
                return Ok(rooms);
            }

            return BadRequest(new { errorText = "Нет ни одного номера" });
        }

        [HttpGet("/api/getroom/{roomid}")]
        public async Task<IActionResult> GetRoom(int roomId)
        {
            var room = await db.Rooms.FirstOrDefaultAsync(r => r.RoomId == roomId);

            if (room != null)
            {
                return Ok(room);
            }

            return BadRequest(new { errorText = "Номер не найден" });
        }

        [HttpPost("/api/newroom/")]
        public async Task<IActionResult> NewRoom([FromBody] Room room)
        {
            if (room != null)
            {
                db.Rooms.Add(room);

                await db.SaveChangesAsync();

                return Ok(room);
            }

            return BadRequest(new { errorText = "Некорректные исходные данные" });
        }

        [HttpPut("/api/updateroom/")]
        [Authorize]
        public async Task<IActionResult> UpdateRoom([FromBody] Room room)
        {
            if (room == null)
            {
                return BadRequest(new { errorText = "Некорректные данные." });
            }
            if (!db.Rooms.Any(r => r.RoomId == room.RoomId))
            {
                return NotFound(new { errorText = "Номер не найден." });
            }

            db.Update(room);
            await db.SaveChangesAsync();

            return Ok(room);
        }

        [HttpDelete("/api/deleteroom/{roomid}")]
        [Authorize]
        public async Task<IActionResult> DeleteRoom(int roomId)
        {
            Room room = db.Rooms.FirstOrDefault(r => r.RoomId == roomId);

            if (room == null)
            {
                return NotFound(new { errorText = "Номер не найден." });
            }

            db.Rooms.Remove(room);
            await db.SaveChangesAsync();

            return Ok(new { okText = "Номер удален" });
        }

        [HttpGet("/api/addroom/")]
        [Authorize]
        public async Task<IActionResult> AddRoom([FromBody] Room room)
        {

            if (room != null)
            {
                db.Rooms.Add(room);

                await db.SaveChangesAsync();

                return Ok(room);
            }

            return BadRequest(new { errorText = "Некорректные исходные данные" });
        }

        [HttpGet("/api/bookroom/")]
        [Authorize]
        public async Task<IActionResult> BookRoom(int? roomId, int? userId)
        {

            if (roomId != null && userId != null)
            {
                var lastOrder = db.Orders.Max(o => o.OrderId);
                var newOrderId = lastOrder + 1;

                var order = new Order()
                {
                    OrderId = newOrderId,
                    RoomId = (int)roomId,
                    UserId = (int)userId
                };

                db.Orders.Add(order);

                await db.SaveChangesAsync();

                return Ok("Ok");
            }

            return BadRequest(new { errorText = "Некорректные исходные данные" });
        }

        [HttpGet("/api/searchrooms")]
        public async Task<IActionResult> SearchRooms(string description)
        {
            IEnumerable<Room> rooms = await db.Rooms.Where(r => r.Description.Contains(description)).ToListAsync();

            if (rooms != null)
            {
                return Ok(rooms);
            }

            return BadRequest(new { errorText = "Нет ни одного номера" });
        }
    }
}
