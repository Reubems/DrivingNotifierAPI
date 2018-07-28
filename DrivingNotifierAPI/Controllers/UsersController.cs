using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
        [HttpGet("{email}", Name = "getUserByEmail")]
        public IActionResult GetUser([FromRoute] string email)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = dataUser.GetUserByEmail(email);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }
       
        // POST: api/Users

        [Route("Register")]
        [HttpPost(Name = "Register")]
        public async Task<IActionResult> PostUser([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); 
            }

            //We only add a new user if its email does not already exist
            var usersWithThatEmail = dataUser.GetUserByEmail(user.Email);
            if (usersWithThatEmail == null)
            {
                await dataUser.InsertUser(user);
                return Ok();
            }

            return BadRequest(ModelState);
        }


        // DELETE: api/Users/54235434
        [HttpDelete("{email}")]
        public async Task<IActionResult> DeleteUser([FromRoute] string email)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = dataUser.GetUserByEmail(email);
            if (user == null)
            {
                return NotFound();
            }

            await dataUser.DeleteUser(user);
          
            return Ok();
        }

        // DELETE: api/Users/Contacts/54235434/39448346
        [HttpDelete("Contacts/{emailRequestor}/{emailToDelete}")]
        public async Task<IActionResult> RemoveContact([FromRoute] string emailRequestor, [FromRoute] string emailToDelete)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = dataUser.GetUserByEmail(emailRequestor);
            var deleted = dataUser.GetUserByEmail(emailToDelete);
            if (user == null || deleted == null)
            {
                return NotFound();
            }

            await dataUser.RemoveUserContactList(emailRequestor, emailToDelete);

            return Ok(deleted);
        }

        // GET: api/Users/Driving
        [HttpGet("Driving")]
        public List<String> GetUsersDriving([FromRoute] string email)
        {
            return dataUser.GetContactsDrivingList(email);
        }

        // GET: api/Users/Contacts
        [HttpGet("Contacts")]
        public List<String> GetContacts([FromRoute] string email)
        {
            return dataUser.GetContactsList(email);
        }

        // POST: api/Users/login
        [Route("Login")]
        [HttpPost(Name = "Login")]
        public async Task<IActionResult> LoginUser([FromBody] User user)
        {
            //TODO Check password and return user.
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await CheckPlayerIdAsync(user);

            return Ok(); //CreatedAtAction("ActionMethodName", dto);
        }

        private async Task CheckPlayerIdAsync(User user)
        {
            string phone = user.Email;
            string playerId = user.PlayerID;

            var userFetched =  dataUser.GetUserByEmail(user.Email);
            if (userFetched!= null && userFetched.PlayerID != user.PlayerID)
            {
                //Update the playerId
                await dataUser.UpdateUserPlayerID(user);
            }
        }
        
        // POST: api/Users/PushNotification
        [Route("PushNotification")]
        [HttpPost(Name = "PushNotification")]
        public IActionResult PushNotification([FromBody] string drivingEmail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = dataUser.GetUserByEmail(drivingEmail);

            if (user == null)
            {
                return NotFound();
            }

            dataUser.PushNotification(drivingEmail);
            
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

        // PUT: api/Users/Driving
        [HttpPut("Driving")]
        public async Task<IActionResult> UpdateUserDrivingState([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await dataUser.UpdateUserDrivingState(user);

            return Ok();
        }
    }
}