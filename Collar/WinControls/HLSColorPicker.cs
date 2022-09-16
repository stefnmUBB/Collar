using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;
using System.Windows.Forms;
using Collar;

namespace Collar.WinControls
{
    public partial class HLSColorPicker: UserControl
    {
        public event EventHandler PickedColorChanged;

        private Point CrossPosition = new Point();
        private double CrossRatioX, CrossRatioY;
        private Size OldCMSize = new Size();
        private MouseDragSystem mdsColorMap;
        private MouseDragSystem mdsBWMap;
        private MouseDragSystem mdsAlphaMap;

        private Bitmap ColorPalette=new Bitmap(360,256);
        private Bitmap BWPalette = new Bitmap(512,1);

        private Color clFromPaletteMap;
        private Color clFromBWMap;
        private Color clResult;

        private int BWX;
        private int AlphaX;

        public Color PickedColor
        {
            get => clResult;                 
        }       

        public int Luminosity
        {
            get => BWX;
            set
            {
                int v = value;
                if (v < 0) v = 0;
                if (v > 511) v = 511;
                BWX = v;
                Color c1, c2;
                c1 = Color.FromArgb(255,clFromPaletteMap);
                if(v<256)
                {
                    c2 = Color.FromArgb(255-v,0,0,0);
                }
                else
                {                    
                    c2 = Color.FromArgb(v-256, 255, 255, 255);
                }
                clFromBWMap = Color.FromArgb(
                    255,
                    (c1.R*(255-c2.A)+c2.R*c2.A)/255,
                    (c1.G*(255-c2.A)+c2.G*c2.A)/255,
                    (c1.B*(255-c2.A)+c2.B*c2.A)/255
                    );
                BWMap.Invalidate();
                AlphaMap.Invalidate();
                Alpha = Alpha;
            }
        }

        public int Alpha
        {
            get
            {
                return AllowTransparentColors ? AlphaX : 255;                
            }
            set
            {                
                int v = value;
                v = AllowTransparentColors ? v : 255;
                if (v < 0) v = 0;
                if (v > 255) v = 255;
                AlphaX = v;
                clResult = Color.FromArgb(v,clFromBWMap);
                ColorName.Text = '#' + clResult.Name.ToUpper();
                AlphaMap.Invalidate();
                OnPickedColorChanged(this, null);
            }
        }

        public bool AllowTransparentColors
        {
            get => AlphaMap.Visible;
            set
            {
                if (AlphaMap.Visible == value) return;
                if(value)
                {
                    BWMap.Top = AlphaMap.Height;
                    ColorMap.Height -= AlphaMap.Height;
                    ColorMap.Top += AlphaMap.Height;
                }
                else
                {                                     
                    BWMap.Top = 0;
                    ColorMap.Height+=AlphaMap.Height;
                    ColorMap.Top -= AlphaMap.Height;
                    Alpha = 255;                    
                }
                ColorMap.Invalidate();
                AlphaMap.Visible = value;
            }
        }
        
        public bool ShowColorName
        {
            get => ColorName.Visible;
            set => ColorName.Visible=value;                      
        }

        public HLSColorPicker()
        {
            InitializeComponent();            
            DrawPalette();
            DrawBWPalette();

            mdsColorMap = new MouseDragSystem()
            {
                MouseMoveAction = delegate(object o,MouseEventArgs e)
                {                    
                    MoveCross(e.X, e.Y);
                },
                control = ColorMap
            };
            mdsBWMap = new MouseDragSystem()
            {
                MouseMoveAction = delegate (object o, MouseEventArgs e)
                {
                    Luminosity = (int)((float)e.X/BWMap.Width*511);
                },
                control = BWMap
            };
            mdsAlphaMap = new MouseDragSystem()
            {
                MouseMoveAction = delegate (object o, MouseEventArgs e)
                {
                    Alpha = (int)((float)e.X / AlphaMap.Width * 255);
                },
                control = AlphaMap

            };
            typeof(Control).InvokeMember("DoubleBuffered", BindingFlags.SetProperty
                | BindingFlags.Instance | BindingFlags.NonPublic, null,
                ColorMap, new object[] { true });
            typeof(Control).InvokeMember("DoubleBuffered", BindingFlags.SetProperty
                | BindingFlags.Instance | BindingFlags.NonPublic, null,
                BWMap, new object[] { true });
            typeof(Control).InvokeMember("DoubleBuffered", BindingFlags.SetProperty
                | BindingFlags.Instance | BindingFlags.NonPublic, null,
                AlphaMap, new object[] { true });

            GetColorFromMap();
            Luminosity = 255;
            Alpha = 255;
            OldCMSize = ColorMap.Size;
        }

        private void ColorMap_Paint(object sender, PaintEventArgs e)
        {
            var bmp = new Bitmap(ColorPalette, ColorMap.Size);
            e.Graphics.DrawImage(bmp,Point.Empty);
            DrawCross(e.Graphics);           
        }

        private void DrawPalette()
        {            
            const float m = 4.25F;
            const int w = 360, h = 256;
            float[] mr = { 0, -m, 0, 0, m, 0 };
            float[] mg = { m, 0, 0, -m, 0, 0 };
            float[] mb = { 0, 0, m, 0, 0, -m };
            byte[] r=new byte[w], g=new byte[w], b= new byte[w];
            double dr = 255, dg = 0, db = 0;
            for (int i = 0; i < w; i++) 
            {
                int k = i / 60;
                r[i] = (byte)(dr += mr[k]);
                g[i] = (byte)(dg += mg[k]);
                b[i] = (byte)(db += mb[k]);
            }

            Rectangle rect = new Rectangle(0, 0, w, h);
            BitmapData bmpData = ColorPalette.LockBits(rect, ImageLockMode.ReadWrite, ColorPalette.PixelFormat);

            IntPtr ptr = bmpData.Scan0;
            byte[] ImageBytes = new byte[4*w*h];
            for (int j = 0; j < h; j++)
            {
                int k = 4 * j * w;
                for (int i = 0; i < w; i++,k+=4)
                {
                    int nr = r[i] * (255 - j) / 255;
                    int ng = g[i] * (255 - j) / 255;
                    int nb = b[i] * (255 - j) / 255;
                    /*B*/
                    ImageBytes[k] = (byte)nb;
                    /*G*/
                    ImageBytes[k + 1] = (byte)ng;
                    /*R*/
                    ImageBytes[k + 2] = (byte)nr;
                    /*A*/
                    ImageBytes[k + 3] = 255;
                }
            }
            System.Runtime.InteropServices.Marshal.Copy(ImageBytes, 0, ptr, ImageBytes.Length);
            ColorPalette.UnlockBits(bmpData);
        }

        private void DrawCross(Graphics g)
        {
            var p = new Pen(Color.LightGray, 3);            
            g.DrawLine(p,CrossPosition.X-8,CrossPosition.Y,CrossPosition.X+8,CrossPosition.Y);
            g.DrawLine(p,CrossPosition.X,CrossPosition.Y-8,CrossPosition.X,CrossPosition.Y+8);            
        }

        public void MoveCross(int x,int y)
        {
            int x1 = (x < 0) ? 0 : ((x > ColorMap.Width) ? ColorMap.Width - 1 : x);
            int y1 = (y < 0) ? 0 : ((y > ColorMap.Height) ? ColorMap.Height - 1 : y);
            CrossPosition.X = x1;
            CrossPosition.Y = y1;
            CrossRatioX = ((double)CrossPosition.X / ColorMap.Width);
            CrossRatioY = ((double)CrossPosition.Y / ColorMap.Height);
            ColorMap.Invalidate();
            GetColorFromMap();
            BWMap.Invalidate();
            AlphaMap.Invalidate();
            UpdateColors();
        }

        public void RefreshCross()
        {
            double x1 = CrossRatioX * ColorMap.Width;
            double y1 = CrossRatioY * ColorMap.Height;
            CrossPosition.X = (int)x1;
            CrossPosition.Y = (int)y1;
            ColorMap.Invalidate();
            GetColorFromMap();
            BWMap.Invalidate();
            AlphaMap.Invalidate();
            UpdateColors();
        }

        private void GetColorFromMap()
        {
            int x = (int)((double)CrossPosition.X / ColorMap.Width * 360),
                y = (int)((double)CrossPosition.Y / ColorMap.Height * 256);
            if (x < 0) x = 0;
            if (x >= 360) x = 359;
            if (y < 0) y = 0;
            if (y >= 256) y = 255;                     
            clFromPaletteMap = ColorPalette.GetPixel(x, y);            
        }

        private void ColorPicker_Load(object sender, EventArgs e)
        {
            MoveCross(0, 0);
        }

        private void DrawBWPalette()
        {
            const int w = 512, h = 1;
            Rectangle rect = new Rectangle(0, 0, w, h);
            BitmapData bmpData = BWPalette.LockBits(rect, ImageLockMode.ReadWrite, BWPalette.PixelFormat);

            IntPtr ptr = bmpData.Scan0;
            byte[] ImageBytes = new byte[4 * w * h];

            for (int j = 0; j < h; j++)
            {
                for (int i = 0; i <= 255; i++)
                {
                    int k = 4 * w * j + 4 * i;
                    ImageBytes[k] = ImageBytes[k + 1] = ImageBytes[k + 2] = 0;
                    ImageBytes[k + 3] = (byte)(255 - i);
                    k = 4*w * j + 4 * (256 + i);
                    ImageBytes[k] = ImageBytes[k + 1] = ImageBytes[k + 2] = 255;
                    ImageBytes[k + 3] = (byte)(i);
                }
            }
            System.Runtime.InteropServices.Marshal.Copy(ImageBytes, 0, ptr, ImageBytes.Length);
            BWPalette.UnlockBits(bmpData);
        }

        private void DrawPointer(Graphics g,int value,int min,int max,Size size)
        {
            if (value < min) value = min;
            if (value >= max) value = max-1;
            float d = (float)(value - min) / (max - min)*size.Width;            
            g.DrawLine(new Pen(Color.FromArgb(255,128,128,128),5),d,0,d,size.Height);
            g.DrawLine(new Pen(Color.FromArgb(128,0,0,0),3),d,0,d,size.Height);

        }
        private void BWMap_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(clFromPaletteMap);
            var bmp = new Bitmap(BWPalette, new Size(BWMap.Width,1));
            for (int i = 0; i < BWMap.Height; i++)
            {
                e.Graphics.DrawImage(bmp, new Point(0,i));
            }
            DrawPointer(e.Graphics,Luminosity,0,512,BWMap.Size);            
        }

        private void AlphaMap_Paint(object sender, PaintEventArgs e)
        {
            //e.Graphics.Clear(Color.Red);
            var squares = new Bitmap(AlphaMap.Width, AlphaMap.Height);
            int sqsz = 9;
            using (Graphics g = Graphics.FromImage(squares))
            {
                for (int i = 0; i < AlphaMap.Width + sqsz; i += sqsz)
                    for (int j = 0; j < AlphaMap.Height + sqsz; j += sqsz)
                        g.FillRectangle(new SolidBrush(Color.FromArgb(128*( (i + j) % 2), 0, 0, 0)), i, j, sqsz, sqsz);
            }
            var alphascale = new Bitmap(AlphaMap.Width,AlphaMap.Height);
            float d = (float)AlphaMap.Width / 256;
            for(int i=0;i<256;i++)
            {
                Graphics.FromImage(alphascale).FillRectangle(new SolidBrush(Color.FromArgb(i, clFromBWMap)), i * d, 0, d, AlphaMap.Height);
            }

            e.Graphics.DrawImage(squares,Point.Empty);
            e.Graphics.DrawImage(alphascale,Point.Empty);

            DrawPointer(e.Graphics, Alpha, 0, 256, AlphaMap.Size);            
        }        
        
        private void UpdateColors()
        {
            Luminosity = Luminosity;
            Alpha = Alpha;
        }        

        protected void OnPickedColorChanged(object sender,EventArgs e)
        {
            if (PickedColorChanged != null)
                PickedColorChanged(this,e);
        }

        private void ColorMap_Resize(object sender, EventArgs e)
        {
            RefreshCross();
            ColorName.Invalidate();
        }

        private void ColorPicker_Resize(object sender, EventArgs e)
        {            
            //CrossRatioX=
            
        }
    }
}
