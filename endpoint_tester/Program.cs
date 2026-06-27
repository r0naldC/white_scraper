using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

var controllersPath = args.Length > 0 ? args[0] : throw new ArgumentException("Falta la ruta de Controllers");
var baseUrl = args.Length > 1 ? args[1].TrimEnd('/') : throw new ArgumentException("Falta la baseUrl");
var outputCsv = args.Length > 2 ? args[2] : "resultados_endpoints.csv";

var files = Directory.GetFiles(controllersPath, "*.cs", SearchOption.AllDirectories);
var rows = new List<ResultRow>();
using var client = new HttpClient();

foreach (var file in files)
{
    var code = await File.ReadAllTextAsync(file);
    var tree = CSharpSyntaxTree.ParseText(code);
    var root = tree.GetCompilationUnitRoot();

    foreach (var cls in root.DescendantNodes().OfType<ClassDeclarationSyntax>())
    {
        var controllerName = StripControllerSuffix(cls.Identifier.Text);
        var controllerRoute = GetControllerRoute(cls) ?? $"api/[controller]";

        foreach (var method in cls.Members.OfType<MethodDeclarationSyntax>())
        {
            var httpAttr = GetHttpAttribute(method);
            if (httpAttr is null) continue;

            var httpMethod = GetHttpMethod(httpAttr);
            var methodRoute = GetMethodRoute(httpAttr);
            var routeTemplate = CombineRoutes(controllerRoute, methodRoute);
            routeTemplate = ResolveTokens(routeTemplate, controllerName, method.Identifier.Text);

            var routeParams = GetRouteParams(routeTemplate);
            var hasBody = HasBody(method, httpMethod);
            var requiresAuthHint = HasAuthorize(cls, method);

            var url = BuildUrl(baseUrl, routeTemplate);
            var payload = hasBody ? BuildPayload(cls.Identifier.Text, method.Identifier.Text, method) : null;

            var row = new ResultRow
            {
                Controller = cls.Identifier.Text,
                Action = method.Identifier.Text,
                HttpMethod = httpMethod,
                RouteTemplate = routeTemplate,
                Url = url,
                RequiresAuthHint = requiresAuthHint ? "SI" : "NO",
                HasRouteParam = routeParams.Count > 0 ? "SI" : "NO",
                HasBody = hasBody ? "SI" : "NO",
                TestPayload = payload is null ? "" : JsonSerializer.Serialize(payload),
            };

            var sw = Stopwatch.StartNew();
            try
            {
                HttpResponseMessage resp;
                if (hasBody && payload is not null)
                {
                    using var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
                    using var req = new HttpRequestMessage(new HttpMethod(httpMethod), url) { Content = content };
                    resp = await client.SendAsync(req);
                }
                else
                {
                    using var req = new HttpRequestMessage(new HttpMethod(httpMethod), url);
                    resp = await client.SendAsync(req);
                }

                sw.Stop();
                row.StatusCode = (int)resp.StatusCode;
                row.Accessible = resp.IsSuccessStatusCode ? "SI" : "NO";
                row.TokenRequired = resp.StatusCode is System.Net.HttpStatusCode.Unauthorized or System.Net.HttpStatusCode.Forbidden ? "SI" : "NO";
                row.Notes = resp.ReasonPhrase ?? "";
                row.Ms = sw.ElapsedMilliseconds;
            }
            catch (Exception ex)
            {
                sw.Stop();
                row.StatusCode = null;
                row.Accessible = "NO";
                row.TokenRequired = "DESCONOCIDO";
                row.Notes = ex.GetType().Name + ": " + ex.Message;
                row.Ms = sw.ElapsedMilliseconds;
            }

            rows.Add(row);
        }
    }
}

await File.WriteAllTextAsync(outputCsv, ToCsv(rows), Encoding.UTF8);
Console.WriteLine($"CSV generado: {outputCsv}");
Console.WriteLine($"Total endpoints: {rows.Count}");

static bool HasAuthorize(ClassDeclarationSyntax cls, MethodDeclarationSyntax method)
{
    return HasAttribute(cls, "Authorize") || HasAttribute(method, "Authorize");
}

static bool HasAttribute(SyntaxNode node, string name)
{
    return node.DescendantNodesAndSelf()
        .OfType<AttributeSyntax>()
        .Any(a => a.Name.ToString().Contains(name, StringComparison.OrdinalIgnoreCase));
}

static bool HasBody(MethodDeclarationSyntax method, string httpMethod)
{
    if (httpMethod is "POST" or "PUT" or "PATCH") return true;
    return method.ParameterList.Parameters.Any(p =>
        p.AttributeLists.SelectMany(a => a.Attributes)
            .Any(a => a.Name.ToString().Contains("FromBody", StringComparison.OrdinalIgnoreCase)));
}

static object BuildPayload(string controller, string action, MethodDeclarationSyntax method)
{
    if (method.ParameterList.Parameters.Any())
    {
        var obj = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);
        foreach (var p in method.ParameterList.Parameters)
        {
            if (p.Modifiers.Any(m => m.Text == "ref" || m.Text == "out")) continue;
            var name = p.Identifier.Text;
            obj[name] = SampleValue(name, p.Type?.ToString() ?? "");
        }
        if (obj.Count > 0) return obj;
    }

    return new
    {
        codigo = 1,
        id = 1,
        nombre = "test",
        descripcion = "prueba automatica",
        comentario = "prueba automatica"
    };
}

static object SampleValue(string name, string typeName)
{
    var n = name.ToLowerInvariant();
    var t = typeName.ToLowerInvariant();

    if (t.Contains("int") || t.Contains("long") || n.Contains("id") || n.Contains("codigo")) return 1;
    if (t.Contains("decimal") || t.Contains("double") || t.Contains("float")) return 1.5;
    if (t.Contains("bool") || n.Contains("activo") || n.Contains("estado")) return true;
    if (t.Contains("datetime") || n.Contains("fecha")) return DateTime.UtcNow;
    if (n.Contains("lat")) return "18.73";
    if (n.Contains("lng") || n.Contains("lon")) return "-70.16";
    if (n.Contains("email")) return "test@example.com";
    return "test";
}

static string BuildUrl(string baseUrl, string routeTemplate)
    => $"{baseUrl}/{routeTemplate.TrimStart('/')}";

static string ResolveTokens(string template, string controllerName, string actionName)
{
    return template
        .Replace("[controller]", controllerName, StringComparison.OrdinalIgnoreCase)
        .Replace("[action]", actionName, StringComparison.OrdinalIgnoreCase);
}

static string StripControllerSuffix(string name)
    => name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase)
        ? name[..^"Controller".Length]
        : name;

static string CombineRoutes(string controllerRoute, string methodRoute)
{
    if (string.IsNullOrWhiteSpace(methodRoute)) return controllerRoute;
    return controllerRoute.TrimEnd('/') + "/" + methodRoute.TrimStart('/');
}

static string? GetControllerRoute(ClassDeclarationSyntax cls)
{
    var attr = cls.AttributeLists.SelectMany(a => a.Attributes)
        .FirstOrDefault(a => a.Name.ToString().Contains("Route", StringComparison.OrdinalIgnoreCase));
    return attr?.ArgumentList?.Arguments.FirstOrDefault()?.ToString().Trim('"');
}

static AttributeSyntax? GetHttpAttribute(MethodDeclarationSyntax method)
    => method.AttributeLists.SelectMany(a => a.Attributes)
        .FirstOrDefault(a => a.Name.ToString().StartsWith("Http", StringComparison.OrdinalIgnoreCase));

static string GetMethodRoute(AttributeSyntax attr)
{
    var args = attr.ArgumentList?.Arguments;
    if (args is null || args.Value.Count == 0) return "";
    return args.Value[0].ToString().Trim('"');
}

static string GetHttpMethod(AttributeSyntax attr)
{
    var name = attr.Name.ToString();
    return name.StartsWith("Http", StringComparison.OrdinalIgnoreCase)
        ? name["Http".Length..].ToUpperInvariant()
        : "GET";
}

static List<string> GetRouteParams(string template)
{
    var list = new List<string>();
    var start = 0;
    while ((start = template.IndexOf('{', start)) >= 0)
    {
        var end = template.IndexOf('}', start + 1);
        if (end < 0) break;
        list.Add(template[(start + 1)..end]);
        start = end + 1;
    }
    return list;
}

static string ToCsv(List<ResultRow> rows)
{
    var sb = new StringBuilder();
    sb.AppendLine("Controller,Action,HttpMethod,RouteTemplate,Url,RequiresAuthHint,HasRouteParam,HasBody,TestPayload,StatusCode,Accessible,TokenRequired,Notes,Ms");
    foreach (var r in rows)
    {
        sb.AppendLine(string.Join(",",
            Csv(r.Controller),
            Csv(r.Action),
            Csv(r.HttpMethod),
            Csv(r.RouteTemplate),
            Csv(r.Url),
            Csv(r.RequiresAuthHint),
            Csv(r.HasRouteParam),
            Csv(r.HasBody),
            Csv(r.TestPayload),
            Csv(r.StatusCode?.ToString() ?? ""),
            Csv(r.Accessible),
            Csv(r.TokenRequired),
            Csv(r.Notes),
            Csv(r.Ms.ToString())));
    }
    return sb.ToString();
}

static string Csv(string? value)
{
    value ??= "";
    return "\"" + value.Replace("\"", "\"\"") + "\"";
}

public class ResultRow
{
    public string Controller { get; set; } = "";
    public string Action { get; set; } = "";
    public string HttpMethod { get; set; } = "";
    public string RouteTemplate { get; set; } = "";
    public string Url { get; set; } = "";
    public string RequiresAuthHint { get; set; } = "";
    public string HasRouteParam { get; set; } = "";
    public string HasBody { get; set; } = "";
    public string TestPayload { get; set; } = "";
    public int? StatusCode { get; set; }
    public string Accessible { get; set; } = "";
    public string TokenRequired { get; set; } = "";
    public string Notes { get; set; } = "";
    public long Ms { get; set; }
}