// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Microsoft.Protocol.TestSuites.ActiveDirectory.Adts.Schema
{
    /// <summary>
    /// Provides a method to read in schema definitions from TD XML.
    /// </summary>
    public static class SchemaReader
    {
        /// <summary>
        /// Reads a schema from a set of XML files. Returns an enumeration of lines, where each line
        /// represents an attribute definition with ':' a separator between attribute name and attribute value, 
        /// and where the empty line indicates the end of an object definition. 
        /// </summary>

        /// <param name="includeFilePatterns">Included file folders</param>
        /// <param name="excludeFiles">Excluded file folder</param>
        /// <param name="isProtocolXMLApplied">Set this to true if new protocol XML applied</param>
        /// <param name="serverVersion">Specify the OS version of the server</param>
        /// <returns>Returns string array.</returns>
        public static string[] ReadSchema(string[] includeFilePatterns, string[] excludeFiles, bool isProtocolXMLApplied, OSVersion serverVersion)
        {
            bool criticalOnly = false;
            List<string> result = new List<string>();

            Checks.MakeLog("Start loading of XML data...");
            if (isProtocolXMLApplied == true)
            {
                foreach (string file in Expand(includeFilePatterns, excludeFiles))
                {
                    if (serverVersion >= OSVersion.Win2016)
                    {
                        #region raw XMLs used after Windows Server 2016 are changed to openXML type

                        using (WordprocessingDocument wdDoc = WordprocessingDocument.Open(file, false))
                        {
                            XNamespace w = @"http://schemas.openxmlformats.org/wordprocessingml/2006/main";
                            XNamespace xml = @"http://www.w3.org/XML/1998/namespace";

                            var items = wdDoc.MainDocumentPart.Document.Body;
                            XAttribute currentStyle = null;
                            XAttribute previousStyle = null;
                            string previousLine = "";
                            foreach (var item in items)
                            {
                                if (item is Paragraph)
                                {
                                    XElement xe = XElement.Parse(item.OuterXml);
                                    currentStyle = xe.Descendants(w + "pPr").Descendants(w + "pStyle").Attributes(w + "val").FirstOrDefault();
                                    if (currentStyle != null && currentStyle.Value.Equals("Code"))
                                    {
                                        var texts = xe.Descendants(w + "t");
                                        string currentLine = "";
                                        foreach (var text in texts)
                                        {
                                            currentLine += text.Value;
                                        }
                                        if (currentLine.Contains("<SchemaNCDN>"))
                                        {
                                            currentLine = currentLine.Replace("<SchemaNCDN>", "CN=Schema,CN=Configuration,<RootDomainDN>");
                                        }
                                        var testsAttr = texts.Attributes(xml + "space").FirstOrDefault();
                                        if (testsAttr != null && testsAttr.Value.Equals("preserve"))
                                        {
                                            if (currentLine.StartsWith(" "))
                                            {
                                                previousLine += currentLine.TrimStart();
                                            }
                                            else
                                            {
                                                if (!string.IsNullOrEmpty(previousLine))
                                                {
                                                    result.Add(previousLine);
                                                }
                                                previousLine = currentLine;
                                            }
                                        }
                                        else
                                        {
                                            if (!string.IsNullOrEmpty(previousLine))
                                            {
                                                result.Add(previousLine);
                                            }
                                            previousLine = currentLine;
                                        }
                                    }
                                    else if (previousStyle != null && previousStyle.Value.Equals("Code"))
                                    {
                                        if (!string.IsNullOrEmpty(previousLine))
                                        {
                                            // If the code does not end with an empty string, add one to seperate it with other schema objects in the result list.
                                            result.Add(previousLine);
                                            result.Add(string.Empty);
                                        }
                                        else
                                        {
                                            // There exists in TD when code already ends up with an empty string.
                                            result.Add(previousLine);
                                        }
                                        previousLine = "";
                                    }
                                    previousStyle = currentStyle;
                                }
                            }
                        }

                        #endregion
                    }
                    else
                    {
                        #region raw XMLs used before Windows Server 2012 R2

                        System.IO.StreamReader sr = new StreamReader(file);
                        string tmp = sr.ReadToEnd();
                        sr.Close();

                        //Get data from <snippet> element
                        int index = tmp.IndexOf("</snippet>");

                        if (serverVersion == OSVersion.WinSvr2008R2)
                        {
                            if (tmp.IndexOf("windows_unreleased") > -1)
                            {
                                if (tmp.IndexOf("</snippet>", tmp.IndexOf("windows_unreleased")) > -1)
                                {
                                    string temper = tmp.Substring(tmp.IndexOf("windows_unreleased"));
                                    if (temper.ToLower().Contains("cn:") && temper.ToLower().Contains("ldapdisplayname:"))
                                    {
                                        tmp = temper;
                                        index = tmp.IndexOf("</snippet>");
                                    }
                                }
                            }
                        }

                        if (index > -1)
                        {
                            index = tmp.LastIndexOf(">", index) + 1;
                            string sp = tmp.Substring(index, tmp.IndexOf("</snippet>") - index);
                            index = tmp.IndexOf("<snippet");
                            string spTag = tmp.Substring(index, tmp.IndexOf(">", index) - index + 1);
                            List<string> autoText = new List<string>();
                            index = 0;

                            //Get platform information
                            while (index < tmp.Length)
                            {
                                int begin = tmp.IndexOf("<auto_text>", index);

                                if (begin > -1)
                                {
                                    int end = tmp.IndexOf("</auto_text>", index);

                                    if (begin > -1 && end > -1 && end > begin)
                                    {
                                        string newAdd = tmp.Substring(begin + "<auto_text>".Length, end - begin - "<auto_text>".Length);
                                        bool toAdd = true;

                                        foreach (string cont in autoText)
                                        {
                                            if (cont == newAdd)
                                            {
                                                toAdd = false;
                                                break;
                                            }
                                        }
                                        if (toAdd == true)
                                        {
                                            autoText.Add(newAdd);
                                        }
                                        index = end + "</auto_text>".Length;
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }

                            List<string> implementationSpecific = new List<string>();

                            foreach (string tagTla in autoText)
                            {
                                if (!tagTla.Equals("windows") && !tagTla.Equals("exchange_server"))
                                {
                                    implementationSpecific.Add(tagTla);
                                }
                            }
                            List<string> lines = new List<string>();
                            bool isCritical = false;
                            string value = sp;
                            value = value.Replace(" ", String.Empty);
                            //Modified. For new XML files, lines are separated by \n, not \n\r
                            string[] splitTag = { "\n" };
                            string[] tmpValue = value.Split(splitTag, StringSplitOptions.RemoveEmptyEntries);
                            string parsedValue = String.Empty;

                            for (int k = 0; k < tmpValue.Length; k++)
                            {
                                if (!tmpValue[k].Contains(":") || tmpValue[k].IndexOf(":") == 1)
                                {
                                    parsedValue = parsedValue + tmpValue[k];
                                }
                                else
                                {
                                    parsedValue = parsedValue + "\r\n" + tmpValue[k];
                                }
                            }
                            if (parsedValue.Contains("&lt;RootDomainDN&gt;"))
                            {
                                parsedValue = parsedValue.Replace("&lt;RootDomainDN&gt;", "<RootDomainDN>");
                            }
                            else if (parsedValue.Contains("&lt;SchemaNCDN&gt;"))
                            {
                                parsedValue = parsedValue.Replace("&lt;SchemaNCDN&gt;", "CN=Schema,CN=Configuration,<RootDomainDN>");
                            }
                            string[] splitTag2 = { "\r\n" };
                            tmpValue = parsedValue.Split(splitTag2, StringSplitOptions.RemoveEmptyEntries);
                            string savedString = String.Empty;

                            foreach (string line in tmpValue)
                            {
                                if (line.Length > 0)
                                {
                                    if (line[line.Length - 1] != ':')
                                    {
                                        lines.Add(savedString + line);
                                        if (criticalOnly && !isCritical)
                                        {
                                            isCritical = CheckCritical(line);
                                        }
                                        savedString = String.Empty;
                                    }
                                    else
                                    {
                                        savedString = line;
                                    }
                                }
                            }
                            if (!criticalOnly || isCritical)
                            {
                                result.AddRange(lines);
                                result.Add(String.Empty); //separate records by an empty element
                            }
                        }

                        #endregion
                    }
                }
            }
            else
            {
                foreach (string file in Expand(includeFilePatterns, excludeFiles))
                {
                    XmlTextReader reader = new XmlTextReader(file);
                    reader.XmlResolver = null;
                    reader.DtdProcessing = DtdProcessing.Prohibit;
                    XPathDocument doc = new XPathDocument(reader);
                    XPathNavigator node = doc.CreateNavigator();

                    // The TD XML has no formal tag for representing the object definitions in ADA* and ADSC.
                    // However, it seems that all those definitions are included in an <example> tag which
                    // in turn contains a <snippet> tag with CDATA, and the snippet tag has a type attribute "syntax".
                    // We use this for identifying the object definitions. However, this code may require
                    // changes if the TD XML representation changes, or if it discovered that we include
                    // too many or not enough objects.
                    foreach (XPathNavigator snippet in node.Select("//example/snippet"))
                    {
                        List<string> implementationSpecific = new List<string>();
                        foreach (XPathNavigator tagTla in node.Select("//p/tla"))
                        {
                            if (
                                !tagTla.GetAttribute("rid", String.Empty).Equals("windows") 
                                && !tagTla.GetAttribute("rid", String.Empty).Equals("exchange_server"))
                            {
                                implementationSpecific.Add(tagTla.GetAttribute("rid", String.Empty));
                            }
                        }

                        if (
                            serverVersion != OSVersion.WinSvr2008R2 
                            && (implementationSpecific.Count == 1 
                            && (implementationSpecific.Contains("windows_server_7"))))
                        {
                            continue;
                        }
                        List<string> lines = new List<string>();
                        bool isCritical = false;
                        string value = snippet.Value;
                        int i = 0;

                        while (i >= 0)
                        {
                            string line = ExtractLine(value, ref i).Trim();

                            while (i >= 0 && Char.IsWhiteSpace(value[i]))
                            {
                                line += ExtractLine(value, ref i).Trim();
                            }
                            if (line != String.Empty)
                            {
                                lines.Add(line);
                                if (criticalOnly && !isCritical)
                                {
                                    isCritical = CheckCritical(line);
                                }
                            }
                        }
                        if (!criticalOnly || isCritical)
                        {
                            result.AddRange(lines);
                            result.Add(String.Empty);
                        }
                    }
                }
            }
            Checks.MakeLog("Successfully loaded XML data files...");
            return result.ToArray();
        }

        static bool CheckCritical(string line)
        {
            line = line.ToLower();
            int i = line.IndexOf(':');
            if (i >= 0)
            {
                string attr = line.Substring(0, i).Trim();
                string value = line.Substring(i + 1);
                if (attr == "schemaflagsex" && value.Contains("flag_attr_is_critical"))
                {
                    return true;
                }
                if (attr == "systemflags" && value.Contains("flag_schema_base_object"))
                {
                    return true;
                }
            }
            return false;
        }


        static string ExtractLine(string cont, ref int i)
        {
            int j = cont.IndexOf("\r\n", i);
            string result;
            if (j >= 0)
            {
                result = cont.Substring(i, j - i);
                i = j + 2;
            }
            else
            {
                result = cont.Substring(i);
                i = -1;
            }
            return result;
        }


        static IEnumerable<string> Expand(string[] includeFilePatterns, string[] excludeFiles)
        {
            foreach (string pat in includeFilePatterns)
            {
                if (pat.Contains("*"))
                {
                    string dirName = Path.GetDirectoryName(pat);

                    if (dirName == String.Empty)
                    {
                        dirName = ".";
                    }
                    string searchPattern = Path.GetFileName(pat);       
                    foreach (string file in Directory.GetFiles(dirName, searchPattern))
                    {
                        if (!Matches(file, excludeFiles))
                        {
                            yield return file;
                        }
                    }
                }
                else if (!Matches(pat, excludeFiles))
                {
                    yield return pat;
                }

            }
        }

        static bool Matches(string file, string[] excluded)
        {
            return excluded != null && excluded.FirstOrDefault<string>(ex => file.IndexOf(ex) >= 0) != null;
        }
    }
}
