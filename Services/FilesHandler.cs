using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StoreApi.Data;
using StoreApi.Data.Dto;
using StoreApi.Data.Enums;
using System.Dynamic;
using System.Globalization;
using System.Reflection;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace StoreApi.Services
{
    public class FilesHandler
    {
        public static async Task GetFileFromUrl(ImportSettingsDto dto, ApplicationDbContext context)
        {
            //Arrange
            //var folderPath = @$"C:\Users\chatz\Downloads\"; ;
            //var fileName = "datafeed.csv";
            //var url = "https://affiliate.linkwi.se/feeds/1.2/CD23070/programs-all/columns-lw_product_id,product_name,description,category,brand_name,tracking_url,image_url,price,full_price,program_name/catinc-0/catex-0/proginc-12149-1188/progex-0/feed.csv";

            //Act
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var path = Path.Combine(AppContext.BaseDirectory, "Downloads");
                    LogService.CreateLog($"Path to use: {path}", LogTypeEnum.Information, LogOriginEnum.StoreApp, Guid.Empty, context);

                    // Ensure the folder exists
                    if (!Directory.Exists(path))
                    {
                        try
                        {
                            Directory.CreateDirectory(path);
                        }catch(Exception ex)
                        {
                            LogService.CreateLog($"Could not create path: {path}, Error: {ex}", LogTypeEnum.Information, LogOriginEnum.StoreApp, Guid.Empty, context);

                        }
                    }

                    // Full path to save the file
                    string filePath = Path.Combine(path, dto.Name);

                    // Download the file
                    byte[] fileBytes = await client.GetByteArrayAsync(dto.GetUrl);

                    // Save the file
                    await File.WriteAllBytesAsync(filePath, fileBytes);

                    Console.WriteLine($"File downloaded and saved as {filePath}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        }
        public static List<dynamic> GetDataFromCSV(string filePath)
        {
            var result = new List<dynamic>();

            using (var reader = new StreamReader(filePath))
            {
                var headerLine = reader.ReadLine();
                if (headerLine == null)
                    throw new Exception("The file is empty.");

                var headers = headerLine.Split(',').ToList();

                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var fields = ParseCsvLine(line);

                    IDictionary<string, object> row = new ExpandoObject();

                    for (int i = 0; i < headers.Count; i++)
                    {
                        string header = headers[i];
                        string value = i < fields.Count ? fields[i] : null;

                        row[header] = value;
                    }

                    result.Add(row);
                }
            }

            return result;
        }

        static List<string> ParseCsvLine(string line)
        {
            var fields = new List<string>();
            var currentField = new StringBuilder();
            bool inQuotes = false;

            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];

                if (c == '"')
                {
                    // Toggle the inQuotes flag if the current character is a double quote
                    inQuotes = !inQuotes;
                }
                else if (c == ',' && !inQuotes)
                {
                    // If we encounter a comma and we're not inside quotes, it's a field separator
                    fields.Add(currentField.ToString().Trim());
                    currentField.Clear();
                }
                else
                {
                    // Otherwise, add the character to the current field
                    currentField.Append(c);
                }
            }

            // Add the last field
            fields.Add(currentField.ToString().Trim());

            return fields.ToList();
        }

    }
}

