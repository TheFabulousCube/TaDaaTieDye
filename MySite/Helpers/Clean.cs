using System.Text;
using System.Text.RegularExpressions;

namespace MySite.Helpers
{
    public class Clean
    {
        public static string HtmlTags(string dirtyString)
        {
            string cleanString = "";
            if (dirtyString != null)
            {
                cleanString = Regex.Replace(dirtyString, "<.*?>", string.Empty, RegexOptions.Singleline);
            }
            return cleanString;
        }

        // http://stackoverflow.com/questions/1120198/most-efficient-way-to-remove-special-characters-from-string
        // I LOVE regex, but this was just too slick not to use!

        public static string FileName(string dirtyString)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < dirtyString.Length; i++)
            {
                if ((dirtyString[i] >= '0' && dirtyString[i] <= '9')
                    || (dirtyString[i] >= 'A' && dirtyString[i] <= 'Z')
                    || (dirtyString[i] >= 'a' && dirtyString[i] <= 'z')
                        || (dirtyString[i] == '.' || dirtyString[i] == '_'))
                {
                    sb.Append(dirtyString[i]);
                }
            }

            return sb.ToString();
        }

    }
}

