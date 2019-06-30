using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using CodeDek.Lib.FortuneCookie;
using CodeDek.Lib.Mvvm;

namespace ReparsePointCreator
{
    public sealed class MainViewModel : ObservableObject
    {
        private string _status;
        private string _passage;
        private string _passageUrl;
        private Brush _statusColor = Brushes.DarkSeaGreen;

        public MainViewModel()
        {
            var (text, passage, url) = CookieGenerator.GenerateCookie(CookieType.Promises);
            Status = text;
            Passage = passage;
            PassageUrl = url;
        }

        public CreateReparsePointViewModel CreateReparsePointViewModel => new CreateReparsePointViewModel(this);
        public AboutViewModel AboutViewModel => new AboutViewModel();

        public Cmd GoToUrlCmd => new Cmd(() => Process.Start(PassageUrl));

        public string Title => "CodeDek's Reparse Point Creator";
        public int MinHeight => 600;
        public int MinWidth => 800;

        public string Status
        {
            get => _status;
            set => Set(ref _status, value);
        }

        public string Passage
        {
            get => _passage;
            set => Set(ref _passage, value);
        }

        public string PassageUrl
        {
            get => _passageUrl;
            set => Set(ref _passageUrl, value);
        }

        public Brush StatusColor
        {
            get => _statusColor;
            set => Set(ref _statusColor, value);
        }
    }
}
