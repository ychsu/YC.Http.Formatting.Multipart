using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web;

namespace YC.Http.Formatting.Multipart.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task TestMethod1()
        {
            var multipartContent = new MultipartFormDataContent();
            multipartContent.Add(IdContent());
            multipartContent.Add(NameContent());
            multipartContent.Add(FileContent());

            var result = await multipartContent.ReadAsStringAsync();
            var formatter = new MultipartMediaTypeFormatter();
            var stream = await multipartContent.ReadAsStreamAsync();
            var obj = await formatter.ReadFromStreamAsync(typeof(TestModel),
                stream,
                multipartContent,
                null) as TestModel;
            Assert.IsNotNull(obj);
            Assert.AreEqual(123, obj.Id);
            Assert.AreEqual("YC", obj.Name);
            Assert.AreEqual("text/plain", obj.File?.ContentType);
        }

        private HttpContent IdContent()
        {
            var content = new StringContent("123");
            content.Headers.Add("Content-Disposition", "form-data; name=\"Id\"");
            
            return content;
        }

        private HttpContent NameContent()
        {
            var content = new StringContent("YC");
            content.Headers.Add("Content-Disposition", "form-data; name=\"Name\"");
            return content;
        }

        private HttpContent FileContent()
        {
            var stream = new System.IO.MemoryStream();
            var writer = new System.IO.StreamWriter(stream);
            writer.WriteLine("李好");
            writer.Flush();
            stream.Seek(0, System.IO.SeekOrigin.Begin);
            var content = new StreamContent(stream);
            content.Headers.Add("Content-Disposition", "form-data; name=\"File\"; filename=\"123.txt\"");
            content.Headers.Add("Content-Type", "text/plain");
            return content;
        }
    }

    public class TestModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public HttpPostedFileWrapper File { get; set; }
    }
}
