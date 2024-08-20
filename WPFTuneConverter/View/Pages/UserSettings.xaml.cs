using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFTuneConverter.View.Pages
{
    /// <summary>
    /// Interaction logic for UserSettings.xaml
    /// </summary>
    public partial class UserSettings : Page
    {
        string defaultExportDirectory = //System.IO.Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile).ToString() + "\\Tune Scribe Exports"; //);

        public string outImageFilePath = System.IO.Path.GetFullPath(System.IO.Path.Combine(Directory.GetCurrentDirectory(),
            System.IO.Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "..", "TuneConverter.Framework.PageImageIO", "OutImages")));

        public UserSettings()
        {
            InitializeComponent();
            outputFolder.textInput.Text = outImageFilePath;
            exportFolder.textInput.Text = defaultExportDirectory;
            Directory.CreateDirectory(defaultExportDirectory);
        }

        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            outputFolder.textInput.Text = outImageFilePath;
            exportFolder.textInput.Text = defaultExportDirectory;
        }

        private async void convertButton_Click(object sender, RoutedEventArgs e)
        {
            saveDisplay.Opacity = 1;
            int mult = 9;

            await Task.Delay(700);

            while (saveDisplay.Opacity > 0)
            {
                saveDisplay.Opacity -= (0.005 * mult);
                await Task.Delay(1);
            }

            
        }
    }
}
