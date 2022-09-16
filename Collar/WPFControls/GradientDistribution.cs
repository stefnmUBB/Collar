using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;

namespace Collar.WPFControls
{
    public enum GradientDistribution
    {
        Vertical,
        Horizontal,
        Adjust,       
    }

    public class GradientDistributionSetter
    {
        public static Brush Create(GradientStopCollection col,Size sz=new Size(),GradientDistribution d=GradientDistribution.Adjust)
        {
            switch(d)
            {
                case GradientDistribution.Horizontal: return new LinearGradientBrush(col,new Point(0,0),new Point(0,1));
                case GradientDistribution.Vertical: return new LinearGradientBrush(col,new Point(0,0),new Point(1,0));
                default: 
                case GradientDistribution.Adjust: return Create(col, default, (sz.Width < sz.Height) ? 
                    GradientDistribution.Horizontal : GradientDistribution.Vertical);                                    
            }
        }
    }
    
}
