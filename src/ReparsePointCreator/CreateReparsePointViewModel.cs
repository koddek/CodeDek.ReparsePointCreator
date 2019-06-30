using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CodeDek.Lib;
using CodeDek.Lib.Mvvm;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace ReparsePointCreator
{
    public sealed class CreateReparsePointViewModel : ObservableObject
    {
        private string _source;
        private string _terget;
        private int _option;
        private bool _overwrite;
        private bool _isEmptyOption = true;
        private string _sourceMessage;
        private string _targetMessage;
        private bool _tergetExists;
        private readonly MainViewModel _mainViewModel;
        private readonly string[] _options = new[]
        {
            "",
            nameof(FileSymbolicLink),
            nameof(FileHardLink),
            nameof(DirectorySymbolicLink),
            nameof(DirectoryJunction),
            nameof(MKLINKFileSymbolicLink),
            nameof(MKLINKFileHardLink),
            nameof(MKLINKDirectorySymbolicLink),
            nameof(MKLINKDirectoryJunction)
        };

        public CreateReparsePointViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
        }

        public string FileSymbolicLink => "File Symbolic Link (requires admin rights)*";
        public string FileHardLink => "File Hard Link";
        public string DirectorySymbolicLink => "Directory Symbolic Link (requires admin rights)*";
        public string DirectoryJunction => "Directory Junction";
        public string MKLINKFileSymbolicLink => "MKLINK File Symbolic Link";
        public string MKLINKFileHardLink => "MKLINK File Hard Link";
        public string MKLINKDirectorySymbolicLink => "MKLINK Directory Symbolic Link";
        public string MKLINKDirectoryJunction => "MKLINK Directory Junction";

        public string Source
        {
            get => _source;
            set => Set(ref _source, value)
                .Alert(nameof(CreateLinkCmd))
                .Alert(nameof(ResetFormCmd));
        }
        public string SourceMessage
        {
            get => _sourceMessage;
            set => Set(ref _sourceMessage, value);
        }

        public string Target
        {
            get => _terget;
            set => Set(ref _terget, value, () =>
            {
                if (Directory.Exists(Target) || File.Exists(Target))
                {
                    TargetExists = true;
                    TargetMessage = "Warning! This is an existing target path. Check overwrite to successfully create this link.";
                }
                else
                {
                    TargetExists = false;
                    TargetMessage = "";
                }
            })
                .Alert(nameof(CreateLinkCmd))
                .Alert(nameof(ResetFormCmd));
        }

        public bool TargetExists
        {
            get => _tergetExists;
            set => Set(ref _tergetExists, value);
        }

        public string TargetMessage
        {
            get => _targetMessage;
            set => Set(ref _targetMessage, value);
        }

        public int Option
        {
            get => _option;
            set => Set(ref _option, value)
                .Alert(nameof(CreateLinkCmd))
                .Alert(nameof(ResetFormCmd));
        }

        public bool Overwrite
        {
            get => _overwrite;
            set
            {
                Set(ref _overwrite, value, () => TargetExists = !value)
                .Alert(nameof(CreateLinkCmd))
               .Alert(nameof(ResetFormCmd));
            }
        }

        public bool IsEmptyOption
        {
            get => _isEmptyOption;
            set => Set(ref _isEmptyOption, value);
        }

        public Cmd SelectSourcePathCmd => new Cmd(() =>
        {
            if (!CanSelect())
                return;

            var result = SelectPath();
            if (!string.IsNullOrWhiteSpace(result))
            {
                Source = result;
                var ext = Path.GetExtension(result);
                var name = Path.GetFileNameWithoutExtension(result);
                var dir = Path.GetDirectoryName(result);

                if (!Path.HasExtension(result))
                    Target = $"{result}Link";
                else
                    Target = Path.Combine(Path.GetDirectoryName(result), $"{Path.GetFileNameWithoutExtension(result)}Link{Path.GetExtension(result)}");

                if (Storage.IsReparsePoint(result))
                    SourceMessage = "Warning! The selected source is a reparse point.";
            }

        });

        public Cmd SelectTargetPathCmd => new Cmd(() =>
        {
            if (!CanSelect())
                return;

            var result = SelectPath();
            if (!string.IsNullOrWhiteSpace(result))
                Target = result;
        });

        private bool CanSelect()
        {
            if (IsEmptyOption)
            {
                MessageBox.Show("You must first select the type of link you'd like to create.", "Missed a step..", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }
            return true;
        }

        private string SelectPath()
        {
            var dlg = new CommonOpenFileDialog();
            dlg.Title = "Select source file or folder.";
            dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            dlg.EnsureValidNames = true;
            dlg.ShowPlacesList = true;

            switch (Option)
            {
                case 1:
                case 2:
                case 5:
                case 6:
                    dlg.IsFolderPicker = false;
                    break;
                case 3:
                case 4:
                case 7:
                case 8:
                    dlg.IsFolderPicker = true;
                    break;
            }


            if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
            {
                return dlg.FileName;
            }
            return "";
        }

        public Cmd ResetFormCmd => new Cmd(() =>
        {
            IsEmptyOption = true;
            Option = 0;
            Source = "";
            Target = "";
            Overwrite = false;
            SourceMessage = "";
            TargetMessage = "";
            TargetExists = false;
            _mainViewModel.Status = "Form was reset.";
        }, () => Option > 0 || !string.IsNullOrWhiteSpace(Source) || !string.IsNullOrWhiteSpace(Target) || Overwrite);

        public Cmd CreateLinkCmd => new Cmd(() =>
        {
            switch (Option)
            {
                case 1:
                    Storage.CreateFileSymbolicLink(Source, Target, Overwrite);
                    break;
                case 2:
                    Storage.CreateFileHardLink(Source, Target, Overwrite);
                    break;
                case 3:
                    Storage.CreateDirectorySymbolicLink(Source, Target, Overwrite);
                    break;
                case 4:
                    Storage.CreateDirectoryJunction(Source, Target, Overwrite);
                    break;
                case 5:
                    Storage.CreateMkLinkFileSymbolicLink(Source, Target, Overwrite);
                    break;
                case 6:
                    Storage.CreateMkLinkFileHardLink(Source, Target, Overwrite);
                    break;
                case 7:
                    Storage.CreateMkLinkDirectorySymbolicLink(Source, Target, Overwrite);
                    break;
                case 8:
                    Storage.CreateMkLinkDirectoryJunction(Source, Target, Overwrite);
                    break;
                default:
                    return;
            }
            _mainViewModel.Status = "Link creation complete.";

        }, () => Option > 0 && !string.IsNullOrWhiteSpace(Source) && !string.IsNullOrWhiteSpace(Target) && !TargetExists);

        public Cmd Option1Cmd => new Cmd(() => Option = 1);
        public Cmd Option2Cmd => new Cmd(() => Option = 2);
        public Cmd Option3Cmd => new Cmd(() => Option = 3);
        public Cmd Option4Cmd => new Cmd(() => Option = 4);
        public Cmd Option5Cmd => new Cmd(() => Option = 5);
        public Cmd Option6Cmd => new Cmd(() => Option = 6);
        public Cmd Option7Cmd => new Cmd(() => Option = 7);
        public Cmd Option8Cmd => new Cmd(() => Option = 8);
    }
}
