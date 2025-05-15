using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using CulturAPPI.Models;

namespace CulturAPPI.Controllers
{
    public class BookingsController : ApiController
    {
        private CulturAppEntities db = new CulturAppEntities();

        // GET: api/Bookings
        public IQueryable<Booking> GetBooking()
        {
            return db.Booking;
        }

        // GET: api/Bookings/Users/5
        [HttpGet]
        [Route("api/Bookings/Users/{id_user}")]
        [ResponseType(typeof(Booking))]
        public async Task<IHttpActionResult> GetBookingUser(int id_user)
        {
            IHttpActionResult result;
            db.Configuration.LazyLoadingEnabled = false;

            var _booking = await db.Booking
                .Where(b => b.user_id == id_user && b.active == true)
                .Select(b => new BookingDTO
                {
                    User_id = b.user_id,
                    Event_id = b.event_id,
                    Quantity = b.quantity,
                    Active = b.active,
                    Events = new EventDTO
                    {
                        Id = b.Events.id,
                        Title = b.Events.title,
                        Start_datetime = b.Events.start_datetime.ToString(),
                        End_datetime = b.Events.end_datetime.ToString(),
                        Capacity = b.Events.capacity,
                        Description = b.Events.description,
                        Price = b.Events.price,
                        Active = b.Events.active,
                        Room_id = b.Events.room_id,
                        Type_id = b.Events.type_id,
                        Rooms = new RoomDTO
                        {
                            Id = b.Events.Rooms.id,
                            Name = b.Events.Rooms.name
                        },
                        Type_event = new TypeEventDTO
                        {
                            Id = b.Events.Type_event.id,
                            Name = b.Events.Type_event.name
                        }
                    }
                }).ToListAsync();

            if (_booking == null)
            {
                result = NotFound();
            }
            else
            {
                result = Ok(_booking);
            }

            return result;
        }

        // GET: api/Bookings/Events/5
        [HttpGet]
        [Route("api/Bookings/Events/{id_event}")]
        [ResponseType(typeof(Booking))]
        public async Task<IHttpActionResult> GetBookingEvent(int id_event)
        {
            IHttpActionResult result;
            db.Configuration.LazyLoadingEnabled = false;

            List<Booking> _booking = await db.Booking
                .Where(b => b.event_id == id_event && b.active == true)
                .ToListAsync();

            if (_booking == null)
            {
                result = NotFound();
            }
            else
            {
                result = Ok(_booking);
            }

            return result;
        }

        // PUT: api/Bookings/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutBooking(Booking booking)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user_id = booking.user_id;
            var event_id = booking.event_id;

            var existingBooking = await db.Booking
                .FirstOrDefaultAsync(b => b.user_id == user_id && b.event_id == event_id);

            if (existingBooking == null)
            {
                return NotFound();
            }

            existingBooking.active = booking.active;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!db.Booking.Any(b => b.user_id == user_id && b.event_id == event_id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Bookings
        [ResponseType(typeof(Booking))]
        public async Task<IHttpActionResult> PostBooking(Booking booking)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Booking.Add(booking);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (BookingExists(booking.user_id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = booking.user_id }, booking);
        }

        private bool BookingExists(int id)
        {
            return db.Booking.Count(e => e.user_id == id) > 0;
        }
    }
}