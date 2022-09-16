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
using System.Windows.Threading;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Collar.WPFControls
{
    /// <summary>
    /// Interaction logic for ToggleSwitch.xaml
    /// </summary>
    public partial class ToggleSwitch : UserControl
    {
        private DispatcherTimer AnimationClock = new DispatcherTimer();
        private Brush bg, bgh, fg, fgh;
        private int spd = 20;
        private bool val = false, rtl = false, msover = false;
        public bool Animated { get; set; }
        public int AnimationSpeed
        {
            get => spd;
            set
            {
                if (value < 1) spd = 1;
                else if (value > 100) spd = 100;
                else spd = value;
            }
        }

        public bool Checked
        {
            get => val;
            set
            {
                val = value;
                if (!AnimationClock.IsEnabled)
                    AnimationClock.Start();
                bg = val ? InputBackgroundChecked : InputBackground;
                bgh = val ? InputBackgroundCheckedHover : InputBackgroundHover;
                fg = val ? SliderBackgroundChecked : SliderBackground;
                fgh = val ? SliderBackgroundCheckedHover : SliderBackgroundHover;
                Border.Background = msover ? bgh : bg;
                Slider.Fill = msover ? fgh : fg;
                OnCheckStateChanged(this, null);
            }
        }

        public bool RightToLeft
        {
            get => rtl;
            set
            {
                rtl = value;
                Checked = Checked;
            }
        }       
        public ToggleSwitch()
        {
            InitializeComponent();
            Border.Background = Background;
            AnimationClock.Interval = new TimeSpan(0, 0, 0, 0, 1);
            AnimationClock.Tick += AnimationClock_Tick;
            Checked = false;
            Animated = true;
        }

        private void AnimationClock_Tick(object sender, EventArgs e)
        {
            double x = Canvas.GetLeft(Slider);
            double x1 = 10, x2 = 210, s = spd, g = 1, tmp;
            if (rtl)
            {
                tmp = x1; x1 = x2; x2 = tmp;
                g = -1;
            }

            if (!Animated)
            {
                Canvas.SetLeft(Slider, val ? x2 : x1);
                AnimationClock.Stop();
                return;
            }
            if (val)
            {
                if (g * (x - x2) < 0) Canvas.SetLeft(Slider, x + g * s);
                else
                {
                    Canvas.SetLeft(Slider, x2);
                    AnimationClock.Stop();
                }
            }
            else
            {
                if (g * (x - x1) > 0) Canvas.SetLeft(Slider, x - g * s);
                else
                {
                    Canvas.SetLeft(Slider, x1);
                    AnimationClock.Stop();
                }
            }
        }

        public double Width { get => base.Width;}
        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            base.Width = 2 * e.NewSize.Height;
        }

        private void Border_MouseEnter(object sender, MouseEventArgs e)
        {
            msover = true;
            Border.Background = bgh;
        }

        private void Border_MouseLeave(object sender, MouseEventArgs e)
        {
            msover = false;
            Border.Background = bg;
        }      
        public double CornerRadius
        {
            get => Slider.RadiusX;
            set
            {
                if (0 <= value && value <= 100)
                {
                    Slider.RadiusX = Slider.RadiusY = value;
                    Border.CornerRadius = new CornerRadius(value);
                }
            }
        }

        public Brush InputBackground { get; set; } = new SolidColorBrush(Color.FromRgb(230, 230, 230));
        public Brush InputBackgroundChecked { get; set; } = new SolidColorBrush(Color.FromRgb(100, 100, 230));
        public Brush InputBackgroundHover { get; set; } = new SolidColorBrush(Color.FromRgb(200, 200, 200));
        public Brush InputBackgroundCheckedHover { get; set; } = new SolidColorBrush(Color.FromRgb(100, 100, 200));
        public Brush SliderBackground { get; set; } = new SolidColorBrush(Color.FromRgb(255, 255, 255));
        public Brush SliderBackgroundChecked { get; set; } = new SolidColorBrush(Color.FromRgb(255, 255, 255));
        public Brush SliderBackgroundHover { get; set; } = new SolidColorBrush(Color.FromRgb(240, 240, 240));
        public Brush SliderBackgroundCheckedHover { get; set; } = new SolidColorBrush(Color.FromRgb(240, 240, 240));

        private void Slider_MouseEnter(object sender, MouseEventArgs e)
        {
            Slider.Fill = fgh;
        }

        private void Slider_MouseLeave(object sender, MouseEventArgs e)
        {
            Slider.Fill = fg;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Checked = Checked;
        }
        private void UserControl_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            ChangeChecked();
        }

        public event EventHandler CheckStateChanged;

        protected virtual void OnCheckStateChanged(object sender, EventArgs e)
        {
            if (IsLoaded)
                CheckStateChanged?.Invoke(sender, e);
        }

        public void ChangeChecked()
        {
            Checked = !Checked;
        }       

        public new void UpdateLayout()
        {
            Checked = Checked;
        }

        public Brush BorderStyle { get => Border.BorderBrush; set { Border.BorderBrush = value; Border.UpdateLayout(); } }
        public Thickness BorderLineThickness { get => Border.BorderThickness; set { Border.BorderThickness = value; Border.UpdateLayout(); } }
    }
}
