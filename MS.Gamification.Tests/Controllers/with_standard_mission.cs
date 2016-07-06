// This file is part of the MS.Gamification project
// 
// File: with_standard_mission.cs  Created: 2016-07-02@17:27
// Last modified: 2016-07-06@23:39

using System.Web.Mvc;
using Machine.Specifications;
using MS.Gamification.Models;
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
        Establish context = () => ContextBuilder
            .WithEntity(new Category {Id = 10, Name = "Phase"})
            .WithEntity(new Category {Id = 20, Name = "Planet"})
            .WithEntity(new Category {Id = 30, Name = "Open Cluster"})
            .WithEntity(new Category {Id = 40, Name = "Galaxy"})
            .WithMissionLevel().WithId(1).Level(1)
            .WithTrack(1).WithId(1)
            .WithChallenge("See the New Moon").WithId(100).InCategory(10).BuildChallenge()
            .WithChallenge("See the Full Moon").WithId(101).InCategory(10).BuildChallenge().BuildTrack()
            .WithTrack(2).WithId(2)
            .WithChallenge("See Jupiter").WithId(200).InCategory(20).BuildChallenge()
            .WithChallenge("See Saturn").WithId(201).InCategory(20).BuildChallenge().BuildTrack()
            .WithTrack(3).WithId(3)
            .WithChallenge("See the Pleiades").WithId(300).InCategory(30).BuildChallenge()
            .WithChallenge("See Andromeda").WithId(400).InCategory(40).BuildChallenge().BuildTrack()
            .BuildMission();
        }
    }