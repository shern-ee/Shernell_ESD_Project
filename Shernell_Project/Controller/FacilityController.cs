using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shernell_Project.Data;
using Shernell_Project.Models;
using System.Globalization;
using System.Text;

namespace Shernell_Project.Controller
{
    [Authorize]
    [Route("api/[controller]/{action}")]
    [ApiController]
    public class FacilityController : ControllerBase
    {
        private readonly BookingDbContext _context;

        public FacilityController(BookingDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.User)]
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

        [Authorize(Roles = UserRoles.Admin)]
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


        [HttpGet("search")]
        public IActionResult Search(string description)
        {
            var facilities = _context.Facilities
                .Where(f => f.FacilityDescription.Contains(description))
                .ToList();

            if (facilities == null || !facilities.Any())
            {
                return NotFound("No facilities match the search criteria.");
            }

            return Ok(facilities);
        }

        [HttpGet("sort")]
        public IActionResult Sort(string orderBy)
        {
            var facilities = _context.Facilities.AsQueryable();

            switch (orderBy.ToLower())
            {
                case "alphabetical":
                    facilities = facilities.OrderBy(f => f.FacilityDescription);
                    break;
                case "datefrom":
                    facilities = facilities.OrderBy(f => f.BookingDateFrom);
                    break;
                case "status":
                    facilities = facilities.OrderBy(f => f.BookingStatus);
                    break;
                default:
                    return BadRequest("Invalid sort parameter.");
            }

            return Ok(facilities.ToList());
        }

        [HttpGet("export")]
        public IActionResult ExportToCSV()
        {
            var facilities = _context.Facilities.ToList();

            var csv = new StringBuilder();
            csv.AppendLine("Facility Description,Booking Date From,Booking Date To,Booked By,Booking Status");

            foreach (var facility in facilities)
            {
                csv.AppendLine($"{facility.FacilityDescription},{facility.BookingDateFrom},{facility.BookingDateTo},{facility.BookedBy},{facility.BookingStatus}");
            }

            var csvBytes = Encoding.UTF8.GetBytes(csv.ToString());
            return File(csvBytes, "text/csv", "facility_bookings.csv");
        }

    }
}
