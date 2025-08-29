namespace EmployeeManagementsystem.Middlewares
{
    public class CustomMiddlewareExtensions
    {
        public IApplicationBuilder _app;
        public CustomMiddlewareExtensions(IApplicationBuilder app)
        {
            _app = app;
        }
        public void ConfigureCustomExceptionMiddleware()
        {
            _app.UseMiddleware<CustomExceptionhandlingMiddleWare>();
        }
    }
}
