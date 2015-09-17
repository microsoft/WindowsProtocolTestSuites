// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService
{
    /// <summary>
    /// This is a 16-bit value in little-endian byte order used to encode a date. 
    /// An SMB_DATE value SHOULD be interpreted as follows. The date is represented 
    /// in the local time zone of the server. This field names below are provided 
    /// for reference only.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum SmbDateBitMask : ushort
    {
        /// <summary>
        /// none
        /// </summary>
        NONE = 0,

        /// <summary>
        /// The year. Add 1980 to the resulting value to return the actual year
        /// the binary form is 1111111000000000
        /// </summary>
        YEAR = 0xFE00,

        /// <summary>
        /// The month. Values range from 1 to 12.
        /// the binary form is 0000000111100000
        /// </summary>
        MONTH = 0x01E0,

        /// <summary>
        /// The date. Values range from 1 to 31.
        /// the binary form is 0000000000011111
        /// </summary>
        DAY = 0x001F,
    }


    /// <summary>
    /// This is a 16-bit value in little-endian byte order used to encode a date.
    /// An SMB_DATE value SHOULD be interpreted as follows. The date is represented 
    /// in the local time zone of the server. This field names below are provided 
    /// for reference only
    /// </summary>
    public struct SmbDate
    {
        private ushort date;

        /// <summary>
        /// The year. Add 1980 to the resulting value to return the actual year
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The Year should be in range of [1980, 2099].</exception>
        public ushort Year
        {
            get
            {
                return (ushort)((((this.date) & (ushort)SmbDateBitMask.YEAR) >> 9) + 1980);
            }
            set
            {
                if (value < 1980 || value > 2099)
                {
                    throw new ArgumentOutOfRangeException("value",
                    "Failed in Set_Year_Property of SmbDate: The Year should be in range of [1980, 2099].");
                }
                // Year occupys the high 9 bits of date, Month occupys the next 5 bits,
                // the left bits are belong to Day
                this.date = (ushort)(((value - 1980) << 9) + (this.Month << 5) + this.Day);
            }
        }


        /// <summary>
        ///  The month. Values range from 1 to 12.
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The Month should be in range of [1, 12].</exception>
        public ushort Month
        {
            get
            {
                return (ushort)((this.date & (ushort)SmbDateBitMask.MONTH) >> 5);
            }
            set
            {
                if (value < 1 || value > 12)
                {
                    throw new ArgumentOutOfRangeException("value",
                        "Failed in Set_Month_Property of SmbDate: The Month should be in range of [1, 12].");
                }
                // Year occupys the high 9 bits of date, Month occupys the next 5 bits,
                // the left bits are belong to Day
                this.date = (ushort)(((this.Year - 1980) << 9) + (value << 5) + this.Day);
            }
        }


        /// <summary>
        /// The date. Values range from 1 to 31.
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The day should be in range of [1, 31].</exception>
        public ushort Day
        {
            get
            {
                return (ushort)(this.date & (ushort)SmbDateBitMask.DAY);
            }
            set
            {
                if (value < 1 || value > 31)
                {
                    throw new ArgumentOutOfRangeException("value",
                        "Failed in Set_Day_Property of SmbDate: The day should be in range of [1, 31].");
                }
                // Year occupys the high 9 bits of date, Month occupys the next 5 bits,
                // the left bits are belong to Day
                this.date = (ushort)(((this.Year - 1980) << 9) + (this.Month << 5) + value);
            }
        }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="year">the year</param>
        /// <param name="month">the month</param>
        /// <param name="day">the day</param>
        public SmbDate(ushort year, ushort month, ushort day)
            : this()
        {
            Year = year;
            Day = day;
            Month = month;
        }


        /// <summary>
        /// translate itself to byte array
        /// </summary>
        /// <returns>the translated byte array</returns>
        public byte[] ToBytes()
        {
            return BitConverter.GetBytes(date);
        }
    }


    /// <summary>
    /// The number of 100-nanosecond intervals that have elapsed since January 1, 1601, in Coordinated 
    /// Universal Time (UTC) format.
    /// </summary>
    public struct FileTime
    {
        /// <summary>
        /// The number of 100-nanosecond intervals that have elapsed since January 1, 1601, in Coordinated 
        /// Universal Time (UTC) format.
        /// </summary>
        public ulong Time;
    }


    /// <summary>
    /// This is a 16-bit value in little-endian byte order use to encode a time of day.
    /// The SMB_TIME value is usually accompanied by an SMB_DATE value that indicates 
    /// what date corresponds with the given time. An SMB_TIME value SHOULD be interpreted 
    /// as follows. This field names below are provided for reference only. The time is 
    /// represented in the local time zone of the server.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum SmbTimeBitMask : ushort
    {
        /// <summary>
        /// none
        /// </summary>
        NONE = 0,

        /// <summary>
        /// The hours. Values range from 0 to 23.
        /// the binary form is 1111100000000000
        /// </summary>
        HOUR = 0xF800,

        /// <summary>
        /// The minutes. Values range from 0 to 59
        /// the binary form is 0000011111100000
        /// </summary>
        MINUTES = 0x07E0,

        /// <summary>
        /// The seconds. Values MUST represent two-second increments
        /// the binary form is 0000000000011111
        /// </summary>
        SECONDS = 0x001F,
    }


    /// <summary>
    /// This is a 16-bit value in little-endian byte order use to encode a time of day. 
    /// The SMB_TIME value is usually accompanied by an SMB_DATE value that indicates what
    /// date corresponds with the given time. An SMB_TIME value SHOULD be interpreted as follows. 
    /// This field names below are provided for reference only. The time is represented in the local 
    /// time zone of the server
    /// </summary>
    public struct SmbTime
    {
        private ushort time;

        /// <summary>
        /// The hours. Values range from 0 to 23.
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The hour should be in range of [0, 23].</exception>
        public ushort Hour
        {
            get
            {
                return (ushort)(((this.time) & (ushort)SmbTimeBitMask.HOUR) >> 11);
            }
            set
            {
                if (value < 0 || value > 23)
                {
                    throw new ArgumentOutOfRangeException("value",
                        "Failed in Set_Hour_Property of SmbTime: The hour should be in range of [0, 23].");
                }
                // Hour occupys the high 11 bits of time, Minutes occupys the next 5 bits,
                // the left bits are belong to Seconds
                this.time = (ushort)((value << 11) + (this.Minutes << 5) + (this.Seconds / 2));
            }
        }


        /// <summary>
        /// The minutes. Values range from 0 to 59
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The Minutes should be in range of [0, 59].</exception>
        public ushort Minutes
        {
            get
            {
                return (ushort)((this.time & (ushort)SmbTimeBitMask.MINUTES) >> 5);
            }
            set
            {
                if (value < 0 || value > 59)
                {
                    throw new ArgumentOutOfRangeException("value",
                        "Failed in Set_Minutes_Property of SmbTime: The Minutes should be in range of [0, 59].");
                }
                // Hour occupys the high 11 bits of time, Minutes occupys the next 5 bits,
                // the left bits are belong to Seconds
                this.time = (ushort)((this.Hour << 11) + (value << 5) + (this.Seconds / 2));
            }
        }


        /// <summary>
        /// The seconds. Values MUST represent two-second increments
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The Seconds should be in range of [0, 59].</exception>
        public ushort Seconds
        {
            get
            {
                return (ushort)((this.time & (ushort)SmbTimeBitMask.SECONDS) * 2);
            }
            set
            {
                if (value < 0 || value > 59)
                {
                    throw new ArgumentOutOfRangeException("value",
                        "Failed in Set_Seconds_Property of SmbTime: The Seconds should be in range of [0, 59].");
                }
                // Hour occupys the high 11 bits of time, Minutes occupys the next 5 bits,
                // the left bits are belong to Seconds
                this.time = (ushort)((this.Hour << 11) + (this.Minutes << 5) + (value / 2));
            }
        }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="hour">the hour</param>
        /// <param name="minutes">the minutes</param>
        /// <param name="seconds">the seconds</param>
        public SmbTime(ushort hour, ushort minutes, ushort seconds)
            : this()
        {
            Hour = hour;
            Minutes = minutes;
            Seconds = seconds;
        }


        /// <summary>
        /// translate itself to byte array
        /// </summary>
        /// <returns>the translated byte array</returns>
        public byte[] ToBytes()
        {
            return BitConverter.GetBytes(time);
        }
    }
}