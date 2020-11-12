using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace RDPToolSet.WebCore.Utils
{
    public static class SessionExtensions
    {
        private const string UserIdentityKey = "UserIdentityKey";
        private static Dictionary<string, Dictionary<string, object>> sessionCache = new Dictionary<string, Dictionary<string, object>>();

        public static void SetObject(this ISession session, string key, object value)
        {
            string userSessionKey = session.GetString(UserIdentityKey);
            if (!string.IsNullOrEmpty(userSessionKey) && sessionCache.ContainsKey(userSessionKey))
            {
                Dictionary<string, object> userSession = sessionCache[userSessionKey];
                userSession[key] = value;
            }
            else
            {
                Dictionary<string, object> userSession = new Dictionary<string, object>();
                userSession[key] = value;
                if (string.IsNullOrEmpty(userSessionKey))
                {
                    userSessionKey = Guid.NewGuid().ToString();
                    session.SetString(UserIdentityKey, userSessionKey);
                }
                sessionCache[userSessionKey] = userSession;
            }
        }

        public static T Get<T>(this ISession session, string key)
        {
            string userSessionKey = session.GetString(UserIdentityKey);
            if (string.IsNullOrEmpty(userSessionKey) || !sessionCache.ContainsKey(userSessionKey))
            {
                return default;
            }

            Dictionary<string, object> userSession = sessionCache[userSessionKey];
            if (userSession.ContainsKey(key))
            {
                return (T)userSession[key];
            }
            else
            {
                return default;
            }
        }


        //public static void Set<T>(this ISession session, string key, T value)
        //{
        //    SessionObject obj = new SessionObject(value.GetType().AssemblyQualifiedName, JsonConvert.SerializeObject(value, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto}));
        //    session.SetString(key, JsonConvert.SerializeObject(obj));
        //}

        //public static T Get<T>(this ISession session, string key)
        //{
        //    var objContent = session.GetString(key);
        //    if(objContent == null)
        //    {
        //        return default;
        //    }
        //    var obj = JsonConvert.DeserializeObject<SessionObject>(objContent);
        //    if(obj == null)
        //    {
        //        return default;
        //    }
        //    Type targetType = Type.GetType(obj.ObjectType);
        //    var targetObject = JsonConvert.DeserializeObject(obj.ObjectContent, targetType, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto });
        //    return (T)targetObject;
        //}
    }

    public class SessionObject
    {
        public SessionObject()
        {

        }

        public SessionObject(string objectType, string objectContent)
        {
            this.ObjectType = objectType;
            this.ObjectContent = objectContent;
        }

        public string ObjectType { get; set; }

        public string ObjectContent { get; set; }
    }
}
