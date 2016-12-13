// This file is part of the MS.Gamification project
// 
// File: with_standard_mission.cs  Created: 2016-11-01@19:37
// Last modified: 2016-12-13@00:00

using System.Web.Mvc;
using Machine.Specifications;
using MS.Gamification.Tests.TestHelpers;

namespace MS.Gamification.Tests.Controllers
    {
    /*
     * Initializes an MVC controller and fake database, with a standard observing mission.
     * 
     * Category 10 - Phase
     * Category 20 - Planet
     * Category 30 - Open Cluster
     * Category 40 - Galaxy
     * 
     * Mission "Unit Test Mission" Id=1 
     * Level=1
     *  Track 1 - Lunar Observer
     *      100 See the New Moon    1 point     Category 10
     *      101 See the Full Moon   1 point     Category 10
     *  Track 2 - Planetologist
     *      200 See Jupiter         1 point     Category 20 
     *      201 See Saturn          1 point     Category 20 
     *  Track 3 - Deep Space Explorer
     *      300 See M45 Pleiades    1 point     Category 30 
     *      400 See Andromeda       1 point     Category 40
     */

    class with_standard_mission<TController> : with_mvc_controller<TController> where TController : Controller
        {
        Establish context = () => ContextBuilder.WithData(TestData.CreateStandardMissionData);
        }
    }