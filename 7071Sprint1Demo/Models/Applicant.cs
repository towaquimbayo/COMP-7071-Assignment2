using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace _7071Sprint1Demo.Models
{
    public enum ApplicantType
    {
        Employee,
        Renter
    }

    public class Applicant
    {
        public Guid Id { get; set; }

        // Common fields
        [Required]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, Phone]
        public string Phone { get; set; }

        [Display(Name = "Emergency Contact")]
        public string EmergencyContact { get; set; }

        [Required]
        public string ApplicationStatus { get; set; }

        [Required]
        public DateTime DateApplied { get; set; }

        [Required]
        public ApplicantType ApplicantType { get; set; }

        // Employee-specific fields
        public string? PositionAppliedFor { get; set; }  
        public string? Resume { get; set; }              

        // Renter-specific fields
        public string? PreferredUnitType { get; set; }    
        public string? IncomeVerification { get; set; }  
    }

}
