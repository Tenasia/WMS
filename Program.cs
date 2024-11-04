using System;
using System.IO;
using System.Net;
using System.Text;
using MySql.Data.MySqlClient; // Import the MySQL library

class Program
{
    static void Main(string[] args)
    {
        // Set the server prefix (e.g., http://localhost:8080/)
        string prefix = "http://localhost:8080/";
        HttpListener listener = new HttpListener();
        listener.Prefixes.Add(prefix);
        listener.Start();
        Console.WriteLine($"Server started at {prefix}");

        // MySQL database connection parameters
        string connectionString = "Server=localhost;Database=wms01;User ID=root;Password=Akosiwilliam47;Port=3306;";

        // Establishing a connection to the database
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            Console.WriteLine("Connected to the database.");

            while (true)
            {
                // Wait for an incoming request
                HttpListenerContext context = listener.GetContext();
                HttpListenerRequest request = context.Request;
                HttpListenerResponse response = context.Response;

                // Determine the file path based on the request URL
                string filePath = request.Url.AbsolutePath.TrimStart('/');

                // Serve Index.html by default if no specific file is requested
                if (string.IsNullOrEmpty(filePath))
                {
                    filePath = "Views/index.html";
                }
                else if (filePath == "Shared/header.html")
                {
                    filePath = "Shared/header.html";
                }
                else
                {
                    // Handle static files (e.g., CSS, JS)
                    if (filePath.StartsWith("CSS/")) // For CSS files
                    {
                        filePath = Path.Combine("CSS", filePath.Substring(4)); // Removes "CSS/" from the path
                    }
                    else if (filePath.StartsWith("JS/")) // For JavaScript files
                    {
                        filePath = Path.Combine("JS", filePath.Substring(3)); // Removes "JS/" from the path
                    }
                    else
                    {
                        filePath = Path.Combine("Views", filePath); // Default to Views folder
                    }
                }

                // Read the file content
                string content;
                if (File.Exists(filePath))
                {
                    content = File.ReadAllText(filePath);
                    response.StatusCode = (int)HttpStatusCode.OK;

                    // Set the content type based on the file extension
                    string extension = Path.GetExtension(filePath);
                    if (extension == ".css")
                    {
                        response.ContentType = "text/css";
                    }
                    else if (extension == ".html")
                    {
                        response.ContentType = "text/html";
                    }
                    else if (extension == ".js") // For JavaScript files
                    {
                        response.ContentType = "application/javascript";
                    }
                }
                else
                {
                    content = "<h1>404 - Not Found</h1>";
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                }

                // Send the response
                byte[] buffer = Encoding.UTF8.GetBytes(content);
                response.ContentLength64 = buffer.Length;
                response.OutputStream.Write(buffer, 0, buffer.Length);
                response.OutputStream.Close();
            }
        }
    }
}
