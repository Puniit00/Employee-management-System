using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Model
{
    public class PerformanceReview
    {
        /// <summary>
        /// Gets or sets the unique identifier for the review
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the reviewer
        /// </summary>
        [Required]
        [StringLength(100)]
        public string ReviewerName { get; set; }

        /// <summary>
        /// Gets or sets the date of the review
        /// </summary>
        [Required]
        public DateTime ReviewDate { get; set; }

        /// <summary>
        /// Gets or sets the rating for the review
        /// </summary>
        [Required]
        public int Rating { get; set; }

        /// <summary>
        /// Gets or sets the comments for the review
        /// </summary>
        [StringLength(2000)]
        public string Comments { get; set; }
    }
}