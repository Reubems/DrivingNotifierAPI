using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DrivingNotifierAPI.Models;
using DrivingNotifierAPI.Data;

namespace DrivingNotifierAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Users")]
    public class UsersController : Controller
    {
        private readonly DataAccessUser dataUser;

        public UsersController(DataAccessUser data)
        {
            dataUser = data;
        }

        // GET: api/Users
        [HttpGet]
        public Task<IEnumerable<User>> GetUsers()
        {
            return dataUser.GetUsers();
        }

        // GET: api/Users/53452345
        [HttpGet("{phone}", Name = "getUserByPhoneNumber")]
        public IActionResult GetUser([FromRoute] string phone)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = dataUser.GetUserByPhone(phone);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }
        /*
        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser([FromRoute] long id, [FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        */

        // POST: api/Users

        [Route("Register")]
        [HttpPost(Name = "Register")]
        public async Task<IActionResult> PostUser([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //We only add a new user if its phone does not already exist
            var usersWithThatPhone = dataUser.GetUserByPhone(user.Phone);
            if (usersWithThatPhone == null)
            {
                await dataUser.InsertUser(user);
                return Ok();
            }

            return BadRequest(ModelState);
        }


        // DELETE: api/Users/54235434
        [HttpDelete("{phone}")]
        public async Task<IActionResult> DeleteUser([FromRoute] string phone)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = dataUser.GetUserByPhone(phone);
            if (user == null)
            {
                return NotFound();
            }

            await dataUser.DeleteUser(user);
          
            return Ok();
        }

        //------------------------------------------------------------------------------------------
        
        // POST: api/Users/login
        [Route("Login")]
        [HttpPost(Name = "Login")]
        public async Task<IActionResult> LoginUser([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await CheckPlayerIdAsync(user);

            return Ok(); //CreatedAtAction("ActionMethodName", dto);
        }

        private async Task CheckPlayerIdAsync(User user)
        {
            string phone = user.Phone;
            string playerId = user.PlayerID;

            var userFetched =  dataUser.GetUserByPhone(user.Phone);
            if (userFetched!= null && userFetched.PlayerID != user.PlayerID)
            {
                //Update the playerId
                await dataUser.UpdateUserPlayerID(user);
            }
        }
        
        // POST: api/Users/PushNotification
        [Route("PushNotification")]
        [HttpPost(Name = "PushNotification")]
        public IActionResult PushNotification([FromBody] string drivingPhone)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = dataUser.GetUserByPhone(drivingPhone);

            if (user == null)
            {
                return NotFound();
            }

            dataUser.PushNotification(drivingPhone);
            
            return NoContent();
        }

        // PUT: api/Users/TrackingEnabled
        [HttpPut("TrackingEnabled")]
        public async Task<IActionResult> UpdateUserTrackingEnabledState([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await dataUser.UpdateUserTrackingEnabledState(user);

            return Ok(); 
        }

        // PUT: api/Users/Mute
        [HttpPut("Mute")]
        public async Task<IActionResult> UpdateUserMuteState([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await dataUser.UpdateUserMuteState(user);

            return Ok();
        }
    }
}