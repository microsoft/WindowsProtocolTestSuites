// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Protocols.TestManager.Kernel
{
    /// <summary>
    /// This class defines the filter of test case.
    /// </summary>
    public class Filter
    {
        List<string> positiveCategories;
        List<string> negativeCategories;
        RuleType type;

        /// <summary>
        /// Constructor of Filter.
        /// </summary>
        /// <param name="categories">A list of categories</param>
        /// <param name="type">the rule</param>
        public Filter(List<string> categories, RuleType type)
        {
            positiveCategories = new List<string>();
            negativeCategories = new List<string>();
            foreach (string item in categories)
            {
                if (item[0] != '!') positiveCategories.Add(item);
                else negativeCategories.Add(item.Substring(1));
            }
            this.type = type;
        }

        /// <summary>
        /// Filters test cases according to the rule.
        /// </summary>
        /// <param name="categories"></param>
        /// <returns></returns>
        public bool FilterTestCase(List<string> categories)
        {
            if (type == RuleType.Selector)
            {
                foreach (var c in categories) if (positiveCategories.Contains(c)) return true;
                foreach (var c in negativeCategories) if (!categories.Contains(c)) return true;
                return false;
            }
            else
            {
                foreach (var c in categories) if (positiveCategories.Contains(c)) return false;
                foreach (var c in negativeCategories) if (!categories.Contains(c)) return false;
                return true;
            }
        }
    }
}
