namespace WebApplication1.Constants
{
    public class EmployeeConstants
    {
        public struct StoreProcedures
        {
            public const string GetAllEmployees = "GetAllEmployees";
            public const string GetEmployeeById = "GetEmployeeById";
            public const string AddEmployee = "AddEmployee";
            public const string UpdateEmployee = "UpdateEmployee";
            public const string DeleteEmployee = "DeleteEmployee";
            //Goals related stored procedures
            public const string AddEmployeeGoals = "AddEmployeeGoals";
            public const string GetEmployeeGoals = "GetEmployeeGoals";
            public const string UpdateEmployeeGoals = "UpdateEmployeeGoals";
            // performance review related stored procedures
            public const string AddPerformanceReview = "AddPerformanceReview";
            public const string GetPerformanceReviews = "GetPerformanceReviews";
        }
    }
}
