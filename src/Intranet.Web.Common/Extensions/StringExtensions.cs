using System;
using System.Collections.Generic;
using System.Text;

namespace Intranet.Web.Common.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Truncate a string, taking in consideration not the break any
        /// words. Optional trail.
        /// </summary>
        /// <param name="content"></param>
        /// <param name="length"></param>
        /// <param name="trail"></param>
        /// <returns></returns>
        public static string TruncateAtWord(this string content, int length, string trail = "...")
        {
            if (content == null || content.Length <= length)
            {
                return content;
            }

            int nextSpace = content.LastIndexOf(" ", length);

            return string.Format("{0}{1}", content.Substring(0, (nextSpace > 0) ? nextSpace : length).Trim(), trail);
        }

        /// <summary>
        /// Truncate a string. Optional trail.
        /// </summary>
        /// <param name="content"></param>
        /// <param name="length"></param>
        /// <param name="trail"></param>
        /// <returns></returns>
        public static string Truncate(this string content, int length, string trail = "...")
        {
            return content.Length <= length ? content : String.Format("{0}{1}", content.Substring(0, length).Trim(), trail);
        }
    }
}