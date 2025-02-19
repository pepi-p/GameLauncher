using System;
using System.Windows;
using Launcher.Constants;
using Launcher.Services;
using System.Windows.Input;
using Reactive.Bindings;
using System.IO;

namespace Launcher.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void UpdateButtonEnter(object sender, MouseEventArgs e)
        {
            var img = ImageProvider.GetImage(Path.Join(FilePaths.ImagePath, "Update_Highlight.png"));
            if (!img.IsSuccess) return;

            UpdateButtonImage.Source = img.Value;
        }

        public void UpdateButtonLeave(object sender, MouseEventArgs e)
        {
            var img = ImageProvider.GetImage(Path.Join(FilePaths.ImagePath, "Update.png"));
            if (!img.IsSuccess) return;

            UpdateButtonImage.Source = img.Value;
        }
    }
}
