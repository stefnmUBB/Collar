using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Collar.WPFControls
{
    public enum CornerRadiusStyle
    {
        Percentage,
        PercentageWidth,
        PercentageHeight,    
        PercentageMin,
        Default
    }

    class CornerRadiusSetter
    {
        public static CornerRadius Create(CornerRadius cr,Size sz,CornerRadiusStyle st=CornerRadiusStyle.Default)
        {
            double f=0;
            switch(st)
            {
                case CornerRadiusStyle.Percentage:
                    f = Math.Sqrt(sz.Width*sz.Height); break;
                case CornerRadiusStyle.PercentageHeight:
                    f = sz.Height; break;
                case CornerRadiusStyle.PercentageWidth:
                    f = sz.Width; break;
                case CornerRadiusStyle.PercentageMin:
                    f = Math.Min(sz.Width,sz.Height); break;
                case CornerRadiusStyle.Default: f = 1; break;
            }
            return new CornerRadius(f * cr.TopLeft, f * cr.TopRight, f * cr.BottomLeft, f * cr.BottomRight);
        }
    }
}
