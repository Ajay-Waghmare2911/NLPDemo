using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLPDemo.Model
{
    public class Property
    {
        public int Id { get; set; }

        [Required]
        public string SurveyNumber { get; set; } = string.Empty;

        public string? SubDivision { get; set; }

        public string Village { get; set; } = string.Empty;

        public string Taluk { get; set; } = string.Empty;

        public string District { get; set; } = string.Empty;

        public string OwnerName { get; set; } = string.Empty;

        public string? FatherName { get; set; }

        public decimal AreaAcre { get; set; }

        public int AreaGunta { get; set; }

        public string LandType { get; set; } = string.Empty; // Dry, Wet, etc.

        public decimal TaxAmount { get; set; }

        public string LastPaidYear { get; set; } = string.Empty;

        public string? KhataNumber { get; set; }

        public string? MutationStatus { get; set; }

        public string? Encumbrance { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string? PhotoUrl { get; set; }

        public string? Notes { get; set; }
    }
}