using System;

/*
The goal of this class is to provide an easy interface for a web
developer to make a web application that uses timezones that are
user specific that might be different from the server. The server
might not even be sitting at UTC.
*/
public static class Time {

    // The user's timezone
    public static string LocalTimeZoneId {
        get {
            var timeZone = RequestData.GetValue<string>("LocalTimeZone");
            if (timeZone == null) {
                throw new InvalidOperationException("The Request Local Time Zone has not been set");
            }

            return timeZone;
        }
        set {
            // If an invalid id is passed, it will throw an InvalidOperationException
            RequestData.SetValue("LocalTimeZone", value);
        }
    }

    public static DateTime NowInLocal {
        get {
            return UtcToLocal(DateTime.UtcNow);
        }
    }

    public static DateTime LocalToUtc(DateTime localTime) {
        return TimeZoneInfo.ConvertTimeToUtc(localTime, TimeZoneInfo.FindSystemTimeZoneById(LocalTimeZoneId));
    }

    public static DateTime UtcToLocal(DateTime utcTime) {
        return TimeZoneInfo.ConvertTimeFromUtc(utcTime, TimeZoneInfo.FindSystemTimeZoneById(LocalTimeZoneId));
    }
}