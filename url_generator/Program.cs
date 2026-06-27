namespace url_generator
{
    class URLGenerator
    {
        static void Main(string[] args)
        {
            string inputPath = args.Length > 0 ? args[0] : "file.txt";
            string outputPath = args.Length > 1 ? args[1] : "urls.txt";
            string urlPrefix = "http://localhost:50618/";
            string apiBaseUrl = "";

            FileAnalyzer fa = new FileAnalyzer();
            DirectoryAnalyzer da = new DirectoryAnalyzer();
        }
    }

}