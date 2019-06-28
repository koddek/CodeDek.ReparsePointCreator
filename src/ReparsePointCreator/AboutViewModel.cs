using System;
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
        public byte[] AppIcon => File.ReadAllBytes(@"D:\.repo\_product\CodeDek.FolderSharer\art\ic_folder_sharer.ico");
        public string Home => "https://github.com/codedek/CodeDek.FolderSharer";
        public string Download => "https://github.com/codedek/CodeDek.FolderSharer/releases";
        public string Issues => "https://github.com/codedek/CodeDek.FolderSharer/issues";
        public string License => "https://github.com/codedek/CodeDek.FolderSharer/blob/master/LICENSE";
        public string Changelog => "https://github.com/codedek/CodeDek.FolderSharer/blob/master/CHANGELOG.md";
        public string AppName => "Folder Sharer";
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
