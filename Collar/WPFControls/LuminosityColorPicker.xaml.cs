using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Collar.WPFControls
{
    /// <summary>
    /// Interaction logic for HueColorPicker.xaml
    /// </summary>
    public partial class LuminosityColorPicker : UserControl
    {       
        private GradientStopCollection colors = new GradientStopCollection()
        {
            new GradientStop(Color.FromRgb(255,255,255),0),            
            new GradientStop(Color.FromRgb(255,0,0),0.5),
            new GradientStop(Color.FromRgb(0,0,0),1)
        };
        private GradientDistribution gd = GradientDistribution.Adjust;
        public GradientDistribution GradientDistribution
        {
            get => gd;
            set
            {
                gd = value;
                Border.Background = GradientDistributionSetter.Create(colors, new System.Windows.Size(Width, Height), gd);
            }
        }

        private Color specl;

        public Color InputColor
        {
            get => specl;
            set
            {
                specl = value;
                colors = new GradientStopCollection()
                {
                    new GradientStop(Color.FromRgb(255,255,255),0),
                    new GradientStop(specl,0.5),
                    new GradientStop(Color.FromRgb(0,0,0),1)
                };
                Border.Background = GradientDistributionSetter.Create(colors, new Size(Width, Height), gd);
                OnInputColorChanged(this, null);
            }
        }
        public LuminosityColorPicker()
        {
            InitializeComponent();
        }

        private CornerRadius cr;
        private CornerRadiusStyle cs = CornerRadiusStyle.Default;
        public CornerRadius CornerRadius
        {
            get => cr;
            set
            {
                cr = value;
                Border.CornerRadius = CornerRadiusSetter.Create(cr, new System.Windows.Size(Width, Height), cs);
            }
        }

        public CornerRadiusStyle CornerRadiusStyle { get => cs; set { cs = value; CornerRadius = CornerRadius; } }

        private void Border_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            CornerRadius = CornerRadius;
            GradientDistribution = GradientDistribution;
        }

        private bool msdown = false;
        private RenderTargetBitmap bmp;

        private Color sc;
        private Point sp;

        public Point SelectedPoint
        {
            get => sp;
            set
            {
                sp = value;
                if (!new System.Drawing.RectangleF(0, 0, (float)Width - 1, (float)Height - 1).Contains((float)sp.X, (float)sp.Y)) return;
                Point pos = Mouse.GetPosition(Border);
                PresentationSource source = PresentationSource.FromVisual(Border);
                double dpiX = 96.0 * source.CompositionTarget.TransformToDevice.M11,
                       dpiY = 96.0 * source.CompositionTarget.TransformToDevice.M22;

                bmp = new RenderTargetBitmap((int)Width, (int)Height, dpiX, dpiY, PixelFormats.Pbgra32);
                DrawingVisual drawingVisual = new DrawingVisual();
                using (DrawingContext drawingContext = drawingVisual.RenderOpen())
                {
                    drawingContext.DrawRectangle(Border.Background, null,
                      new Rect(new Point(), new Size(Border.Width, Border.Height)));
                }
                bmp.Render(drawingVisual);
                CroppedBitmap cb = new CroppedBitmap(bmp, new Int32Rect((int)sp.X, (int)sp.Y, 1, 1));
                byte[] pixel = new byte[cb.Format.BitsPerPixel / 8];
                cb.CopyPixels(pixel, cb.Format.BitsPerPixel / 8, 0);
                sc = Color.FromArgb(255, pixel[2], pixel[1], pixel[0]);
                OnSelectedColorChanged(this, null);
            }
        }


        public Color SelectedColor { get => sc; }

        private void Border_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            msdown = true;
        }

        private void Border_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            msdown = false;
        }


        private void Border_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (!msdown) return;
            SelectedPoint = Mouse.GetPosition(Border);
        }

        public event EventHandler SelectedColorChanged;

        protected virtual void OnSelectedColorChanged(object sender, EventArgs e)
        {
            SelectedColorChanged?.Invoke(sender, e);
        }

        public event EventHandler InputColorChanged;


        protected virtual void OnInputColorChanged(object sender, EventArgs e)
        {
            InputColorChanged?.Invoke(sender, e);
            SelectedPoint = SelectedPoint;
        }

        private void Border_MouseLeave(object sender, MouseEventArgs e)
        {
            msdown = false;
        }

        private void Border_Loaded(object sender, RoutedEventArgs e)
        {
            SelectedPoint = new Point(0, 0);
        }

        public Brush BorderStyle { get => Border.BorderBrush; set => Border.BorderBrush = value; }
        public Thickness BorderLineThickness { get => Border.BorderThickness; set => Border.BorderThickness = value; }
    }
}
