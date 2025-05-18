using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using CulturAPPI.Models;

namespace CulturAPPI.Controllers
{
    public class UsersController : ApiController
    {
        private CulturAppEntities db = new CulturAppEntities();

        // GET: api/Users
        public IQueryable<Users> GetUsers()
        {
            return db.Users;
        }

        [HttpGet]
        [Route("api/Users/{email}/{password}")]
        // Devuelve un usuario si el email y contraseña coinciden y no es de tipo "super".
        public async Task<IHttpActionResult> GetUser(string email, string password)
        {
            IHttpActionResult result;
            db.Configuration.LazyLoadingEnabled = false;

            Users _users = await db.Users
                .Where(u => u.email == email && u.password == password && u.type != "super" && u.active == true)
                .FirstOrDefaultAsync();

            if (_users == null)
            {
                result = NotFound();
            }
            else
            {
                result = Ok(_users);
            }

            return result;
        }

        [HttpPut]
        [Route("api/Users/Contraseña/{email}/{password}")]
        // Actualiza la contraseña de un usuario según su email.
        public async Task<IHttpActionResult> PutUsersPassword(string email, string password)
        {
            IHttpActionResult result;
            db.Configuration.LazyLoadingEnabled = false;

            if (!ModelState.IsValid)
            {
                result = BadRequest(ModelState);
            }

            var user = await db.Users.FirstOrDefaultAsync(u => u.email == email);

            if (user == null)
            {
                result = NotFound();
            }
            else
            {
                user.password = password;
                await db.SaveChangesAsync();

                result = StatusCode(HttpStatusCode.NoContent);
            }

            return result;
        }

        // PUT: api/Users/5
        [HttpPut]
        [ResponseType(typeof(void))]
        // Actualiza los datos (nombre, apellido, email) de un usuario por su ID.
        public async Task<IHttpActionResult> PutUser(int id, Users user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.id)
            {
                return BadRequest();
            }
            var existingUser = await db.Users
                .FirstOrDefaultAsync(b => b.id == id);

            existingUser.name = user.name;
            existingUser.surname = user.surname;
            existingUser.email = user.email;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsersExists(id))
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

        // POST: api/Users
        [ResponseType(typeof(Users))]
        // Crea un nuevo usuario en la base de datos.
        public async Task<IHttpActionResult> PostUsers(Users users)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Users.Add(users);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = users.id }, users);
        }

        // Verifica si existe un usuario con el ID dado.
        private bool UsersExists(int id)
        {
            return db.Users.Count(e => e.id == id) > 0;
        }
    }
}