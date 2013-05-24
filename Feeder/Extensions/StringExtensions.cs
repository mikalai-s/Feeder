using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace Feeder.Extensions
{
    internal static class StringExtensions
    {
        public static string FromHtmlToPreview(this string html)
        {
            int length = 100;
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            return doc.DocumentNode.InnerText.Replace('\n', ' ').Substring(0, length) + "...";
        }
    }
}
