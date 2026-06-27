using System.Text.RegularExpressions;

namespace url_generator
{
    class FileAnalyzer
    {
        public void Analyze(string inputPath, string outputPath, string urlPrefix)
        {
            //string inputPath = args.Length > 0 ? args[0] : "file.txt";
            //string outputPath = args.Length > 1 ? args[1] : "urls.txt";
            string prefix = "http://localhost:50618/";

            string text = File.ReadAllText(inputPath);

            var matches = Regex.Matches(text, @"Include([A-Za-z0-9_\-]+\.aspx)", RegexOptions.IgnoreCase);

            var urls = matches
                .Select(m => m.Groups[1].Value)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .Select(name => prefix + name)
                .ToList();

            File.WriteAllLines(outputPath, urls);

            Console.WriteLine($"Generadas {urls.Count} URLs");
            Console.WriteLine($"Archivo creado: {outputPath}");
        }
    }
}
