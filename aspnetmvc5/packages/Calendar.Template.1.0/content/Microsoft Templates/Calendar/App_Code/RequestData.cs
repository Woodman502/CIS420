using System.Collections.Generic;
using System.Web;

/// <summary>
/// Helper class to store information in context
/// that is very unlikely to collide with other values
/// 
/// The data stored in this helper live only throughout the
/// request.
/// </summary>
public static class RequestData {
    // The key to use as a faux namespace in context
    private static readonly object _key = new object();

    /// <summary>
    /// Get the entire collection of information stored
    /// in our namespace
    /// </summary>
    /// <returns>A dictionary of settings</returns>
    private static IDictionary<string, object> GetSettings() {
        return GetSettings(new HttpContextWrapper(HttpContext.Current));
    }

    internal static IDictionary<string, object> GetSettings(HttpContextBase context) {
        var settings = context.Items[_key] as IDictionary<string, object>;

        if (settings == null) {
            // We will create it
            settings = new Dictionary<string, object>();
            context.Items[_key] = settings;
        }

        return settings;
    }

    // Get the generic object of the item stored in the context
    public static object GetValue(string key) {
        return GetValue<object>(new HttpContextWrapper(HttpContext.Current), key);
    }

    public static TValue GetValue<TValue>(string key) {
        return GetValue<TValue>(new HttpContextWrapper(HttpContext.Current), key);
    }

    internal static TValue GetValue<TValue>(HttpContextBase context, string key) {
        var settings = GetSettings(context);
        object value;

        if (settings.TryGetValue(key, out value)) {
            return (TValue)value;
        }

        return default(TValue);
    }

    public static void SetValue(string key, object value) {
        SetValue(new HttpContextWrapper(HttpContext.Current), key, value);
    }

    internal static void SetValue(HttpContextBase context, string key, object value) {
        var settings = GetSettings(context);
        settings[key] = value;
    }
}