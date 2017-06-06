using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MS.Gamification.Models;

namespace MS.Gamification.EmailTemplates
{
    public class ObservingSessionReminderEmailModel : EmailModelBase
    {
    public ObservingSession Session { get; set; }
    }
}