using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using CulturAPPI.Models;

namespace CulturAPPI.Controllers
{
    public class RoomsController : ApiController
    {
        private CulturAppEntities db = new CulturAppEntities();

        // GET: api/Rooms/
        [ResponseType(typeof(Rooms))]
        public async Task<IHttpActionResult> GetRooms()
        {
            IHttpActionResult result;
            db.Configuration.LazyLoadingEnabled = false;

            var _rooms = await db.Rooms
                .Select(te => new RoomDTO
                {
                    Id = te.id,
                    Name = te.name
                }).ToListAsync();

            if (_rooms == null)
            {
                result = NotFound();
            }
            else
            {
                result = Ok(_rooms);
            }

            return result;
        }
    }
}