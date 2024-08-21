﻿using System.ComponentModel.DataAnnotations;

namespace Shernell_Project.Models
{
    public class Facility
    {
        [Key]
        public int BookingID { get; set; }
        public string? FacilityDescription { get; set; }
        public string? BookingDateFrom { get; set; }
        public string? BookingDateTo { get; set; } 
        public string? BookedBy { get; set; }
        public string? BookingStatus { get; set; }
    }
}
