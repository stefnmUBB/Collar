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
using Collar.Utils;

namespace wpftest
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

        private void ToggleSwitch_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void ToggleSwitch_CheckStateChanged(object sender, EventArgs e)
        {
            ts4.RightToLeft = ts1.Checked;
        }

        private void ts5_CheckStateChanged(object sender, EventArgs e)
        {
            ts1.Checked = !ts1.Checked;
        }

        private void ts3_CheckStateChanged(object sender, EventArgs e)
        {           
            ts5.Checked = !ts5.Checked;            
            ts4.Checked = !ts4.Checked;
        }

        private void ts6_CheckStateChanged(object sender, EventArgs e)
        {
            ts4.AnimationSpeed = ts6.Checked ?30:10;
        }

        private void HueColorPicker_SelectedColorChanged(object sender, EventArgs e)
        {            
            lum.InputColor = hue.SelectedColor;

        }      
        private void LuminosityColorPicker_SelectedColorChanged_1(object sender, EventArgs e)
        {
            ts2.InputBackgroundChecked = new SolidColorBrush(lum.SelectedColor);
            ts2.InputBackgroundCheckedHover = new SolidColorBrush(Collar.Utils.Colors.Add(lum.SelectedColor, Color.FromArgb(60, 0, 0, 0)));
            ts2.UpdateLayout();
        }

        private void lum_InputColorChanged(object sender, EventArgs e)
        {
           
        }
    }
}
