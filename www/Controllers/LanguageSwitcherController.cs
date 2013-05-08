using System.Web.Mvc;
using System.Web.WebPages;
using System.Globalization;

namespace Royale50th_Facebook.Controllers
{
    public class LanguageSwitcherController : Controller
    {
        public RedirectResult SwitchLanguage(string language)
        {
            Session["Culture"] = new CultureInfo(language);
            return Request.UrlReferrer != null ? Redirect(Request.UrlReferrer.AbsolutePath.ToString(CultureInfo.InvariantCulture)) : null;
        }

    }
}
