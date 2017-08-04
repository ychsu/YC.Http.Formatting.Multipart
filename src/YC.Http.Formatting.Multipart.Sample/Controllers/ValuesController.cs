using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace YC.Http.Formatting.Multipart.Sample.Controllers
{
    public class ValuesController : ApiController
    {
        public IHttpActionResult Post(TestModel model)
        {
            return base.Ok(model);
        }
    }

    public class TestModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [JsonIgnore]
        public HttpPostedFileWrapper File { get; set; }

        public string FileName { get { return this.File?.FileName; } }
    }
}
