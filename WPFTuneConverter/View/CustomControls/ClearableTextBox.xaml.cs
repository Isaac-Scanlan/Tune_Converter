using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using TuneConverter.Framework.PageImageIO.ImageComponents;

namespace WPFTuneConverter.View.CustomControls
{
    /// <summary>
    /// Interaction logic for ClearableTextBox.xaml
    /// </summary>
    public partial class ClearableTextBox : UserControl
    {
        public ClearableTextBox()
        {
            InitializeComponent();
        }

        private string placeHolder;

        public string PlaceHolder
        {
            get { return placeHolder; }
            set {
                placeHolder = value;
                tbPlaceHolder.Text = placeHolder;
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
            textInput.Clear();
            textInput.Focus();
            
        }

        private void textInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(string.IsNullOrEmpty(textInput.Text))
            {
                tbPlaceHolder.Visibility = Visibility.Visible;
            }
            else
            {
                tbPlaceHolder.Visibility = Visibility.Hidden;
            }
        }


        private void buttonClear_MouseEnter(object sender, MouseEventArgs e)
        {

            xSVG.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00b38c"));
            xSVG.Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00b38c"));
        }

        private void buttonClear_MouseLeave(object sender, MouseEventArgs e)
        {
            xSVG.Fill = new SolidColorBrush(Colors.White);
            xSVG.Stroke = new SolidColorBrush(Colors.White);
        }
    }
}
