using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shernell_Project.Data;
using Shernell_Project.Models;

namespace Shernell_Project.Controller
{
    [Route("api/[controller]/{action}")]
    [ApiController]
    public class FacilityController : ControllerBase
    {
        private readonly BookingDbContext _context;

        public FacilityController(BookingDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_context.Facilities);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int? id) 
        {
            var facilities = _context.Facilities.FirstOrDefault(e => e.BookingID == id);
            if (facilities == null)
                return Problem(detail: "Facility with id " + id + " is not found.", statusCode: 404);
            
            return Ok(facilities);
        }

        [HttpPost]
        public IActionResult Post(Facility facility)
        {
            _context.Facilities.Add(facility);
            _context.SaveChanges();

            return CreatedAtAction("GetAll", new { id = facility.BookingID }, facility);
        }

        [HttpPut]
        public IActionResult Put(int? id, Facility facility)
        {
            var entity = _context.Facilities.FirstOrDefault(e => e.BookingID == id);
            if (entity == null)
                return Problem(detail: "Facility with id " + id + " is not found.", statusCode: 404);

            entity.FacilityDescription = facility.FacilityDescription;
            entity.BookingDateFrom = facility.BookingDateFrom;
            entity.BookingDateTo = facility.BookingDateTo;
            entity.BookedBy = facility.BookedBy;
            entity.BookingStatus = facility.BookingStatus;

            _context.SaveChanges();

            return Ok(entity);
        }

        [HttpDelete]
        public IActionResult Delete(int? id, Facility facility)
        {
            var entity = _context.Facilities.FirstOrDefault(e => e.BookingID == id);
            if (entity == null)
                return Problem(detail: "Facility with id " + id + " is not found.", statusCode: 404);

            _context.Facilities.Remove(entity);
            _context.SaveChanges();

            return Ok(entity);
        }
    }
}
