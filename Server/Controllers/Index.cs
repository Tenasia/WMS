using Nancy;

namespace StudentDashboard
{
    public class Index : NancyModule
    {
        public Index()
        {
            // Define a GET route at "/index/hello" which returns a simple message
            Get("/index/hello", _ => "Hello from the backend!");

            // Example POST route to process data
            Post("/index/data", args =>
            {
                var requestBody = this.Request.Body;
                string data = new StreamReader(requestBody).ReadToEnd();
                return $"Received data: {data}";
            });

            // Define more routes as needed
        }
    }
}
