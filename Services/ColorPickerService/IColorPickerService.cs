using System;

namespace ColorPickerLite.Services.ColorPickerService
{
    public interface IColorPickerService
    {
        event EventHandler<ColorPickedEventArgs> ColorPicked;
        void StartListening();
        void StopListening();
    }
}
