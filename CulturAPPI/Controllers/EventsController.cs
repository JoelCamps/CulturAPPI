using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using CulturAPPI.Models;

namespace CulturAPPI.Controllers
{
    public class EventsController : ApiController
    {
        private CulturAppEntities db = new CulturAppEntities();

        // GET: api/Events
        [ResponseType(typeof(Events))]
        public async Task<IHttpActionResult> GetEvents()
        {
            IHttpActionResult result;
            db.Configuration.LazyLoadingEnabled = false;

            var _events = await db.Events
                .Where(e => e.active == true)
                .Select(e => new EventDTO
                {
                    Id = e.id,
                    Title = e.title,
                    Start_datetime = e.start_datetime.ToString(),
                    End_datetime = e.end_datetime.ToString(),
                    Capacity = e.capacity,
                    Description = e.description,
                    Price = e.price,
                    Active = e.active,
                    Room_id = e.room_id,
                    Type_id = e.type_id,
                    Rooms = new RoomDTO
                    {
                        Id = e.Rooms.id,
                        Name = e.Rooms.name,
                    },
                    Type_event = new TypeEventDTO
                    {
                        Id = e.Type_event.id,
                        Name = e.Type_event.name
                    }
                }).ToListAsync();

            if (_events == null)
            {
                result = NotFound();
            }
            else
            {
                result = Ok(_events);
            }

            return result;
        }

        // POST: api/Events
        [ResponseType(typeof(Events))]
        public async Task<IHttpActionResult> PostEvents(EventDTO events)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string formato = "yyyy-MM-dd HH:mm:ss.fff";

            DateTime.TryParseExact(events.Start_datetime, formato, CultureInfo.InvariantCulture,
                                    DateTimeStyles.None, out DateTime Start_datetime);

            DateTime.TryParseExact(events.End_datetime, formato, CultureInfo.InvariantCulture,
                           DateTimeStyles.None, out DateTime End_datetime);

            Events newEvents = new Events
            {
                title = events.Title,
                start_datetime = Start_datetime,
                end_datetime = End_datetime,
                capacity = events.Capacity,
                description = events.Description,
                price = events.Price,
                active = events.Active,
                room_id = events.Room_id,
                type_id = events.Type_id
            };


            db.Events.Add(newEvents);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = events.Id }, events);
        }
    }
}