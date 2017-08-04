using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace System.Net.Http.Formatting
{
    public class MultipartMediaTypeFormatter : MediaTypeFormatter
    {
        public static MultipartMediaTypeFormatter Create() => new MultipartMediaTypeFormatter();

        public MultipartMediaTypeFormatter()
        {
            this.SupportedMediaTypes.Add(MediaTypeConstants.MultipartMediaType);
        }

        public override bool CanReadType(Type type)
        {
            return true;
        }

        public override bool CanWriteType(Type type)
        {
            return false;
        }
        
        public override async Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
        {
            if (content.IsMimeMultipartContent() == false)
            {
                return null;
            }
            var provider = await content.ReadAsMultipartAsync();

            var converter = new MultipartStreamConverter(formatterLogger);

            return await converter.ToObjectAsync(provider, type);
        }
    }
}
