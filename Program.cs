using System;
using Microsoft.Extensions.Logging;
using Microsoft.DotNet.SignTool;
using Microsoft.Build.Construction;
using SignToolAsLibrary;
using Microsoft.Build.Utilities;
using System.Collections.Generic;
using Microsoft.Build.Framework;

namespace SignToolAsALibrary
{
    class Program
    {
        static void Main(string[] args)
        {
            string dropFolder = "{dropFolder}";
            List<ITaskItem> strongNameSignInfo = new List<ITaskItem>()
            {
                new TaskItem("MsSharedLib72",
                                new Dictionary<string, string> {
                                    {"PublicKeyToken", "31bf3856ad364e35"},
                                    {"CertificateName", "Microsoft400" }
                                })
            };
            List<ITaskItem> fileExtensionSignInfo = new List<ITaskItem>()
            {
                new TaskItem(".exe", new Dictionary<string, string> { { "CertificateName", "Microsoft400" } }),
                new TaskItem(".dll", new Dictionary<string, string> { { "CertificateName", "Microsoft400" } }),
                new TaskItem(".msi", new Dictionary<string, string> { { "CertificateName", "Microsoft400" } }),
                new TaskItem(".zip", new Dictionary<string, string> { { "CertificateName", "None" } }),
                new TaskItem(".nupkg", new Dictionary<string, string> { { "CertificateName", "NuGet" } })
            };

            var task = new SignToolTask {
                BuildEngine = new MsBuildEngine(/* pass release logger */),
                MicroBuildCorePath = "E:\\gh\\chcosta\\arcade\\.packages\\microbuild.core\\0.2.0",
                MSBuildPath = "C:\\Program Files (x86)\\Microsoft Visual Studio\\2019\\Preview\\MSBuild\\Current\\Bin\\MSBuild.exe",
                SNBinaryPath = "C:\\Program Files (x86)\\Microsoft SDKs\\Windows\\v10.0A\\bin\\NETFX 4.8 Tools\\sn.exe",
                AllowEmptySignList = true,
                TestSign = true,
                DryRun = false,
                DoStrongNameCheck = true,
                TempDir = "e:\\chcosta\\sometempdir",
                WixToolsPath = "E:\\gh\\chcosta\\arcade\\.packages\\wix\\3.11.1\\tools",
                LogDir ="e:\\chcosta\\temp",
                ItemsToSign = new string[] { $"{dropFolder}windowsdesktop-runtime-5.0.0-rc.2.20464.2-win-x64.exe",
                                             $"{dropFolder}windowsdesktop-runtime-5.0.0-rc.2.20464.2-win-x64.exe.wixpack.zip",
                                             $"{dropFolder}windowsdesktop-runtime-5.0.0-rc.2.20464.2-win-x64.msi",
                                             $"{dropFolder}windowsdesktop-runtime-5.0.0-rc.2.20464.2-win-x64.msi.wixpack.zip",
                                             $"{dropFolder}VS.Redist.Common.WindowsDesktop.SharedFramework.x64.5.0.5.0.0-rc.2.20464.2.nupkg"},
                StrongNameSignInfo = strongNameSignInfo.ToArray(),
                FileExtensionSignInfo = fileExtensionSignInfo.ToArray()
            };

            task.Execute();
        }
    }

}
