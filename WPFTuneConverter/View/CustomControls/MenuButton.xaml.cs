using SharpVectors.Converters;
using SharpVectors.Renderers.Wpf;
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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace WPFTuneConverter.View.CustomControls
{
    /// <summary>
    /// Interaction logic for MenuButton.xaml
    /// </summary>
    public partial class MenuButton : UserControl
    {
        private static Dictionary<string, string> iconDictionary = new Dictionary<string, string>() {
            { "Write", SvgIcons.PenIcon},
            { "Convert", SvgIcons.ConvertIcon},
            { "Repository", SvgIcons.DatabaseIcon},
            { "Settings", SvgIcons.SettingsIcon},
            { "Teaching", SvgIcons.TeachingIcon },
            { "Help", SvgIcons.HelpIcon }
        };

        public MenuButton()
        {
            InitializeComponent();
        }

        private string buttonText;

        public string ButtonText
        {
            get { return buttonText; }
            set
            {
                buttonText = value;
                menuButtn.Content = buttonText;
            }
        }


        private string imageSource;

        private string iconName;

        public string IconName
        {
            get { return imageSource; }
            set 
            {
                iconName = value;

                //Image myImage3 = new Image();
                //BitmapImage bi3 = new BitmapImage();
                //bi3.BeginInit();


                //string url = "/View/CustomControls/" + imageSource;
                //bi3.UriSource = new Uri(url, UriKind.Relative);
                //bi3.EndInit();
                ////myImage3.Stretch = Stretch.Fill;
                //myImage3.Source = bi3;

                //svgPath = "C:/Users/Isaac/source/repos/TuneConverter/WPFTuneConverter/View/CustomControls/settings_icon.svg";

                //// 1. Create conversion options
                //WpfDrawingSettings settings = new WpfDrawingSettings();
                //settings.IncludeRuntime = false;
                //settings.TextAsGeometry = true;

                //// 2. Select a file to be converted
                //string svgTestFile = "Test.svg";

                //FileAttributes fileAttr = File.GetAttributes(svgPath);
                //File.SetAttributes(svgPath, fileAttr);

                //// 3. Create a file converter
                //FileSvgConverter converter = new FileSvgConverter(settings);
                //// 4. Perform the conversion to XAML
                //converter.Convert(svgPath);

                //path.Data = (Geometry)XamlBindingHelper.ConvertValue(typeof(Geometry), "M 0,0 L 23,0 L 31,16 L 23,31 L 0,31 Z"); ;

                ButtonSVG.Data = Geometry.Parse(iconDictionary[iconName]);

                if(iconName.Equals("Convert") || iconName.Equals("Repository") || iconName.Equals("Teaching"))
                {
                    ButtonSVG.Width = 25;
                }
                else if (iconName.Equals("Help"))
                {
                    //ButtonSVG.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ffffff"));
                    //ButtonSVG.Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00114d"));
                    ButtonSVG.StrokeThickness = 2;
                    
                }
  
                //menuButtn.Content = path;
                //ButtonSVG.Data = path;

             
            }

        }


        public event RoutedEventHandler CustomClick;

        private void OnButtonClick(object sender, RoutedEventArgs e)
        {
            if (CustomClick != null)
            {
                CustomClick(this, new RoutedEventArgs());
            }
        }

        private void menuButtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void menuButtn_IsMouseDirectlyOverChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

        }
    }
}
