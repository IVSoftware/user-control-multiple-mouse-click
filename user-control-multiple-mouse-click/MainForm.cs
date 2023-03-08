using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace user_control_multiple_mouse_click
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }
    }
    enum ColorState{ StateBlue, StateRed, StateGreen, }
    class RoundButton : Button
    {
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            BackColor = Color.CornflowerBlue;
            var region = new Region(new Rectangle(Point.Empty, ClientSize));
            Bitmap bitmap = new Bitmap(Width, Height);
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.FillEllipse(
                Brushes.LimeGreen, 
                new RectangleF(
                    new PointF(4, 4), 
                    new SizeF(bitmap.Width - 8, bitmap.Height - 8))
            );
            for (int x = 0; x < bitmap.Width; x++) for (int y = 0; y < bitmap.Height; y++)
                {
                    Color pixel = bitmap.GetPixel(x, y);
                    if (pixel.ToArgb() != Color.LimeGreen.ToArgb())
                    { 
                        region.Exclude(new Rectangle(x, y, 1, 1));
                    }
                }
            Region = region;
        }
        private ColorState _colorState;
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            BeginInvoke(new Action(() =>
            {
                _colorState = (ColorState)(((int)(_colorState + 1)) % 3);
                switch (_colorState)
                {
                    case ColorState.StateBlue:
                        BackColor = Color.CornflowerBlue;
                        break;
                    case ColorState.StateRed:
                        BackColor = Color.Red;
                        break;
                    case ColorState.StateGreen:
                        BackColor = Color.Green;
                        break;
                }
            }));
        }
    }
}
