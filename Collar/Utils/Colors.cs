using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Media;

namespace Collar.Utils
{
    public class Colors
    {
        public static System.Drawing.Color Add(System.Drawing.Color c1, System.Drawing.Color c2)
        {
            return System.Drawing.Color.FromArgb(c1.A,
                (c1.R * (255 - c2.A) + c2.R * c2.A) / 255,
                (c1.G * (255 - c2.A) + c2.G * c2.A) / 255,
                (c1.B * (255 - c2.A) + c2.B * c2.A) / 255);
        }
        public static System.Windows.Media.Color Add(System.Windows.Media.Color c1, System.Windows.Media.Color c2)
        {
            return System.Windows.Media.Color.FromArgb(c1.A,
                (byte)((c1.R * (255 - c2.A) + c2.R * c2.A) / 255),
                (byte)((c1.G * (255 - c2.A) + c2.G * c2.A) / 255),
                (byte)((c1.B * (255 - c2.A) + c2.B * c2.A) / 255));
        }
    }
}
