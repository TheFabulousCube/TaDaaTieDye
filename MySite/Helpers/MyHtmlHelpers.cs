using System.Web.Mvc;

namespace MySite.Helpers
{
    public static class MyHtmlHelpers
    {
        public static MvcHtmlString DisplayAllErrors(this HtmlHelper html, string key)
        {
            string message = "<ul class=\"validation-summary-errors\">";
            if (html.ViewData.ModelState[key] != null)
            {
                foreach (var error in html.ViewData.ModelState[key].Errors)
                {
                    TagBuilder li = new TagBuilder("li");
                    li.SetInnerText(error.ErrorMessage);
                    message += li;


                }
            }

            message += "</ul>";
            return new MvcHtmlString(message);
        }


    }
}