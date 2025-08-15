using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Model
{
    public class Employee
    {
        /// <summary>
        /// Gets or Sets the id
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the employee
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the employee's email address
        /// </summary>
        [Required]
        public string Email { get; set; }

        [Required]
        /// <summary>
        /// Gets or sets the department where the employee works
        /// </summary>
        public string Department { get; set; }

        [Required]
        /// <summary>
        /// Gets or sets the date when the employee joined the organization
        /// </summary>
        public DateTime DateOfJoining { get; set; }
    }
}
