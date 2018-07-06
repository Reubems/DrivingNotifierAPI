using DrivingNotifierAPI.Data;
using DrivingNotifierAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DrivingNotifierAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Requests")]
    public class RequestsController : Controller
    {
        private readonly DataAccessRequest dataRequest;
        private readonly DataAccessUser dataUser;

        public RequestsController(DataAccessRequest dataForRequest, DataAccessUser dataForUser)
        {
            dataRequest = dataForRequest;
            dataUser = dataForUser;
        }

        // GET: api/Requests
        [HttpGet]
        public Task<IEnumerable<Request>> GetRequests()
        {
            return dataRequest.GetRequests();
        }

        // GET: api/Requests/53452345/23452343
        [HttpGet("{requestorPhone}/{replierPhone}", Name = "getRequestByPhones")]
        public IActionResult GetRequest([FromRoute] string requestorPhone, [FromRoute] string replierPhone)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var request = dataRequest.GetRequest(requestorPhone, replierPhone);

            if (request == null)
            {
                return NotFound();
            }

            return Ok(request);
        }

        // GET: api/PendingRequests/23453294
        [HttpGet("PendingRequests/{phone}")]
        public Task<IEnumerable<Request>> GetPendingRequests([FromRoute] string phone)
        {
            return dataRequest.GetPendingRequests(phone);
        }


        // POST: api/Requests
        [HttpPost]
        public async Task<IActionResult> PostRequest([FromBody] Request request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //We only add a new request if that does not already exist
            var requestFetched = dataRequest.GetRequest(request.RequestorPhone, request.ReplierPhone);
            if (requestFetched == null)
            {
                await dataRequest.CreateRequest(request);
                return Ok(); //TODO change for other // https://github.com/Microsoft/aspnet-api-versioning/issues/18
            }

            return BadRequest(ModelState);
        }

        [HttpPut]
        public async Task<IActionResult> PutRequest([FromBody] Request request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var requestFetched = dataRequest.GetRequest(request.RequestorPhone, request.ReplierPhone);
            if (requestFetched != null)
            {
                await dataRequest.UpdateRequestState(request.RequestorPhone, request.ReplierPhone, request.State);
            }

            // Add the ObjectId to the Contacts lists of the user, in case request is accepted.
            if (request.State.Equals(RequestState.ACCEPTED))
            {
                //Update the list of the replier in the database.
                await dataUser.AddUserContactList(request.RequestorPhone, request.ReplierPhone);
            }

            return Ok(request);
        }

        // DELETE: api/Requests/54235434/23452346
        [HttpDelete("{requestorPhone}/{replierPhone}")]
        public async Task<IActionResult> DeleteRequest([FromRoute] string requestorPhone, [FromRoute] string replierPhone)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var request = dataRequest.GetRequest(requestorPhone, replierPhone);
            if (request == null)
            {
                return NotFound();
            }

            await dataRequest.DeleteRequest(requestorPhone, replierPhone);

            return Ok();
        }
    }
}
