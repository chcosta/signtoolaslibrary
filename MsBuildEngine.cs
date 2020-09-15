// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Xml.Linq;

namespace SignToolAsALibrary
{ 
    class MsBuildEngine : IBuildEngine
    {
        // It's just a test helper so public fields is fine.
        public List<BuildErrorEventArgs> LogErrorEvents = new List<BuildErrorEventArgs>();

        public List<BuildMessageEventArgs> LogMessageEvents =
            new List<BuildMessageEventArgs>();

        public List<CustomBuildEventArgs> LogCustomEvents =
            new List<CustomBuildEventArgs>();

        public List<BuildWarningEventArgs> LogWarningEvents =
            new List<BuildWarningEventArgs>();

        public readonly List<ImmutableArray<XElement>> FilesToSign = new List<ImmutableArray<XElement>>();
        private ILogger<MsBuildEngine> _output;

        public MsBuildEngine()
        {
        }
        public MsBuildEngine(ILogger<MsBuildEngine> _logger)
        {
            _output = _logger;
        }

        public bool BuildProjectFile(string projectFileName, string[] targetNames, IDictionary globalProperties, IDictionary targetOutputs)
        {
            throw new NotImplementedException();
        }

        public int ColumnNumberOfTaskNode
        {
            get { return 0; }
        }

        public bool ContinueOnError
        {
            get; set;
        }

        public int LineNumberOfTaskNode
        {
            get { return 0; }
        }

        public void LogCustomEvent(CustomBuildEventArgs e)
        {
            if (_output != null)
            {
                _output.LogInformation(e.Message ?? string.Empty);
            }

            LogCustomEvents.Add(e);
        }

        public void LogErrorEvent(BuildErrorEventArgs e)
        {
            if (_output != null)
            {
                _output.LogError($"error {e.Code}: {e.Message}");
            }

            LogErrorEvents.Add(e);
        }

        public void LogMessageEvent(BuildMessageEventArgs e)
        {
            if (_output != null)
            {
                _output.LogInformation(e.Message ?? string.Empty);
            }

            LogMessageEvents.Add(e);
        }

        public void LogWarningEvent(BuildWarningEventArgs e)
        {
            if (_output != null)
            {
                _output.LogWarning($"warning {e.Code}: {e.Message}");
            }

            LogWarningEvents.Add(e);
        }

        public string ProjectFileOfTaskNode
        {
            get { return "fake ProjectFileOfTaskNode"; }
        }

    }
}
