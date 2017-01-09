// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;
using System.Management;

namespace FileStreamDataParser
{
    /// <summary>
    /// This enum indicates the type of 
    /// date-time property to be retrieved.
    /// </summary>
    public enum fileDateTimeType
    {
        //Represents the File Creation Time
        CreationDate,

        //Represents the time when 
        //the file was last accessed.
        LastAccessed,

        //Represents the time when 
        //the file was last modified.
        LastModified
    }

    /// <summary>
    /// This enum is to check whether
    /// a file is present or not.
    /// </summary>
    public enum fileCheck
    {
        //File Exists
        FileExists,

        //File does not exist.
        FileNotPresent
    }

    public class Helper
    {        
        #region GetSubArray

        /// <summary>
        /// This method is used to extract and return "length" number
        /// of bytes from a byte array starting from the given stratIndex.
        /// </summary>
        /// <param name="byteArray">
        /// Byte array to be parsed.
        /// </param>
        /// <param name="startIndex">
        /// Starting index of the required byte array.
        /// </param>
        /// <param name="length">
        /// Number of bytes in the required byte array.
        /// </param>
        /// <returns>
        /// Returns the Sub-Array
        /// Required byte array or, 
        /// null in case of some error in byte parsing.
        /// </returns>
        public byte[] GetSubArray(byte[] byteArray, int startIndex, int length)
        {
            if (length < 0)
            {
                return null;
            }
            else if (startIndex < 0)
            {
                return null;
            }
            else if (length > byteArray.Length)
            {
                return null;
            }

            byte[] temp = new byte[length];
            Array.Copy(byteArray, startIndex, temp, 0, length);
            return temp;
        }

        #endregion

        /// <summary>
        /// Combines high and low bits 
        /// and returns the combined Integer value.
        /// </summary>
        /// <param name="dwLowDateTime">Low Bits</param>
        /// <param name="dwHighDateTime">high Bits</param>
        /// <returns>
        /// Returns UInt64 value.
        /// </returns>
        public UInt64 Get__FILETIMEValue(UInt32 dwLowDateTime, UInt32 dwHighDateTime)
        {
            UInt64 low = dwLowDateTime;
            UInt64 high = dwHighDateTime;

            high = high << 32;

            return (high | low);
        }

        /// <summary>
        /// This method reads and extracts the date-time attributes
        /// from CIM-TYPE-DATETIME format and stores it in
        /// DateTime structure.
        /// </summary>
        /// <param name="dateTime">
        /// The string in the CIM-TYPE-DATETIME format.
        /// </param>
        /// <returns>
        /// It returns DateTime structure with only
        /// year-month-day-hour-minute-second filled.
        /// </returns>
        public DateTime FillDateTimeStructure(string dateTime)
        {
            //FORMAT of CIM-DATETIME
            //yyyymmddhhmmss.mmmmmmmm+300 (+ or - depending on offset)
            string[] cimDateTime = new string[2];
            Int64 yyyymmddhhmmss;

            cimDateTime = dateTime.Split(new char[] { '.' });
            yyyymmddhhmmss = Int64.Parse(cimDateTime[0]);

            int year = (int)(yyyymmddhhmmss / 10000000000);
            int month = (int)(yyyymmddhhmmss / 100000000) % year;
            int date = (int)((yyyymmddhhmmss / 1000000) % (yyyymmddhhmmss / 100000000));
            int hour = (int)((yyyymmddhhmmss / 10000) % (yyyymmddhhmmss / 1000000));
            int minute = (int)((yyyymmddhhmmss / 100) % (yyyymmddhhmmss / 10000));
            int second = (int)(yyyymmddhhmmss % 100);

            DateTime dt = new DateTime(year, month, date, hour, minute, second);
            return dt;
        }

        /// <summary>
        /// This method retrieves the dates and times of the 
        /// file getting replicated by the server
        /// </summary>
        /// <param name="directoryName">
        /// Path of the directory where the file 
        /// resides in the server.
        /// </param>
        /// <param name="fileName">
        /// Name of the file being replicated at present.
        /// The name should be without file extension.
        /// </param>
        /// <param name="computername">
        /// The IP address of the server.
        /// </param>
        /// <param name="domainname">
        /// Domain name in which client and server resides.
        /// </param>
        /// <param name="username">
        /// username to log onto the server.
        /// </param>
        /// <param name="password">
        /// password to log onto to the server.
        /// </param>
        /// <param name="dtType">
        /// This enum specifies which type of date-time 
        /// property is to be retrieved.
        /// </param>
        /// <returns>
        /// It returns DateTime structure with only 
        /// year-month-day-hour-minute-second filled.
        /// </returns>
        public DateTime GetDateTime(string directoryName,
                                    string fileName,
                                    string computername,
                                    string domainname,
                                    string username,
                                    string password,
                                    fileDateTimeType dtType,
                                    ref fileCheck fileChk)
        {
            string sql = "ASSOCIATORS OF " +
                "{Win32_Directory='" + directoryName + "'} " +
                "WHERE " +
                "AssocClass=CIM_DirectoryContainsFile " +
                "ResultClass=CIM_DataFile " +
                "ResultRole=PartComponent " +
                "Role=GroupComponent";

            string path = "\\root\\cimv2";
            ConnectionOptions oConn = new ConnectionOptions();
            oConn.Username = domainname + @"\" + username;
            oConn.Password = password;
            
            //ManagementScope scope = new System.Management.ManagementScope("root\\cimv2");
            ManagementScope scope = new System.Management.ManagementScope("\\\\" + computername + path, oConn);
            System.Management.RelatedObjectQuery oQuery = new System.Management.RelatedObjectQuery(sql);
            ManagementObjectSearcher oSearcher = new ManagementObjectSearcher(scope, oQuery);
            ManagementObjectCollection oFiles = oSearcher.Get();

            DateTime dt = new DateTime();
            int count = 0;

            foreach (ManagementObject oFile in oFiles)
            {
                string fileInDirectory = Convert.ToString(oFile["FileName"]);
                if (fileInDirectory.ToUpper() == fileName.ToUpper())
                {
                    count++;
                    switch (dtType)
                    {
                        case fileDateTimeType.CreationDate:
                            string creationTime = Convert.ToString(oFile["CreationDate"]);
                            dt = FillDateTimeStructure(creationTime);
                            break;

                        case fileDateTimeType.LastAccessed:
                            string lastAccessedTime = Convert.ToString(oFile["LastAccessed"]);
                            dt = FillDateTimeStructure(lastAccessedTime);
                            break;

                        case fileDateTimeType.LastModified:
                            string lastModifiedTime = Convert.ToString(oFile["LastModified"]);
                            dt = FillDateTimeStructure(lastModifiedTime);
                            break;
                    }
                }
            }

            if (count == 0)
            {
                fileChk = fileCheck.FileNotPresent;
            }

            return dt;
        }
    }
}
