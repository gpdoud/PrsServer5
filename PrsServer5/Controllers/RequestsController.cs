using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using PrsServer5.Models;

namespace PrsServer5.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class RequestsController : ControllerBase {
        private readonly AppDbContext _context;

        public RequestsController(AppDbContext context) {
            _context = context;
        }

        // GET: api/Requests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Request>>> GetRequests() {
            return await _context.Requests
                                    .Include(x => x.User)
                                    .ToListAsync();
        }

        // GET: api/Requests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Request>> GetRequest(int id) {
            var request = await _context.Requests
                                    .Include(x => x.User)
                                    .Include(x => x.Requestlines)
                                    .ThenInclude(x => x.Product)
                                    .SingleOrDefaultAsync(x => x.Id == id);

            if(request == null) {
                return NotFound();
            }

            return request;
        }

        // GET: api/Requests/reviewed
        [HttpGet("reviewed/{userid}")]
        public async Task<ActionResult<IEnumerable<Request>>> GetRequestsInReview(int userid) {
            return await _context.Requests
                                    .Include(r => r.User)
                                    .Where(r => r.Status == Models.Request.StatusReview
                                             && r.UserId != userid)
                                    .ToListAsync();
        }

        // POST: api/requests/review
        [HttpPost("review")]
        public async Task<IActionResult> SetRequestToReviewPost(Request request) {
            return await SetRequestToReview(request);
        }

        // PUT: api/Requests/Review
        [HttpPut("review")]
        public async Task<IActionResult> SetRequestToReview(Request request) {
            request.Status = request.Total <= 50m
                ? PrsServer5.Models.Request.StatusApproved
                : PrsServer5.Models.Request.StatusReview;
            return await PutRequest(request.Id, request);
        }

        // POST: api/requests/review
        [HttpPost("approve")]
        public async Task<IActionResult> SetRequestToApprovePost(Request request) {
            return await SetRequestToApprove(request);
        }

        // PUT: api/Requests/Approve
        [HttpPut("approve")]
        public async Task<IActionResult> SetRequestToApprove(Request request) {
            request.Status = PrsServer5.Models.Request.StatusApproved;
            return await PutRequest(request.Id, request);
        }

        // POST: api/requests/review
        [HttpPost("reject")]
        public async Task<IActionResult> SetRequestToRejectPost(Request request) {
            return await SetRequestToReject(request);
        }


        // PUT: api/Requests/Reject
        [HttpPut("reject")]
        public async Task<IActionResult> SetRequestToReject(Request request) {
            request.Status = PrsServer5.Models.Request.StatusRejected;
            return await PutRequest(request.Id, request);
        }

        // POST: api/requests/update/5
        [HttpPost("update/{id}")]
        public async Task<IActionResult> UpdateReqeust(int id, Request request) {
            return await PutRequest(id, request);
        }

        // PUT: api/Requests/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRequest(int id, Request request) {
            if(id != request.Id) {
                return BadRequest();
            }

            _context.Entry(request).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
            } catch(DbUpdateConcurrencyException) {
                if(!RequestExists(id)) {
                    return NotFound();
                } else {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Requests
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Request>> PostRequest(Request request) {
            _context.Requests.Add(request);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRequest", new { id = request.Id }, request);
        }

        // POST: api/requests/delete/5
        [HttpPost("delete/{id}")]
        public async Task<IActionResult> RemoveRequest(int id) {
            return await DeleteRequest(id);
        }

        // DELETE: api/Requests/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRequest(int id) {
            var request = await _context.Requests.FindAsync(id);
            if(request == null) {
                return NotFound();
            }

            _context.Requests.Remove(request);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RequestExists(int id) {
            return _context.Requests.Any(e => e.Id == id);
        }
    }
}
