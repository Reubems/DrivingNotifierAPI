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

        public RequestsController(DataAccessRequest data)
        {
            dataRequest = data;
        }

        // GET: api/Requests
        [Route("Requests")]
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

        // GET: api/PendingRequests
        [Route("PendingRequests")]
        [HttpGet("{phone}")]
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
                return CreatedAtAction("getRequestByPhones", new {
                    requestorPhone = request.RequestorPhone,
                    replierPhone = request.ReplierPhone }, 
                    request);
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
            if (requestFetched == null)
            {
                await dataRequest.UpdateRequestState(request.RequestorPhone, request.ReplierPhone, request.State);
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
