using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace System.Net.Http.Formatting
{
    internal class MultipartStreamConverter
    {
        private IFormatterLogger _logger;
        public MultipartStreamConverter(IFormatterLogger logger)
        {
            this._logger = logger;
        }

        public async Task<object> ToObjectAsync(MultipartStreamProvider provider, Type type)
        {
            var contents = provider.Contents;
            var textContents = contents.Where(p => string.IsNullOrWhiteSpace(p.Headers.ContentDisposition.FileName));
            var fileContents = contents.Except(textContents);
            var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);

            object obj = await ParseHttpContentToObjectAsync(type, textContents);

            await SetFilePropertiesAsync(fileContents, properties, obj);

            return obj;
        }

        private async Task SetFilePropertiesAsync(IEnumerable<HttpContent> fileContents, PropertyInfo[] properties, object obj)
        {
            foreach (var fileContent in fileContents)
            {
                var name = fileContent.Headers.ContentDisposition.Name.Trim('\"').Split('.');
                await SetFileAsync(fileContent, name, properties, obj);
            }
        }

        private async Task SetFileAsync(HttpContent fileContent, string[] name, PropertyInfo[] properties, object obj)
        {
            var prop = properties.FirstOrDefault(p =>
                p.Name == name[0]);
            if (name.Length > 1)
            {
                var val = prop.GetValue(obj) ?? Activator.CreateInstance(prop.PropertyType);
                var _properties = prop.PropertyType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                await SetFileAsync(fileContent, name.Skip(1).ToArray(), _properties, val);
                prop.SetValue(obj, val);
                return;
            }
            if (prop.PropertyType.IsSubclassOf(typeof(HttpPostedFileBase)))
            {
                prop?.SetValue(obj, await ParseToFileWrap(fileContent));
            }
        }

        private async Task<object> ParseHttpContentToObjectAsync(Type type, IEnumerable<HttpContent> textContents)
        {
            var keyValuePairs = await GetKeyValuePairs(textContents);

            var obj = await ParseToObject(type, keyValuePairs);

            return obj;
        }

        private static async Task<HttpPostedFileWrapper> ParseToFileWrap(HttpContent fileContent)
        {
            HttpPostedFileWrapper wrap;
            var ms = new MemoryStream();
            var stream = await fileContent.ReadAsStreamAsync();
            stream.CopyTo(ms);

            var httpPostedFile =
                HttpPostedFileHelper.FromStream(
                    fileContent.Headers.ContentDisposition.FileName.Trim('\"'),
                    fileContent.Headers.ContentType?.MediaType,
                    ms);
            wrap = new HttpPostedFileWrapper(httpPostedFile);
            return wrap;
        }

        private async Task<IEnumerable<KeyValuePair<string, string>>> GetKeyValuePairs(IEnumerable<HttpContent> contents)
        {
            var lst = new List<KeyValuePair<string, string>>();

            foreach (var content in contents)
            {
                var pair = new KeyValuePair<string, string>(
                    content.Headers.ContentDisposition.Name.Trim('\"'),
                    await content.ReadAsStringAsync());
                lst.Add(pair);
            }
            return lst.OrderBy(p => p.Key);
        }
        private async Task<object> ParseToObject(Type type, IEnumerable<KeyValuePair<string, string>> pairs)
        {
            var content = new FormUrlEncodedContent(pairs);
            var formatter = new FormUrlEncodedMediaTypeFormatter();
            var stream = await content.ReadAsStreamAsync();
            var jtoken = await formatter.ReadFromStreamAsync(typeof(JToken), stream, content, this._logger) as JToken;

            return jtoken.ToObject(type);
        }
    }
}
