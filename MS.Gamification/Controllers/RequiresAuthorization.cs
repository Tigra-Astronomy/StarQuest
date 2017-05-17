// This file is part of the MS.Gamification project
// 
// File: RequiresAuthorization.cs  Created: 2016-11-01@19:37
// Last modified: 2017-05-16@20:39

using System.Web.Mvc;

namespace MS.Gamification.Controllers
    {
    [Authorize]
    public class RequiresAuthorization : Controller { }
    }