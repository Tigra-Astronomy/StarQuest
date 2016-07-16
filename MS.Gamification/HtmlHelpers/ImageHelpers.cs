// This file is part of the MS.Gamification project
// 
// File: ImageHelpers.cs  Created: 2016-07-10@00:07
// Last modified: 2016-07-16@02:27

using System.Web.Mvc;
using System.Web.Routing;

namespace MS.Gamification.HtmlHelpers
    {
    public static class ImageHelpers
        {
        public static MvcHtmlString ValidationImage(this HtmlHelper htmlHelper, string imageName, object htmlAttributes = null)
            {
            UrlHelper urlHelper = ((Controller)htmlHelper.ViewContext.Controller).Url;
            TagBuilder imgTag = new TagBuilder("img");
            var routeToImage = urlHelper.Action("GetImage", "ValidationImage", new {id = imageName});
            imgTag.MergeAttribute("src", routeToImage);
            imgTag.MergeAttribute("alt", "Validation Image");
            imgTag.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            var html = MvcHtmlString.Create(imgTag.ToString(TagRenderMode.StartTag));
            return html;
            }

        public static MvcHtmlString Badge(this HtmlHelper htmlHelper, string imageName, object htmlAttributes = null)
            {
            UrlHelper urlHelper = ((Controller)htmlHelper.ViewContext.Controller).Url;
            TagBuilder imgTag = new TagBuilder("img");
            var routeToImage = urlHelper.Action("GetImage", "BadgeImage", new {id = imageName});
            imgTag.MergeAttribute("src", routeToImage);
            imgTag.MergeAttribute("alt", "Badge Image");
            imgTag.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            var html = MvcHtmlString.Create(imgTag.ToString(TagRenderMode.StartTag));
            return html;
            }
        }
    }
