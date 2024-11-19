using System;
using System.IO;
using System.Net;
using System.Text;
using MySql.Data.MySqlClient; // Import the MySQL library
using System.Collections.Generic; // For List<T>
using System.Text.Json; // For JSON serialization

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

        while (true)
        {
            // Wait for an incoming request
            HttpListenerContext context = listener.GetContext();
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;

            string filePath = request.Url.AbsolutePath.TrimStart('/');

            // Check if the request is for an API endpoint
            if (filePath == "api/users")
            {
                // Fetch data from the database
                List<Dictionary<string, object>> users = FetchUsersFromDatabase(connectionString);
                // Convert the result to JSON
                string jsonResponse = JsonSerializer.Serialize(users);

                // Return the JSON response
                byte[] buffer = Encoding.UTF8.GetBytes(jsonResponse);
                response.ContentType = "application/json";
                response.ContentLength64 = buffer.Length;
                response.StatusCode = (int)HttpStatusCode.OK;
                response.OutputStream.Write(buffer, 0, buffer.Length);
                response.OutputStream.Close();
            }
            else
            {
                // Handle static file serving as in the original code
                HandleStaticFiles(request, response, filePath);
            }
        }
    }

    // Fetch data from the database
    static List<Dictionary<string, object>> FetchUsersFromDatabase(string connectionString)
    {
        List<Dictionary<string, object>> users = new List<Dictionary<string, object>>();

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            string query = "SELECT id, username, password FROM wms_users";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    // Create a dictionary for each row
                    Dictionary<string, object> row = new Dictionary<string, object>();

                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        row[reader.GetName(i)] = reader.GetValue(i);
                    }

                    users.Add(row);
                }
            }
        }

        return users;
    }

    // Handle static file serving
    static void HandleStaticFiles(HttpListenerRequest request, HttpListenerResponse response, string filePath)
    {
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
            if (filePath.StartsWith("CSS/"))
            {
                filePath = Path.Combine("CSS", filePath.Substring(4));
            }
            else if (filePath.StartsWith("JS/"))
            {
                filePath = Path.Combine("JS", filePath.Substring(3));
            }
            else
            {
                filePath = Path.Combine("Views", filePath);
            }
        }

        string content;
        if (File.Exists(filePath))
        {
            content = File.ReadAllText(filePath);
            response.StatusCode = (int)HttpStatusCode.OK;

            string extension = Path.GetExtension(filePath);
            if (extension == ".css")
            {
                response.ContentType = "text/css";
            }
            else if (extension == ".html")
            {
                response.ContentType = "text/html";
            }
            else if (extension == ".js")
            {
                response.ContentType = "application/javascript";
            }
        }
        else
        {
            content = "<h1>404 - Not Found</h1>";
            response.StatusCode = (int)HttpStatusCode.NotFound;
        }

        byte[] buffer = Encoding.UTF8.GetBytes(content);
        response.ContentLength64 = buffer.Length;
        response.OutputStream.Write(buffer, 0, buffer.Length);
        response.OutputStream.Close();
    }
}
