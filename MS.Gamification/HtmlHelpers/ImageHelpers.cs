using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MS.Gamification.HtmlHelpers
    {
    public static class ImageHelpers
        {
        public static MvcHtmlString ValidationImage(this HtmlHelper htmlHelper, string filename, object htmlAttributes = null)
            {
            UrlHelper urlHelper = ((Controller)htmlHelper.ViewContext.Controller).Url;
            TagBuilder imgTag = new TagBuilder("img");
            var routeToImage = urlHelper.Action("GetImage", "ValidationImage", new {filename});
            imgTag.MergeAttribute("src", routeToImage);
            imgTag.MergeAttribute("alt", "Validation Image");
            imgTag.MergeAttributes(new System.Web.Routing.RouteValueDictionary(htmlAttributes));
            var html = MvcHtmlString.Create(imgTag.ToString(TagRenderMode.StartTag));
            return html;
            }
        }
    }