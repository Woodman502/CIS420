using System;

public static class Sharing {
    // Get all people with access
    // Add access
    // Edit access

    public static dynamic GetUsersWithAccess(int calendarId, Permission minPermission = Permission.View) {
        var db = UserHelper.DatabaseInstance;

        return db.Query(@"
                SELECT u.*, cu.Permissions 
                FROM Users AS u
                JOIN CalendarsUsers AS cu ON u.Id = cu.UserId
                JOIN Calendars AS c ON cu.CalendarId = c.Id
                WHERE c.Id = @0 AND cu.Permissions >= @1
                ORDER BY cu.Permissions DESC,
                u.Email ASC",
                calendarId, minPermission);
    }

    // Give user access to calendar 
    public static void ShareCalendar(int calendarId, int userId, Permission permission, string color = null) {
        color = color ?? ColorHelper.GetRandomColor();

        var db = UserHelper.DatabaseInstance;
        db.Execute("INSERT INTO CalendarsUsers (CalendarId, UserId, Permissions, Color) VALUES (@0, @1, @2, @3)", calendarId, userId, permission, color);
    }

    private static void RemoveAccess(int calendarId, int userId) {
        var db = UserHelper.DatabaseInstance;

        db.Execute("DELETE FROM CalendarsUsers WHERE CalendarId = @0 AND UserId = @1", calendarId, userId);
    }

    public static void EditShareCalendar(int calendarId, int userId, Permission permission) {
        var db = UserHelper.DatabaseInstance;

        if (permission == Permission.NoAccess) {
            RemoveAccess(calendarId, userId);
        } else {
            db.Execute("UPDATE CalendarsUsers SET Permissions = @0 WHERE CalendarId = @1 AND UserId = @2", permission, calendarId, userId);
        }
    }
}