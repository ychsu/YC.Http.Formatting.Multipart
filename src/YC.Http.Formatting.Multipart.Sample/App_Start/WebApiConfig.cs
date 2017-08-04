using System;
using System.Collections.Generic;
using System.Net.Http.Formatting;
using System.Linq;
using System.Web.Http;

namespace YC.Http.Formatting.Multipart.Sample
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API 設定和服務

            // Web API 路由
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Formatters.Add(MultipartMediaTypeFormatter.Create());
        }
    }
}
