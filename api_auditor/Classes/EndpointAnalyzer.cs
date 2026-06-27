//using Microsoft.CodeAnalysis;
//using Microsoft.CodeAnalysis.CSharp.Syntax;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace api_auditor.Classes
//{
//    public static class EndpointAnalyzer
//    {
//        static bool HasAuthorize(ClassDeclarationSyntax cls, MethodDeclarationSyntax method)
//            => HasAttribute(cls, "Authorize") || HasAttribute(method, "Authorize");

//        static bool HasAttribute(SyntaxNode node, string name)
//            => node.DescendantNodesAndSelf().OfType<AttributeSyntax>()
//                .Any(a => a.Name.ToString().Contains(name, StringComparison.OrdinalIgnoreCase));

//        static bool HasBody(MethodDeclarationSyntax method, string httpMethod)
//            => httpMethod is "POST" or "PUT" or "PATCH" ||
//               method.ParameterList.Parameters.Any(p =>
//                   p.AttributeLists.SelectMany(a => a.Attributes)
//                       .Any(a => a.Name.ToString().Contains("FromBody", StringComparison.OrdinalIgnoreCase)));

//        static object BuildPayload(MethodDeclarationSyntax method)
//        {
//            var dict = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);

//            foreach (var p in method.ParameterList.Parameters)
//            {
//                var name = p.Identifier.Text;
//                dict[name] = SampleValue(name, p.Type?.ToString() ?? "");
//            }

//            return dict.Count > 0 ? dict : new { id = 1, nombre = "test", descripcion = "prueba" };
//        }

//        static object SampleValue(string name, string typeName)
//        {
//            var n = name.ToLowerInvariant();
//            var t = typeName.ToLowerInvariant();

//            if (t.Contains("int") || t.Contains("long") || n.Contains("id") || n.Contains("codigo")) return 1;
//            if (t.Contains("decimal") || t.Contains("double") || t.Contains("float")) return 1.5;
//            if (t.Contains("bool")) return true;
//            if (t.Contains("datetime") || n.Contains("fecha")) return DateTime.UtcNow;
//            return "test";
//        }

//        static bool HasRouteParams(string template) => template.Contains('{') && template.Contains('}');

//        static string StripControllerSuffix(string name)
//            => name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase)
//                ? name[..^"Controller".Length]
//                : name;

//        static string ResolveTokens(string template, string controllerName, string actionName)
//            => template.Replace("[controller]", controllerName, StringComparison.OrdinalIgnoreCase)
//                       .Replace("[action]", actionName, StringComparison.OrdinalIgnoreCase);

//        static string CombineRoutes(string controllerRoute, string methodRoute)
//            => string.IsNullOrWhiteSpace(methodRoute)
//                ? controllerRoute
//                : controllerRoute.TrimEnd('/') + "/" + methodRoute.TrimStart('/');

//        static string? GetControllerRoute(ClassDeclarationSyntax cls)
//        {
//            var attr = cls.AttributeLists.SelectMany(a => a.Attributes)
//                .FirstOrDefault(a => a.Name.ToString().Contains("Route", StringComparison.OrdinalIgnoreCase));
//            return attr?.ArgumentList?.Arguments.FirstOrDefault()?.ToString().Trim('"');
//        }

//        static AttributeSyntax? GetHttpAttribute(MethodDeclarationSyntax method)
//            => method.AttributeLists.SelectMany(a => a.Attributes)
//                .FirstOrDefault(a => a.Name.ToString().StartsWith("Http", StringComparison.OrdinalIgnoreCase));

//        static string GetMethodRoute(AttributeSyntax attr)
//            => attr.ArgumentList?.Arguments.FirstOrDefault()?.ToString().Trim('"') ?? "";

//        static string GetHttpMethod(AttributeSyntax attr)
//        {
//            var name = attr.Name.ToString();
//            return name.StartsWith("Http", StringComparison.OrdinalIgnoreCase)
//                ? name["Http".Length..].ToUpperInvariant()
//                : "GET";
//        }

//        static string ToCsv(List<ResultRow> rows)
//        {
//            var sb = new StringBuilder();
//            sb.AppendLine("Controller,Action,HttpMethod,RouteTemplate,Url,RequiresAuthHint,HasRouteParam,HasBody,TestPayload,StatusCode,Accessible,TokenRequired,Notes,Ms");
//            foreach (var r in rows)
//            {
//                sb.AppendLine(string.Join(",",
//                    Csv(r.Controller),
//                    Csv(r.Action),
//                    Csv(r.HttpMethod),
//                    Csv(r.RouteTemplate),
//                    Csv(r.Url),
//                    Csv(r.RequiresAuthHint),
//                    Csv(r.HasRouteParam),
//                    Csv(r.HasBody),
//                    Csv(r.TestPayload),
//                    Csv(r.StatusCode?.ToString() ?? ""),
//                    Csv(r.Accessible),
//                    Csv(r.TokenRequired),
//                    Csv(r.Notes),
//                    Csv(r.Ms.ToString())));
//            }
//            return sb.ToString();
//        }

//        static string Csv(string? value)
//        {
//            value ??= "";
//            return "\"" + value.Replace("\"", "\"\"") + "\"";
//        }
//    }
//}
