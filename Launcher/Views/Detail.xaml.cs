using Launcher.Constants;
using Launcher.Services;
using System.IO;
using System.Windows.Controls;
using System.Windows.Input;

namespace Launcher.Views
{
    /// <summary>
    /// Detail.xaml の相互作用ロジック
    /// </summary>
    public partial class Detail : UserControl
    {
        public Detail()
        {
            InitializeComponent();
        }

        public void PlayButtonEnter(object sender, MouseEventArgs e)
        {
            var img = ImageProvider.GetImage(Path.Join(FilePaths.ImagePath, "Play_Highlight.png"));
            if (!img.IsSuccess) return;

            PlayButtonImage.Source = img.Value;
        }

        public void PlayButtonLeave(object sender, MouseEventArgs e)
        {
            var img = ImageProvider.GetImage(Path.Join(FilePaths.ImagePath, "Play.png"));
            if (!img.IsSuccess) return;

            PlayButtonImage.Source = img.Value;
        }

        public void ExitButtonEnter(object sender, MouseEventArgs e)
        {
            var img = ImageProvider.GetImage(Path.Join(FilePaths.ImagePath, "Exit_Highlight.png"));
            if (!img.IsSuccess) return;

            ExitButtonImage.Source = img.Value;
        }

        public void ExitButtonLeave(object sender, MouseEventArgs e)
        {
            var img = ImageProvider.GetImage(Path.Join(FilePaths.ImagePath, "Exit.png"));
            if (!img.IsSuccess) return;

            ExitButtonImage.Source = img.Value;
        }
    }
}
