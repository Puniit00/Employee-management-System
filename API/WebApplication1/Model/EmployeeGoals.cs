namespace WebApplication1.Model
{
    public class EmployeeGoals
    {
        public int EmployeeId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime TargetDate { get; set; }
        public bool IsCompleted { get; set; }
        public int Id { get; set; }
    }
}