using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace api_auditor.Classes
{
    class RouteGenerator
    {
        public ControllerScraper controllerScraper { get; set; }

        public RouteGenerator()
        {
            controllerScraper = new ControllerScraper();
        }

        public bool FromTxtFile(string inputPath, string outputPath, string urlPrefix)
        {
            try
            {
                //string inputPath = args.Length > 0 ? args[0] : "file.txt";
                //string outputPath = args.Length > 1 ? args[1] : "urls.txt";
                //string prefix = "http://localhost:50618/";

                string text = File.ReadAllText(inputPath);

                var matches = Regex.Matches(text,
                    @"Include([A-Za-z0-9_\-]+\.aspx)",
                    RegexOptions.IgnoreCase)
                    .Cast<Match>();

                var urls = matches
                    .Select(m => m.Groups[1].Value)
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .Select(name => urlPrefix + name)
                    .ToList();

                File.WriteAllLines(outputPath, urls);

                Console.WriteLine($"Generadas {urls.Count} URLs");
                Console.WriteLine($"Archivo creado: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"***EXCEPTION: \n{ex.Message}");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Genera un archivo .csv con los endpoints generados a partir de los controladores 
        /// encontrados dentro de un directorio/carpeta. 
        /// </summary>
        /// <param name="dirPath">Ruta del directorio/carpeta que contiene los archivos(controladores).</param>
        /// <param name="baseUrl">URL base de la  API.</param>
        /// <param name="fileName">Nombre con el que se desea guardar el archivo generado.</param>
        public bool FromDirectory(string dirPath, string baseUrl, string fileName)
        {
            try
            {
                controllerScraper.GenerateRoutes(dirPath, baseUrl, fileName);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"***EXCEPTION: \n{ex.Message}");
                return false;
            }
            return true;
        }
    }
}
