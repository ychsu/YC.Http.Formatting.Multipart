using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

namespace System.Net.Http.Formatting
{
    public static class HttpPostedFileHelper
    {
        static Type HttpPostedFileType = typeof(HttpPostedFile);
        static Type HttpRawUploadedContentType = Type.GetType($"{HttpPostedFileType.Namespace}.HttpRawUploadedContent, {HttpPostedFileType.Assembly.FullName}");
        static Type HttpInputStreamType = Type.GetType($"{HttpPostedFileType.Namespace}.HttpInputStream, {HttpPostedFileType.Assembly.FullName}");

        /// <summary>
        /// 轉換byte array為 <see cref="System.Web.HttpRawUploadedContent"/> 
        /// </summary>
        /// <param name="arr">byte array</param>
        /// <returns></returns>
        private static object ConvertToHttpRawUploadedContent(byte[] arr)
        {
            var constructor = HttpRawUploadedContentType.GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance)
                .FirstOrDefault();
            var obj = constructor.Invoke(new object[] { 1024, arr.Length });
            var addBytesMethod = HttpRawUploadedContentType.GetMethod("AddBytes", BindingFlags.NonPublic | BindingFlags.Instance);
            var doneAddingBytesMethod = HttpRawUploadedContentType.GetMethod("DoneAddingBytes", BindingFlags.NonPublic | BindingFlags.Instance);
            addBytesMethod.Invoke(obj, new object[] { arr, 0, arr.Length });
            doneAddingBytesMethod.Invoke(obj, new object[] { });
            return obj;
        }

        /// <summary>
        /// 轉換 byte array 為 <see cref="System.Web.HttpInputStream"/>
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        private static object ConvertToHttpInputStream(byte[] arr)
        {
            var constructor = HttpInputStreamType.GetConstructor(
                BindingFlags.NonPublic | BindingFlags.Instance,
                null,
                new Type[] { HttpRawUploadedContentType, typeof(int), typeof(int) },
                null);
            var httpRawUploadedContent = ConvertToHttpRawUploadedContent(arr);

            return constructor.Invoke(new object[] { httpRawUploadedContent, 0, arr.Length });
        }

        /// <summary>
        /// 建立 <see cref="System.Web.HttpPostedFile"/>
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="contentType"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static HttpPostedFile FromStream(
            string fileName,
            string contentType,
            MemoryStream stream)
        {
            var arr = stream.ToArray();

            var constructor = HttpPostedFileType.GetConstructor(
                BindingFlags.NonPublic | BindingFlags.Instance,
                null,
                new Type[] { typeof(string), typeof(string), HttpInputStreamType },
                null);
            var inputStream = ConvertToHttpInputStream(arr);
            return constructor.Invoke(new object[] {
                fileName,
                contentType,
                inputStream
            }) as HttpPostedFile;
        }
    }
}
