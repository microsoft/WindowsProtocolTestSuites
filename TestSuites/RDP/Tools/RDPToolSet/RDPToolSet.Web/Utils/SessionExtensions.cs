// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CodecToolSet.Core;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Timers;

namespace RDPToolSet.Web.Utils
{
    public static class SessionExtensions
    {
        public static int SessionTimeOut = 60;

        private const string UserIdentityKey = "UserIdentityKey";
        private static Dictionary<string, Dictionary<string, object>> sessionCache = new Dictionary<string, Dictionary<string, object>>();
        private static Dictionary<string, DateTime> sessionTimeoutCheck = new Dictionary<string, DateTime>();
        private static System.Timers.Timer timer = new System.Timers.Timer(5000);
        private static ReaderWriterLockSlim rwl = new ReaderWriterLockSlim();

        static SessionExtensions()
        {
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        private static void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            List<string> expiredKey = new List<string>();
            rwl.EnterReadLock();
            try
            {
                sessionTimeoutCheck.ForEach((KeyValuePair<string, DateTime> kvp) =>
                {
                    if ((DateTime.UtcNow - kvp.Value).TotalMinutes > SessionTimeOut)
                    {
                        expiredKey.Add(kvp.Key);
                    }
                });
            }
            finally
            {
                rwl.ExitReadLock();
            }

            if (expiredKey.Count > 0)
            {
                rwl.EnterWriteLock();
                try
                {
                    expiredKey.ForEach(key =>
                    {
                        sessionCache.Remove(key);
                        sessionTimeoutCheck.Remove(key);
                    });
                }
                finally
                {
                    rwl.ExitWriteLock();
                }
            }
        }

        public static void SetObject(this ISession session, string key, object value)
        {
            string userSessionKey = session.GetString(UserIdentityKey);
            rwl.EnterWriteLock();
            try
            {
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
                ResetTimeSticks(userSessionKey);
            }
            finally
            {
                rwl.ExitWriteLock();
            }
        }

        public static T Get<T>(this ISession session, string key)
        {
            string userSessionKey = session.GetString(UserIdentityKey);
            rwl.EnterReadLock();
            try
            {
                if (string.IsNullOrEmpty(userSessionKey) || !sessionCache.ContainsKey(userSessionKey))
                {
                    return default;
                }

                Dictionary<string, object> userSession = sessionCache[userSessionKey];
                ResetTimeSticks(userSessionKey);
                if (userSession.ContainsKey(key))
                {
                    return (T)userSession[key];
                }
                else
                {
                    return default;
                }
            }
            finally
            {
                rwl.ExitReadLock();
            }
        }

        private static void ResetTimeSticks(string key)
        {
            sessionTimeoutCheck[key] = DateTime.UtcNow;
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
}
