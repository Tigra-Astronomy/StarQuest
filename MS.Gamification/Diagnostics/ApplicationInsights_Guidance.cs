// This file is part of the MS.Gamification project
// 
// File: ApplicationInsights_Guidance.cs  Created: 2016-07-29@12:12
// Last modified: 2016-07-29@16:21

using System;
using System.Diagnostics;
using Microsoft.ApplicationInsights;

namespace MS.Gamification.Diagnostics
    {
    public class ApplicationInsights_Guidance
        {
        private void MonitorDependency(string dependencyName, string callName, Action dependency)
            {
            var success = false;
            // Note: A single instance of telemetry client is sufficient to track multiple telemetry items.
            var ai = new TelemetryClient();
            var startTime = DateTime.UtcNow;
            var timer = Stopwatch.StartNew();
            try
                {
                dependency();
                success = true;
                }
            catch (Exception)
                {
                success = false;
                throw;
                }
            finally
                {
                timer.Stop();
                ai.TrackDependency(dependencyName, callName, startTime, timer.Elapsed, success);
                }
            }
        }
    }