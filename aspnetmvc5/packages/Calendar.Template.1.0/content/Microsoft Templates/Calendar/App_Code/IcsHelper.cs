using System.IO;

public static class IcsHelper {
    public static string CalendarHeaders {
        get {
            var file = new StringWriter();
            file.WriteLine("BEGIN:VCALENDAR");
            file.WriteLine("PRODID:-//ASP//SharedCalendar//EN");
            file.WriteLine("VERSION:2.0");
            return file.ToString();
        }
    }

    public static string CalendarFooters {
        get {
            var file = new StringWriter();
            file.WriteLine("END:VCALENDAR");
            return file.ToString();
        }
    }

    public static string Encode(string input) {
        input = input.Replace("\\", "\\\\");
        input = input.Replace(",", "\\,");
        input = input.Replace(";", "\\;");

        return input;
    }

    public static string BuildEvent(dynamic eventInfo) {

        var file = new StringWriter();

        file.WriteLine("BEGIN:VEVENT");

        if (eventInfo.AllDay) {
            file.WriteLine("DTSTART;VALUE=DATE:{0}", eventInfo.Start.ToString("yyyyMMdd"));
        } else {
            file.WriteLine("DTSTART: {0}", eventInfo.Start.ToString("yyyyMMdd\\THHmmss\\Z"));
            file.WriteLine("DTEND: {0}", eventInfo.End.ToString("yyyyMMdd\\THHmmss\\Z"));
        }
        file.WriteLine("LOCATION: {0}", IcsHelper.Encode(eventInfo.Location));
        file.WriteLine("DESCRIPTION: {0}", IcsHelper.Encode(eventInfo.Description));
        file.WriteLine("SUMMARY;LANGUAGE=en-us: {0}", IcsHelper.Encode(eventInfo.Name));

        var user = UserHelper.GetUser(eventInfo.OrganizerId);

        file.WriteLine("ORGANIZER;CN=\"{0}\":mailto:{1}", IcsHelper.Encode(user.Name), user.Email);
        file.WriteLine("UID:{0}", eventInfo.Id);
        file.WriteLine("PRIORITY:3");
        file.WriteLine("END:VEVENT");

        return file.ToString();
    }

    public static string BuildCalendar(int calendarId) {

        var file = new StringWriter();
        var calendar = Calendar.GetCalendar(calendarId);

        file.Write(CalendarHeaders);
        file.WriteLine("METHOD:PUBLISH");
        file.WriteLine("X-WR-RELCALID:{0}", calendarId);
        file.WriteLine("X-WR-CALNAME:{0}", calendar.Name);
        var events = Event.GetCalendarEvents(calendarId);
        foreach (var e in events) {
            file.Write(BuildEvent(e));
        }

        file.Write(CalendarFooters);
        return file.ToString();
    }
}