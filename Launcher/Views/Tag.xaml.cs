using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Launcher.Views
{
    /// <summary>
    /// Tag.xaml の相互作用ロジック
    /// </summary>
    public partial class Tag : UserControl
    {
        public Tag()
        {
            InitializeComponent();
        }

        public void TagTextSizeChanged(object sender, SizeChangedEventArgs e)
        {
            Width = GetTextWidth(TagText) + 30;
        }

        private double GetTextWidth(TextBlock textBlock)
        {
            var formattedText = new FormattedText(
                        textBlock.Text,
                        System.Globalization.CultureInfo.CurrentCulture,
                        FlowDirection.LeftToRight,
                        new Typeface(textBlock.FontFamily, textBlock.FontStyle, textBlock.FontWeight, textBlock.FontStretch),
                        textBlock.FontSize,
                        Brushes.Green,
                        VisualTreeHelper.GetDpi(this).PixelsPerDip
                    );

            return formattedText.Width;
        }
    }
}
