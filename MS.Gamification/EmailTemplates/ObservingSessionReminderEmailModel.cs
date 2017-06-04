using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MS.Gamification.EmailTemplates
{
    public class ObservingSessionReminderEmailModel : EmailModelBase
    {
    public string ObservingSessionTitle { get; private set; }

    public DateTime ObservingSessionStartsAt { get; private set; }

    public string ObservingSessionDescription { get; private set; }

    public string ObservingSessionVenue { get; private set; }
    }
}