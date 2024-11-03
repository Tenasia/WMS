using Nancy;

namespace StudentDashboard
{
    public class StaticFileModule : NancyModule
    {
        public StaticFileModule()
        {
            // Serve the index.html file at the root URL
            Get("/", _ => Response.AsFile("public/index.html", "text/html"));

            // Serve other static files
            Get("/public/{file*}", parameters =>
            {
                var filePath = "public/" + (string)parameters.file;
                return Response.AsFile(filePath);
            });
        }
    }
}
