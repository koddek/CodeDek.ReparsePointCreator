﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using CodeDek.Lib.Mvvm;

namespace ReparsePointCreator
{
    public sealed class AboutViewModel
    {
        public byte[] AppIcon => File.ReadAllBytes(@"D:\.repo\_product\CodeDek.ReparsePointCreator\art\ic_reparse_point_creator.ico");
        public string Home => "https://github.com/codedek/codedek.reparsepointcreator";
        public string Download => "https://github.com/codedek/codedek.reparsepointcreator/releases";
        public string Issues => "https://github.com/codedek/codedek.reparsepointcreator/issues";
        public string License => "https://github.com/codedek/codedek.reparsepointcreator/blob/master/LICENSE";
        public string Changelog => "https://github.com/codedek/codedek.reparsepointcreator/blob/master/CHANGELOG.md";
        public string AppName => "Reparse Point Creator";
        public string AppVersion => $"v{FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion}";
        public string Copyright => "© 2019 CodeDek. All Rights Reserved";
        public string Developer => "Written by CodeDek";

        public Cmd NavigateHomeUrlCmd => new Cmd(() => Process.Start(Home));
        public Cmd NavigateDownloadUrlCmd => new Cmd(() => Process.Start(Download));
        public Cmd NavigateIssuesUrlCmd => new Cmd(() => Process.Start(Issues));
        public Cmd NavigateLicenseUrlCmd => new Cmd(() => Process.Start(License));
        public Cmd NavigateChangelogUrlCmd => new Cmd(() => Process.Start(Changelog));
    }
}
