// This file is part of the MS.Gamification project
// 
// File: RequiresAuthorization.cs  Created: 2016-07-17@07:43
// Last modified: 2016-07-17@07:48

using System.Web.Mvc;

namespace MS.Gamification.Controllers
    {
    [Authorize]
    public class RequiresAuthorization : Controller {}
    }