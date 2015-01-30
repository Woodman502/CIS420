using System;
using WebMatrix.Data;
using WebMatrix.WebData;

public static class UserHelper {

    public static string ConnectionStringName {
        get;
        private set;
    }

    public static string UserTableName {
        get;
        private set;
    }

    public static string UserNameField {
        get;
        private set;
    }

    public static string UserIdField {
        get;
        private set;
    }

    public static dynamic CurrentUser {
        get {
            // HTTPContext could be null
            dynamic current = RequestData.GetValue("CurrentUser");
            if (current == null) {
                current = GetUser(WebSecurity.CurrentUserId);
                RequestData.SetValue("CurrentUser", current);
            }
            return current;
        }
    }

    // Can we name this Database since there is also the Database.Open thing?
    public static dynamic DatabaseInstance {
        get {

            // HTTPContext could be null
            var db = RequestData.GetValue<Database>("Database");
            if (db == null) {
                db = Database.Open(ConnectionStringName);
                RequestData.SetValue("Database", db);
            }

            return db;
        }
    }

    public static void Initialize(string connectionStringName, string userTableName, string userIdColumn, string userNameColumn, bool autoCreateTables) {

        WebSecurity.InitializeDatabaseConnection(connectionStringName, userTableName, userIdColumn, userNameColumn, autoCreateTables);

        ConnectionStringName = connectionStringName;
        UserTableName = userTableName;
        UserNameField = userNameColumn;
        UserIdField = userIdColumn;
    }

    public static dynamic GetUser(int userId) {
        string query = String.Format("SELECT * FROM [{0}] WHERE [{1}] = @0", UserTableName, UserIdField);
        return DatabaseInstance.QuerySingle(query, userId);
    }

    public static bool UserExists(string userNameValue) {
        return WebSecurity.GetUserId(userNameValue) != -1;
    }

    public static int AddUser(string name, string email, string timeZone) {
        string query = String.Format("INSERT INTO [{0}] (Name, Email, TimeZone) VALUES (@0, @1, @2)", UserTableName);

        DatabaseInstance.QuerySingle(query, name, email, timeZone);

        return Convert.ToInt32(DatabaseInstance.GetLastInsertId());
    }
}