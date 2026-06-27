using System.Net;

namespace white_scraper
{
    class URLAnalyzer
    {
        static async Task Main(string[] args)
        {
            //var urls = new List<string>
            //{
            //    "http://localhost:50618/Acerca.aspx",
            //    "http://localhost:50618/ActivarRifa.aspx",
            //    "http://localhost:50618/Acuerdos.aspx"
            //};

            var urls = File.ReadAllLines("urls.txt")
                   .Where(line => !string.IsNullOrWhiteSpace(line))
                   .ToList();

            using var handler = new HttpClientHandler
            {
                AllowAutoRedirect = false,
                UseCookies = false
            };

            using var client = new HttpClient(handler)
            {
                Timeout = TimeSpan.FromSeconds(0.01)
            };

            var results = new List<string>();
            results.Add("Url,StatusCode,AccessibleWithoutToken,Notes");

            var results200 = new List<string>();
            //results200.Add("Url,StatusCode,AccessibleWithoutToken,Notes");

            foreach (var url in urls)
            {
                try
                {
                    using var request = new HttpRequestMessage(HttpMethod.Get, url);
                    request.Headers.TryAddWithoutValidation("User-Agent", "Mozilla/5.0");

                    using var response = await client.SendAsync(request);

                    bool accessible = response.StatusCode == HttpStatusCode.OK ||
                                      response.StatusCode == HttpStatusCode.Accepted ||
                                      response.StatusCode == HttpStatusCode.NoContent;

                    string notes =
                        response.StatusCode == HttpStatusCode.Unauthorized ? "401 Unauthorized" :
                        response.StatusCode == HttpStatusCode.Forbidden ? "403 Forbidden" :
                        response.StatusCode == HttpStatusCode.Redirect ||
                        response.StatusCode == HttpStatusCode.MovedPermanently ||
                        response.StatusCode == HttpStatusCode.Found ? "Redirect (possible login)" :
                        "";

                    var line = $"{Escape(url)},{(int)response.StatusCode},{accessible},{Escape(notes)}";
                    results.Add(line);

                    if ((int)response.StatusCode == 200) results200.Add(url);

                    //Console.WriteLine($"{url} -> {(int)response.StatusCode} {response.ReasonPhrase}");
                    BeautifyOutput(response, url);
                }
                catch (TaskCanceledException)
                {
                    results.Add($"{Escape(url)},TIMEOUT,false,Timeout");
                    Console.Write($"{url} -> ");
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("TIMEOUT");
                    Console.ResetColor();
                }
                catch (Exception ex)
                {
                    results.Add($"{Escape(url)},ERROR,false,{Escape(ex.Message)}");
                    Console.WriteLine($"{url} -> ERROR: {ex.Message}");
                }
            }

            File.WriteAllLines("url_check_results.csv", results);
            File.WriteAllLines("url_check_results_200.csv", results200);

            Console.WriteLine("CSV generado: url_check_results.csv");
            Console.WriteLine("CSV generado: url_check_results_200.csv");
            Console.Write("\n------------------!!EJECUCIÓN FINALIZADA!!------------------");
            Console.ReadKey();
        }

        static string Escape(string value)
            => "\"" + value.Replace("\"", "\"\"") + "\"";

        static bool BeautifyOutput(HttpResponseMessage response, string url)
        {

            if ((int)response.StatusCode == 200)
            {
                Console.Write($"{url} -> ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"[200] {response.ReasonPhrase}");
            }
            else if ((int)response.StatusCode == 401 || (int)response.StatusCode == 403)
            {
                Console.Write($"{url} -> ");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[{(int)response.StatusCode}] {response.ReasonPhrase}");
            }
            else
            {
                Console.Write($"{url} -> ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"[{(int)response.StatusCode}] {response.ReasonPhrase}");
            }

            Console.ResetColor();
            return true;


        }
    }
}