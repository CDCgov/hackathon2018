using System;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Example.WebUI.Models
{
    /// <summary>
    /// Class representing a customer
    /// </summary>
    public sealed class Customer
    {

    /// <summary>
    /// Gets/sets the customer's numeric ID
    /// </summary>
    [Required(ErrorMessage = "The customer ID field is required")]
        [StringLength(38, ErrorMessage = "The customer ID field may not exceed 38 characters")]
        [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "The customer ID field contain only letters and numbers")]
        public string Id { get; set; }

        /// <summary>
        /// Gets/sets the customer's first name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets/sets the customer's last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets/sets the customer's age in years
        /// </summary>
        [Required(ErrorMessage = "The customer age field is required")]
        [Range(0, 200, ErrorMessage = "The customer age must be between 0 and 200, inclusive")]
        public int Age { get; set; }

        /// <summary>
        /// Gets/sets the customer's name
        /// </summary>
        public string StreetAddress { get; set; }

        /// <summary>
        /// Gets/sets the customer's date of birth
        /// </summary>
        public DateTime DateOfBirth { get; set; }
    }
}