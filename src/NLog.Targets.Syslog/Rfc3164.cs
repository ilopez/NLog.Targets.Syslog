﻿using NLog.Config;
using NLog.Layouts;
using System;
using System.Globalization;
using System.Text;

// ReSharper disable CheckNamespace
namespace NLog.Targets
// ReSharper restore CheckNamespace
{
    /// <summary>Allows to build Syslog messages comliant with RFC 3164</summary>
    [NLogConfigurationItem]
    public class Rfc3164 : MessageBuilder
    {
        private const string TimestampFormat = "{0:MMM} {0,11:d HH:mm:ss}";
        private Layout MachineName { get; }
        private Layout Sender { get; }

        /// <summary>Initializes a new instance of the Rfc3164 class</summary>
        public Rfc3164(Layout sender, Layout machineName)
        {
            Sender = sender;
            MachineName = machineName;
        }

        /// <summary>Builds the Syslog message according to RFC 3164</summary>
        /// <param name="logEvent">The NLog.LogEventInfo</param>
        /// <param name="pri">The Syslog PRI part</param>
        /// <param name="logEntry">The entry to be logged</param>
        /// <returns>Byte array containing the Syslog message</returns>
        public override byte[] BuildMessage(LogEventInfo logEvent, string pri, string logEntry)
        {
            var header = Header(logEvent);
            var msg = Msg(logEvent, logEntry);

            var syslogMessage = $"{pri}{header} {msg}";

            return Encoding.ASCII.GetBytes(syslogMessage);
        }

        private string Header(LogEventInfo logEvent)
        {
            var timestamp = string.Format(CultureInfo.InvariantCulture, TimestampFormat, logEvent.TimeStamp);
            var hostname = MachineName.Render(logEvent);
            var header = $"{timestamp} {hostname}";
            return header;
        }

        private string Msg(LogEventInfo logEvent, string logEntry)
        {
            var tag = Sender.Render(logEvent);
            var content = Char.IsLetterOrDigit(logEntry[0]) ? " {logEntry}" : logEntry;
            var msg = $"{tag}{content}";
            return msg;
        }
    }
}