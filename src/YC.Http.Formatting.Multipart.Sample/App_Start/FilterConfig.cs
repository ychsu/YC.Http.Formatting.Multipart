﻿using System.Web;
using System.Web.Mvc;

namespace YC.Http.Formatting.Multipart.Sample
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
