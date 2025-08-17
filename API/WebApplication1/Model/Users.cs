namespace EmployeeManagementsystem.Model
{
    public class Users
    {
        /// <summary>
        /// Gets or sets the badge number of the user.
        /// </summary>
        public int BadgeNo { get; set; }

        /// <summary>
        /// Gets or sets the full name of the user.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the JWT token assigned to the user.
        /// </summary>
        public string JwtToken { get; set; }

        /// <summary>
        /// gets or sets the date when the user was created.
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// Gets or sets the password of the user.
        /// </summary>
        public string? Password { get; set; }
    }
}
