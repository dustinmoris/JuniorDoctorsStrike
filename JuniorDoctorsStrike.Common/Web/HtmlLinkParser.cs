using System.Text.RegularExpressions;

namespace JuniorDoctorsStrike.Common.Web
{
    public class HtmlLinkParser : IHtmlLinkParser
    {
        public string ParseHtmlLinks(string input)
        {
            const string pattern = @"((www\.|(http|https|ftp|news|file)+\:\/\/)[&#95;.a-z0-9-]+\.[a-z0-9\/&#95;:@=.+?,##%&~-]*[^.|\'|\# |!|\(|?|,| |>|<|;|\)])";

            var regex = new Regex(pattern, RegexOptions.IgnoreCase);

            return regex.Replace(input, "<a href=\"$1\">$1</a>").Replace("href=\"www", "href=\"http://www");
        }
    }
}