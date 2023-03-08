As I understand it, you made a custom round button and would like its colors to change based on mouse clicks even when they occur in rapid succession. To achieve this, you may want to try the `MouseDown` event instead, and use `BeginInvoke` to post the color change messages without blocking the mouse events already in the queue.

For testing, I made _three_ color states instead of two so that two rapid clicks of the mouse would cause a definite change that would indicate this is working. In other words, for testing purposes since the button starts out as Blue, two rapid clicks should cycle through the Red state and end up on Green.  (I will attempt to reproduce your use case with a "typical" implementation since you haven't shown how you are inheriting or implementing your custom button.)

[![screenshot][1]][1]

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


  [1]: https://i.stack.imgur.com/aL42L.png