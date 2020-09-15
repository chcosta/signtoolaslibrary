// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace SignToolAsLibrary
{
    public class LoggingRecord
    {
        public LogLevel LogLevel { get; }
        public EventId EventId { get; }
        public Exception Exception { get; }
        public string Message { get; }


        public LoggingRecord(LogLevel logLevel, EventId eventId, Exception exception, string message)
        {
            LogLevel = logLevel;
            EventId = eventId;
            Exception = exception;
            Message = message;
        }
    }
}
