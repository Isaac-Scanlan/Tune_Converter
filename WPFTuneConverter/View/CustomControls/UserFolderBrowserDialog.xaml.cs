using Microsoft.WindowsAPICodePack.Dialogs;
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
using System.Windows.Navigation;
using System.Windows.Shapes;



namespace WPFTuneConverter.View.CustomControls
{
    /// <summary>
    /// Interaction logic for UserFolderBrowserDialog.xaml
    /// </summary>
    public partial class UserFolderBrowserDialog : UserControl
    {

        public UserFolderBrowserDialog()
        {
            InitializeComponent();
        }

        private string placeHolder;

        public string PlaceHolder
        {
            get { return placeHolder; }
            set
            {
                placeHolder = value;
                textInput.Text = placeHolder;
            }
        }

        //protected void OnPropertyChanged(string name)
        //{
        //    PropertyChangedEventHandler handler = PropertyChanged;
        //    if (handler != null)
        //    {
        //        handler(this, new PropertyChangedEventArgs(name));
        //    }
        //}

        private void buttonClear_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true,
                InitialDirectory = @"" + textInput.Text
            };

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                textInput.Text = dialog.FileName;
            }

        }

        //private void textInput_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    if (string.IsNullOrEmpty(textInput.Text))
        //    {
        //        tbPlaceHolder.Visibility = Visibility.Visible;
        //    }
        //    else
        //    {
        //        tbPlaceHolder.Visibility = Visibility.Hidden;
        //    }
        //}


        private void buttonClear_MouseEnter(object sender, MouseEventArgs e)
        {

            xSVG.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00176b"));
            xSVG.Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00176b"));
        }

        private void buttonClear_MouseLeave(object sender, MouseEventArgs e)
        {
            xSVG.Fill = new SolidColorBrush(Colors.White);
            xSVG.Stroke = new SolidColorBrush(Colors.White);
        }
    }
}
