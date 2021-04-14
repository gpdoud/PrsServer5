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
    public class VendorsController : ControllerBase {
        private readonly AppDbContext _context;

        public VendorsController(AppDbContext context) {
            _context = context;
        }

        // GET: /api/Vendors/Po/vendId
        [HttpGet("poview/{vendId}")]
        public async Task<ActionResult<PurchaseOrderView>> GetPo(int vendId) {
            var poview = new PurchaseOrderView();
            // get the vendor
            poview.Vendor = await _context.Vendors.FindAsync(vendId);
            // summarize the approved request lines for this vendor
            var approvedRequests = from r in _context.Requests
                                   join l in _context.Requestlines
                                   on r.Id equals l.RequestId
                                   join p in _context.Products
                                   on l.ProductId equals p.Id
                                   join v in _context.Vendors
                                   on p.VendorId equals v.Id
                                   where v.Id == vendId && r.Status == "APPROVED"
                                   select new {
                                       Requestlineid = l.Id, RequestlineQuantity = l.Quantity, RequestId = l.RequestId,
                                       ProductId = l.Product.Id, ProductDescription = l.Product.Description,
                                       ProductPrice = l.Product.Price, ProductPartNbr = l.Product.PartNbr
                                   };
            var polines = new Dictionary<int, Requestline>();
            foreach(var x in approvedRequests.ToList()) {
                if(!polines.ContainsKey(x.ProductId)) {
                    polines[x.ProductId] = new Requestline {
                        Id = x.Requestlineid, RequestId = x.RequestId,
                        ProductId = x.ProductId, Quantity = x.RequestlineQuantity,
                        Product = new Product {
                            Price = x.ProductPrice, Description = x.ProductDescription,
                            PartNbr = x.ProductPartNbr
                        }
                    };
                    continue;
                }
                // key already exists
                polines[x.ProductId].Quantity += x.RequestlineQuantity;
            }
            foreach(var poline in polines.OrderBy(x => x.Value.Product.Description)) {
                poview.Polines.Add(poline.Value);
                poview.Total += poline.Value.Product.Price * poline.Value.Quantity;
            }
            return poview;
        }

        // GET: api/Vendors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vendor>>> GetVendors() {
            return await _context.Vendors.ToListAsync();
        }

        // GET: api/Vendors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Vendor>> GetVendor(int id) {
            var vendor = await _context.Vendors.FindAsync(id);

            if(vendor == null) {
                return NotFound();
            }

            return vendor;
        }

        // POST: api/vendors/update/5
        [HttpPost("update/{id}")]
        public async Task<IActionResult> UpdateVendor(int id, Vendor vendor) {
            return await PutVendor(id, vendor);
        }

        // PUT: api/Vendors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVendor(int id, Vendor vendor) {
            if(id != vendor.Id) {
                return BadRequest();
            }

            _context.Entry(vendor).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
            } catch(DbUpdateConcurrencyException) {
                if(!VendorExists(id)) {
                    return NotFound();
                } else {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Vendors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Vendor>> PostVendor(Vendor vendor) {
            _context.Vendors.Add(vendor);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVendor", new { id = vendor.Id }, vendor);
        }

        // POST: api/vendors/delete/5
        [HttpPost("delete/{id}")]
        public async Task<IActionResult> RemoveVendor(int id) {
            return await DeleteVendor(id);
        }

        // DELETE: api/Vendors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVendor(int id) {
            var vendor = await _context.Vendors.FindAsync(id);
            if(vendor == null) {
                return NotFound();
            }

            _context.Vendors.Remove(vendor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VendorExists(int id) {
            return _context.Vendors.Any(e => e.Id == id);
        }
    }
}
