using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WPFTuneConverter.View.Windows
{
    /// <summary>
    /// Interaction logic for TunePageWindow.xaml
    /// </summary>
    public partial class TunePageWindow : Window
    {
        private bool _multiPage = false;
        private int _currentPage = 1;
        private int _maxPage = 1;
        public string directoryString {  get; set; } = string.Empty;

        public TunePageWindow(bool multiPage, int maxPageNumber = 1)
        {
            InitializeComponent();
            _multiPage = multiPage;
            _maxPage = maxPageNumber;


            pageCountLabel.Content = _currentPage + " out of " + _maxPage;
 
        }

        private void nextPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_multiPage || _currentPage >= _maxPage)
            {
                return;
            }

            _currentPage++;
            pageCountLabel.Content = _currentPage + " out of " + _maxPage;

            var bmpTemp = new BitmapImage();
            var file1 = directoryString + "_page_" + _currentPage + ".png";
            bmpTemp.BeginInit();
            bmpTemp.CacheOption = BitmapCacheOption.OnLoad;
            bmpTemp.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            bmpTemp.UriSource = new Uri(file1);
            bmpTemp.EndInit();

            TuneImage.Opacity = 1;
            TuneImage.Source = bmpTemp;

        }

        private void previousPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_multiPage || _currentPage <= 1)
            {
                return;
            }

            _currentPage--;
            pageCountLabel.Content = _currentPage + " out of " + _maxPage;

            var bmpTemp = new BitmapImage();
            var file1 = directoryString + "_page_" + _currentPage + ".png";
            bmpTemp.BeginInit();
            bmpTemp.CacheOption = BitmapCacheOption.OnLoad;
            bmpTemp.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            bmpTemp.UriSource = new Uri(file1);
            bmpTemp.EndInit();

            TuneImage.Opacity = 1;
            TuneImage.Source = bmpTemp;


        }
    }
}
