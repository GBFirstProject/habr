using HtmlAgilityPack;
using System.Xml;

namespace FirstProject.ArticlesAPI.Utility
{
    public class LeadDataUtilityService
    {
        public static string GetLeadDataDescription(string textHtml)
        {
            if (string.IsNullOrEmpty(textHtml))
            {
                return string.Empty;
            }

            var document = new HtmlDocument();
            document.LoadHtml(textHtml);

            var firstParagraph = document.DocumentNode.Descendants("p").FirstOrDefault();
            if (firstParagraph != null)
            {
                var sanitizedParagraph = SanitizeHtml(firstParagraph.OuterHtml);
                return sanitizedParagraph;
            }
            else
            {
                var sanitizedText = SanitizeHtml(textHtml);
                var firstLineBreakIndex = sanitizedText.IndexOf("\n");
                if (firstLineBreakIndex != -1)
                {
                    return sanitizedText.Substring(0, firstLineBreakIndex);
                }
                else
                {
                    return sanitizedText;
                }
            }
        }

        public static string SanitizeHtml(string html)
        {
            var allowedTags = new[] { "b", "i", "u", "em", "strong", "a", "br" };

            var document = new HtmlDocument();
            document.LoadHtml(html);

            foreach (var node in document.DocumentNode.Descendants().ToArray())
            {
                if (!allowedTags.Contains(node.Name))
                {
                    node.Remove();
                }
            }

            return document.DocumentNode.OuterHtml;
        }
    }
}
