using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using CulturAPPI.Models;

namespace CulturAPPI.Controllers
{
    public class TypeEventController : ApiController
    {
        private CulturAppEntities db = new CulturAppEntities();

        // GET: api/TypeEvent/5
        [ResponseType(typeof(Type_event))]
        // Obtiene la lista de tipos de eventos y la devuelve como DTO.
        public async Task<IHttpActionResult> GetType_event()
        {
            IHttpActionResult result;
            db.Configuration.LazyLoadingEnabled = false;

            var _typeEvent = await db.Type_event
                .Select(te => new TypeEventDTO
                {
                    Id = te.id,
                    Name = te.name
                }).ToListAsync();

            if (_typeEvent == null)
            {
                result = NotFound();
            }
            else
            {
                result = Ok(_typeEvent);
            }

            return result;
        }
    }
}