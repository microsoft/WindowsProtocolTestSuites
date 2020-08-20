// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Novell.Directory.Ldap;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Protocols.TestTools.StackSdk
{
    public static class LdapExtensions
    {
        public static IList<LdapAttribute> GetLdapEntryAttributes(this LdapEntry entry)
        {
            IList<LdapAttribute> attributes = new List<LdapAttribute>();

            LdapAttributeSet attributeSet = entry.GetAttributeSet();
            System.Collections.IEnumerator ienum = attributeSet.GetEnumerator();
            while (ienum.MoveNext())
            {
                LdapAttribute attribute = (LdapAttribute)ienum.Current;
                attributes.Add(attribute);
            }

            return attributes;
        }

        public static Dictionary<string, IList<LdapAttribute>> GetAllLdapEntries(this ILdapSearchResults searchResult)
        {
            Dictionary<string, IList<LdapAttribute>> entryList = new Dictionary<string, IList<LdapAttribute>>();
            while (searchResult.HasMore())
            {
                var entry = searchResult.Next();

                entryList.Add(entry.Dn, entry.GetLdapEntryAttributes());
            }
            return entryList;
        }

        public static string GetStringValueFromAttributes(this IList<LdapAttribute> listAttribures, string key)
        {
            var listQuery = listAttribures.Where(a => a.Name.Equals(key));
            if (listQuery.Any())
            {
                var attribute = listQuery.First();
                string attributeVal = attribute.StringValue;
                return attributeVal;
            }
            else
            {
                return null;
            }
        }

        public static IList<string> GetStringListValueFromAttributes(this IList<LdapAttribute> listAttribures, string key)
        {
            var listQuery = listAttribures.Where(a => a.Name.Equals(key));
            if (listQuery.Any())
            {
                var attribute = listQuery.First();
                return attribute.StringValueArray;
            }
            else
            {
                return null;
            }
        }

        public static byte[][] GetBytesValueFromAttributes(this IList<LdapAttribute> listAttribures, string key)
        {
            var listQuery = listAttribures.Where(a => a.Name.Equals(key));
            if (listQuery.Any())
            {
                var attribute = listQuery.First();
                return attribute.ByteValueArray;
            }
            else
            {
                return null;
            }
        }
    }
}
