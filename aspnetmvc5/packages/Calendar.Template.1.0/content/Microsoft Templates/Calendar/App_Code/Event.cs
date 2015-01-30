﻿using System;

public static class Event {
    /// <summary>
    /// Add an event to a calendar
    /// </summary>
    /// <param name="userId">The id of the creator of the event</param>
    /// <param name="calendarId">The id of the calendar to add the event to</param>
    /// <param name="name">The name of the event</param>
    /// <param name="description">The description of the event</param>
    /// <param name="location">The location of the event</param>
    /// <param name="utcStart">The start of the event</param>
    /// <param name="utcEnd">The end of the event</param>
    /// <param name="allDay">Whether the event is all day or not. If it is all day
    /// then the start and end TIMES play no role</param>
    /// <remarks>Only events that start and end on the same day are supported</remarks>
    /// <returns>The id of the added event</returns>
    public static int AddEvent(int userId, int calendarId, string name, string description, string location, DateTime utcStart, DateTime utcEnd, bool allDay = false) {
        var db = UserHelper.DatabaseInstance;
        db.Query(@"INSERT INTO Events
                    (OrganizerId,   CalendarId,     Name,   Description,    Location,   AllDay, Start,  [End])
             Values (@0,            @1,             @2,             @3,         @4,     @5,     @6,     @7)",
                    userId, calendarId, name, description, location, allDay, utcStart, utcEnd);

        return Convert.ToInt32(db.GetLastInsertId());
    }

    /// <summary>
    /// Edit an event
    /// </summary>
    /// <see>Event.AddEvent</see>>
    /// <param name="eventId">The id of the event to edit</param>
    public static void EditEvent(int eventId, int calendarId, string name, string description, string location, DateTime utcStart, DateTime utcEnd, bool allDay = false) {
        var db = UserHelper.DatabaseInstance;

        db.Execute(@"
                    UPDATE Events
                    SET CalendarId = @0, Name = @1, Description = @2, Location = @3, AllDay = @4, Start = @5, [End] = @6
                    WHERE Id = @7",
                    calendarId, name, description, location, allDay, utcStart, utcEnd,
                    eventId);
    }

    /// <summary>
    /// Delete an event
    /// </summary>
    /// <param name="eventId">The id of the event to delete</param>
    public static void DeleteEvent(int eventId) {
        var db = UserHelper.DatabaseInstance;

        db.Execute("DELETE FROM EVENTS WHERE Id = @0", eventId);
    }

    /// <summary>
    /// Get information about the user-calendar-event pairing
    /// </summary>
    /// <param name="userId">The user id to find</param>
    /// <param name="eventId">The event id to find</param>
    /// <returns>Null if no result matches that pairing, otherwise information regarding the pairing</returns>
    public static dynamic GetUserEvent(int userId, int eventId) {
        var db = UserHelper.DatabaseInstance;
        return db.QuerySingle(@"
            SELECT e.*, c.Name AS CalendarName, c.Id AS CalendarId, cu.Color, cu.Permissions
            FROM EVENTS AS e 
            JOIN Calendars AS c ON e.CalendarID = c.Id 
            JOIN CalendarsUsers AS cu ON c.Id = cu.CalendarId
            WHERE cu.UserId = @0 AND e.Id = @1",
            userId, eventId);
    }

    public static dynamic GetCalendarEvents(int calendarId) {
        var db = UserHelper.DatabaseInstance;
        return db.Query("SELECT * FROM Events WHERE CalendarId = @0", calendarId);
    }
}