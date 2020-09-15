// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SignToolAsLibrary
{
    public class ReleaseLogger : ILogger, ILoggerProvider
    {
        private ILogger _consoleLogger = null;

        public List<LoggingRecord> Logs = new List<LoggingRecord>();
        public List<LoggingRecord> GroupedErrors = new List<LoggingRecord>();
        public List<LoggingRecord> GroupedWarnings = new List<LoggingRecord>();

        public void Dispose()
        {
        }

        public ILogger CreateLogger(string categoryName)
        {
            return this;
        }

        public void SetConsoleLogger(ILogger consoleLogger)
        {
            _consoleLogger = consoleLogger;
        }

        public void StageWarning(EventId eventId, Exception exception, string message)
        {
            LoggingRecord record = new LoggingRecord(LogLevel.Warning, eventId, exception, message);
            Logs.Add(record);
            GroupedWarnings.Add(record);
        }

        public void StageError(EventId eventId, Exception exception, string message)
        {
            LoggingRecord record = new LoggingRecord(LogLevel.Error, eventId, exception, message);
            Logs.Add(record);
            GroupedErrors.Add(record);
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            Logs.Add(new LoggingRecord(logLevel, eventId, exception, formatter(state, exception)));
            if (_consoleLogger != null)
            {
                _consoleLogger.Log(logLevel, eventId, exception, formatter(state, exception));
            }
        }

        public void LogGroupedWarnings()
        {
            if (_consoleLogger == null)
            {
                return;
            }

            foreach (LoggingRecord warning in GroupedWarnings)
            {
                _consoleLogger.LogWarning(warning.EventId, warning.Exception, warning.Message);
            }
        }

        public void LogGroupedErrors()
        {
            if (_consoleLogger == null)
            {
                return;
            }

            foreach (LoggingRecord error in GroupedErrors)
            {
                _consoleLogger.LogError(error.EventId, error.Exception, error.Message);
            }
        }

        public bool ContainsError()
        {
            return Logs.Any(record => record.LogLevel == LogLevel.Error);
        }

        public bool ContainsWarning()
        {
            return Logs.Any(record => record.LogLevel == LogLevel.Warning);
        }

        public bool ContainsEventId(EventId eventId)
        {
            return Logs.Any(record => record.EventId == eventId);
        }

        public bool ContainsText(string text)
        {
            return Logs.Any(record => record.Message.Contains(text));
        }

        public bool ContainsException<TException>()
        {
            return Logs.Any(record => record.Exception is TException);
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return this;
        }

        public void RemoveEntriesWithEventId(int eventId)
        {
            for (int i = 0; i < Logs.Count; i++)
            {
                if (Logs[i].EventId == eventId)
                {
                    Logs.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}
