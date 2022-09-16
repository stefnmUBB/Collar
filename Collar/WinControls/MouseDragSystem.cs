using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Collar.WinControls
{
    class MouseDragSystem
    {
        public bool MousePressed = false;
        private Control ctrl=null;
        public Control control
        {
            get => ctrl;
            set => Attach(value);
        }

        public Action<object, MouseEventArgs> MouseMoveAction;
        public void MouseDown(object o,MouseEventArgs e)
        {
            MousePressed = true;
        }

        public void MouseMove(object o,MouseEventArgs e)
        {
            if (!MousePressed) return;            
            MouseMoveAction(o, e);
        }

        public void MouseUp(object o,MouseEventArgs e)
        {
            MousePressed = false;
        }

        public void Attach(Control control)
        {
            Detach();
            ctrl = control;
            ctrl.MouseDown += MouseDown;
            ctrl.MouseMove += MouseMove;
            ctrl.MouseUp += MouseUp;            
        }

        public void Detach()
        {
            if(ctrl!=null)
            {
                ctrl.MouseDown -= MouseDown;
                ctrl.MouseMove -= MouseMove;
                ctrl.MouseUp -= MouseUp;
                ctrl = null;
            }
        }
    }
}
