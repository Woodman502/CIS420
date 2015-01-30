using System;
using System.Linq;
using System.Collections.Generic;

// This class does calendar based actions, add, edit, remove, fetch, etc.
public static class Calendar {

    public static void UpdateCalendarColor(int userId, int calendarId, string color) {
        var db = UserHelper.DatabaseInstance;
        db.Execute("UPDATE CalendarsUsers SET Color = @0 WHERE CalendarId = @1 AND UserId = @2", color, calendarId, userId);
    }

    public static void UpdateCalendar(int userId, int calendarId, string calendarName = null, string color = null) {
        var db = UserHelper.DatabaseInstance;

        color = color ?? ColorHelper.GetRandomColor();
        calendarName = calendarName ?? "Default";

        // We are editing a calendar
        db.Execute("UPDATE Calendars SET Name = @0 WHERE Id = @1", calendarName, calendarId);
        UpdateCalendarColor(userId, calendarId, color);
    }

    public static int CreateCalendar(int userId, string calendarName = null, string color = null) {
        var db = UserHelper.DatabaseInstance;

        color = color ?? ColorHelper.GetRandomColor();
        calendarName = calendarName ?? "Default";

        // Create a new calendar
        db.Execute("INSERT INTO Calendars (Name, Creator, Guid) Values (@0, @1, @2)", calendarName, userId, GenerateUniqueId());

        int calendarId = Convert.ToInt32(db.GetLastInsertId());

        // Add the calendar id to CalendarUsers
        db.Execute("INSERT INTO CalendarsUsers (CalendarId, UserId, Permissions, Color) VALUES (@0, @1, @2, @3)", calendarId, userId, Permission.Own, color);

        return calendarId;
    }

    private static string GenerateUniqueId() {
        return Guid.NewGuid().ToString();

    }

    /// <summary>
    /// Deletes a calendar and all events on that calendar
    /// </summary>
    /// <param name="calendarId">The id of the calendar to delete</param>
    public static void DeleteCalendar(int calendarId) {
        var db = UserHelper.DatabaseInstance;

        db.Execute("DELETE FROM Events WHERE CalendarId = @0", calendarId);
        db.Execute("DELETE FROM CalendarsUsers WHERE CalendarId = @0", calendarId);
        db.Execute("DELETE FROM Calendars WHERE Id = @0", calendarId);
    }

    /// <summary>
    /// Get calendars that userId has permissions on.
    /// </summary>
    /// <param name="userId">The id of the user to look for</param>
    /// <param name="minimumPermission">The minimum value of permission the user must have on the calendar</param>
    /// <returns>Set of rows ordered first by permissions(desc) then by name (asc)</returns>
    public static dynamic GetUserCalendars(int userId, Permission minimumPermission = Permission.View) {
        var db = UserHelper.DatabaseInstance;

        return db.Query(@"SELECT c.Name, c.Creator, c.Guid, cu.* 
                            FROM Calendars AS c 
                            JOIN CalendarsUsers AS cu ON c.Id = cu.CalendarId
                            WHERE cu.UserId = @0
                            AND cu.Permissions >= @1
                            ORDER BY cu.Permissions DESC, c.name ASC",
                            userId, minimumPermission);
    }

    /// <summary>
    /// Get calendar information specified by a user calendar pair
    /// </summary>
    /// <param name="userId">The id of the user</param>
    /// <param name="calendarId">The id of the calendar</param>
    /// <returns>null if no calendar exists, otherwise returns calendar information</returns>
    public static dynamic GetUserCalendar(int userId, int calendarId) {
        var db = UserHelper.DatabaseInstance;

        return db.QuerySingle(@"SELECT c.Name, c.Creator, c.Guid, cu.*
                                FROM Calendars AS c
                                JOIN CalendarsUsers AS cu ON c.Id = cu.CalendarId
                                WHERE cu.UserId = @0
                                AND cu.CalendarId = @1",
                                userId, calendarId);
    }

    public static dynamic GetCalendarGroups(int userId) {
        // I should maintain a cache of all of the calendars loaded on the page
        // to reduce calendar queries / joins
        var calendars = Calendar.GetUserCalendars(userId);

        var calendarGroups =
            (from dynamic c in (IEnumerable<object>)calendars
             group c by c.Permissions >= (int)Permission.Own into g
             orderby g.Key
             select new {
                 Own = g.Key ? "Mine" : "Shared",
                 Calendars = g.OrderBy(c => c.Name)
             }).ToDictionary(g => g.Own, g => g.Calendars);

        return calendarGroups;
    }

    public static dynamic GetCalendar(int calendarId) {
        var db = UserHelper.DatabaseInstance;
        return db.QuerySingle("SELECT * FROM Calendars WHERE Id = @0", calendarId);
    }

    public static dynamic GetCalendarByHash(int calendarId, string hash) {
        var db = UserHelper.DatabaseInstance;
        return db.QuerySingle("SELECT * FROM Calendars WHERE Id = @0 AND Guid = @1", calendarId, hash);
    }
}