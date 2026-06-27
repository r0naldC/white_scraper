//using Microsoft.CodeAnalysis.CSharp;
//using Microsoft.CodeAnalysis.CSharp.Syntax;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.IO;
//using System.Net.Http;
//using System.Text;
//using System.Threading.Tasks;

//namespace api_auditor.Classes
//{
//    internal static class EndpointAnalyzerHelpers
//    {
//        public static async Task<List<ResultRow>> RunAsync(string controllersPath, string baseUrl, string outputCsv)
//        {
//            var files = Directory.GetFiles(controllersPath, "*.cs", SearchOption.AllDirectories);
//            var rows = new List<ResultRow>();

//            var client = new HttpClient();

//            foreach (var file in files)
//            {
//                var code = await File.ReadAllTextAsync(file);
//                var tree = CSharpSyntaxTree.ParseText(code);
//                var root = tree.GetCompilationUnitRoot();

//                foreach (var cls in root.DescendantNodes().OfType<ClassDeclarationSyntax>())
//                {
//                    var controllerName = StripControllerSuffix(cls.Identifier.Text);
//                    var controllerRoute = GetControllerRoute(cls) ?? "api/[controller]";

//                    foreach (var method in cls.Members.OfType<MethodDeclarationSyntax>())
//                    {
//                        var httpAttr = GetHttpAttribute(method);
//                        if (httpAttr is null) continue;

//                        var httpMethod = GetHttpMethod(httpAttr);
//                        var methodRoute = GetMethodRoute(httpAttr);
//                        var routeTemplate = ResolveTokens(CombineRoutes(controllerRoute, methodRoute), controllerName, method.Identifier.Text);
//                        var url = $"{baseUrl.TrimEnd('/')}/{routeTemplate.TrimStart('/')}";
//                        var hasBody = HasBody(method, httpMethod);
//                        var requiresAuthHint = HasAuthorize(cls, method);
//                        var payload = hasBody ? BuildPayload(method) : null;

//                        var row = new ResultRow
//                        {
//                            Controller = cls.Identifier.Text,
//                            Action = method.Identifier.Text,
//                            HttpMethod = httpMethod,
//                            RouteTemplate = routeTemplate,
//                            Url = url,
//                            RequiresAuthHint = requiresAuthHint ? "SI" : "NO",
//                            HasRouteParam = HasRouteParams(routeTemplate) ? "SI" : "NO",
//                            HasBody = hasBody ? "SI" : "NO",
//                            TestPayload = payload is null ? "" : JsonSerializer.Serialize(payload)
//                        };

//                        var sw = Stopwatch.StartNew();
//                        try
//                        {
//                            HttpResponseMessage resp;
//                            if (hasBody && payload is not null)
//                            {
//                                var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
//                                var req = new HttpRequestMessage(new HttpMethod(httpMethod), url) { Content = content };
//                                resp = await client.SendAsync(req);
//                            }
//                            else
//                            {
//                                var req = new HttpRequestMessage(new HttpMethod(httpMethod), url);
//                                resp = await client.SendAsync(req);
//                            }

//                            sw.Stop();
//                            row.StatusCode = (int)resp.StatusCode;
//                            row.Accessible = resp.IsSuccessStatusCode ? "SI" : "NO";
//                            row.TokenRequired = resp.StatusCode is System.Net.HttpStatusCode.Unauthorized or System.Net.HttpStatusCode.Forbidden ? "SI" : "NO";
//                            row.Notes = resp.ReasonPhrase ?? "";
//                            row.Ms = sw.ElapsedMilliseconds;
//                        }
//                        catch (Exception ex)
//                        {
//                            sw.Stop();
//                            row.Accessible = "NO";
//                            row.TokenRequired = "DESCONOCIDO";
//                            row.Notes = ex.GetType().Name + ": " + ex.Message;
//                            row.Ms = sw.ElapsedMilliseconds;
//                        }

//                        rows.Add(row);
//                    }
//                }
//            }

//            await File.WriteAllTextAsync(outputCsv, ToCsv(rows), Encoding.UTF8);
//            return rows;
//        }
//    }
//}