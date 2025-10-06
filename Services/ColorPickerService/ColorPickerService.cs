using Gma.System.MouseKeyHook;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ColorPickerLite.Services.ColorPickerService
{
    public class ColorPickerService : IColorPickerService
    {
        private IKeyboardMouseEvents _hook;
        public ColorPickerService()
        {
            StartListening();
        }
        private System.Windows.Forms.Timer _timer;
        private Point _lastMouseLocation;
        public event EventHandler<ColorPickedEventArgs> ColorPicked;

        public void StartListening()
        {
            _hook = Hook.GlobalEvents();
            _hook.MouseMove += OnMouseMove;
            _hook.MouseDownExt += OnMouseDown;
        }

        public void StopListening()
        {
            if (_hook != null)
            {
                _hook.MouseMove -= OnMouseMove;  // הסרה של האירוע שהוגדר בקונסטרקטור
                _hook.MouseDownExt -= OnMouseDown;
                _hook.Dispose();                  // ניתוק מלא
                _hook = null;
            }
        }
        private void OnMouseMove(object s, MouseEventArgs e)
        {
            _lastMouseLocation = e.Location;

            if (_timer == null)
            {
                _timer = new System.Windows.Forms.Timer();
                _timer.Interval = 300; // 1 שניה
                _timer.Tick += Timer_Tick;
                _timer.Start();
            }
            else
            {
                _timer.Stop();  // מאפס את הטיימר כל תזוזה
                _timer.Start();
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            _timer.Stop();  // עצירת הטיימר עד לתזוזה הבאה
            var color = GetColorAtPoint(_lastMouseLocation);
            ColorPicked?.Invoke(this, new ColorPickedEventArgs(color, false));
        }

        private void OnMouseDown(object s, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var location = e.Location;
                var color = GetColorAtPoint(location);
                ColorPicked?.Invoke(this, new ColorPickedEventArgs(color, true));
            }
        }

        private Color GetColorAtPoint(Point location)
        {
            using (Bitmap screenshot = new Bitmap(1, 1))
            using (Graphics g = Graphics.FromImage(screenshot))
            {
                g.CopyFromScreen(location, Point.Empty, new Size(1, 1));
                return screenshot.GetPixel(0, 0);
            }
        }
    }

    public class ColorPickedEventArgs : EventArgs
    {
        public Color Color { get; }
        public bool Clicked { get; }
        public ColorPickedEventArgs(Color color, bool clicked = false)
        {
            Color = color;
            Clicked = clicked;
        }
    }
}
