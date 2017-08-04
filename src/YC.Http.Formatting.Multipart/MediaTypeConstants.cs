using System.Net.Http.Headers;

namespace System.Net.Http.Formatting
{
    public class MediaTypeConstants
    {
        private static readonly MediaTypeHeaderValue _defaultMultipartMediaType = new MediaTypeHeaderValue("multipart/form-data");

        /// <summary>
        /// Gets a <see cref="MediaTypeHeaderValue"/> instance representing <c>multipart/form-data</c>.
        /// </summary>
        /// <value>
        /// A new <see cref="MediaTypeHeaderValue"/> instance representing <c>multipart/form-data</c>.
        /// </value>
        public static MediaTypeHeaderValue MultipartMediaType
        {
            get { return _defaultMultipartMediaType; }
        }
    }
}
