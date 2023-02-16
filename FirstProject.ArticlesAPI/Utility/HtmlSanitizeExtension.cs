using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace FirstProject.ArticlesAPI.Utility
{    
    public static class HtmlSanitizeExtension
    {        
        /// <summary>
        /// Метод, который удаляет тэг img из входящего html
        /// </summary>
        public static string FilterHtmlToWhitelist(string text)
        {            
            Regex regex = new Regex(@"\<img[^\>]+\>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            return regex.Replace(text, "");
        }
    }
}
