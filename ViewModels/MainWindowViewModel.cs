using ColorPickerLite.Services.ColorPickerService;
using Prism.Commands;
using Prism.Mvvm;
using System.Drawing;
using System.Windows;
using System.Windows.Input;

namespace ColorPickerLite.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        #region Properties

        private string _title = "Color Picker By Ori";
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }


        private bool _isColorPickerOff = false;
        public bool IsColorPickerOff
        {
            get => _isColorPickerOff;
            set => SetProperty(ref _isColorPickerOff, value);
        }

        #region Tooltips
        private string _RGBTooltip = "";
        public string RGBTooltip
        {
            get { return _RGBTooltip; }
            set => SetProperty(ref _RGBTooltip, value);
        }

        private string _HEXTooltip = "";
        public string HEXTooltip
        {
            get { return _HEXTooltip; }
            set => SetProperty(ref _HEXTooltip, value);
        }
        #endregion

        #region Visibility Properties
        private string _copyButtonsAndResetButtonAndWarningVisibility = "Hidden";
        public string CopyButtonsAndResetButtonAndWarningVisibility
        {
            get { return _copyButtonsAndResetButtonAndWarningVisibility; }
            set => SetProperty(ref _copyButtonsAndResetButtonAndWarningVisibility, value);
        }

        private string _easterEggVisibility = "Hidden";
        public string EasterEggVisibility
        {
            get { return _easterEggVisibility; }
            set => SetProperty(ref _easterEggVisibility, value);
        }
        #endregion

        #region Selected color
        private Color _selectedColor;
        public Color SelectedColor
        {
            get => _selectedColor;
            set => SetProperty(ref _selectedColor, value);
        }

        private string _selectedColorDisplayCodeRGB = "RGB(XXX, YYY, ZZZ)";
        public string SelectedColorDisplayCodeRGB
        {
            get => _selectedColorDisplayCodeRGB;
            set => SetProperty(ref _selectedColorDisplayCodeRGB, value);
        }

        private string _selectedColorDisplayCodeHEX = "#XXXXXX";
        public string SelectedColorDisplayCodeHEX
        {
            get => _selectedColorDisplayCodeHEX;
            set => SetProperty(ref _selectedColorDisplayCodeHEX, value);
        }
        #endregion

        #region Live Color
        public string LiveDisplayColorCodeRGB
        {
            get => _liveDisplayColorCodeRGB;
            set => SetProperty(ref _liveDisplayColorCodeRGB, value);
        }
        public string LiveDisplayColorCodeHEX
        {
            get => _liveDisplayColorCodeHEX;
            set => SetProperty(ref _liveDisplayColorCodeHEX, value);
        }

        private Color _liveDisplayColor;
        public Color LiveDisplayColor
        {
            get => _liveDisplayColor;
            set => SetProperty(ref _liveDisplayColor, value);
        }
        #endregion

        #endregion

        private readonly IColorPickerService _colorPickerService;
        private string _liveDisplayColorCodeRGB;
        private string _liveDisplayColorCodeHEX;
        public MainWindowViewModel(IColorPickerService colorPickerService)
        {
            CopyRGBCommand = new DelegateCommand(CopyRGBToClipboard);
            CopyHEXCommand = new DelegateCommand(CopyHEXToClipboard);
            ResetColorPickerCommand = new DelegateCommand(ResetColorPicker);
            EasterEggTriggeredCommand = new DelegateCommand(EasterEggTriggered);
            _colorPickerService = colorPickerService;
            InitializeMouseHook();
        }


        private void InitializeMouseHook()
        {
            _colorPickerService.StartListening();
            _colorPickerService.ColorPicked += (s, e) =>
            {
                LiveDisplayColor = e.Color;
                LiveDisplayColorCodeRGB = $"RGB({e.Color.R}, {e.Color.G}, {e.Color.B})";
                LiveDisplayColorCodeHEX = $"{ColorTranslator.ToHtml(e.Color)}";


                if (e.Clicked)
                {
                    SelectedColor = e.Color;
                    SelectedColorDisplayCodeRGB = $"RGB({e.Color.R}, {e.Color.G}, {e.Color.B})";
                    SelectedColorDisplayCodeHEX = $"{ColorTranslator.ToHtml(e.Color)}";
                    IsColorPickerOff = true;
                    CopyButtonsAndResetButtonAndWarningVisibility = "Visible";
                    RGBTooltip = $"Click to copy: RGB({e.Color.R}, {e.Color.G}, {e.Color.B})";
                    HEXTooltip = $"Click to copy: {ColorTranslator.ToHtml(e.Color)}";
                    _colorPickerService.StopListening();
                }
            };
        }

        #region Commands
        public ICommand EasterEggTriggeredCommand { get; }
        private void EasterEggTriggered()
        {
            if (EasterEggVisibility == "Hidden")
            {
                EasterEggVisibility = "Visible";
                return;
            }
            EasterEggVisibility = "Hidden";
        }

        public ICommand CopyHEXCommand { get; }
        private void CopyHEXToClipboard()
        {
            if (!string.IsNullOrEmpty(SelectedColor.ToString()))
            {
                //CopyButtonsAndResetButtonAndWarningVisibility = "Hidden";
                Clipboard.SetText(ColorTranslator.ToHtml(SelectedColor));
                //_colorPickerService.StartListening();
                //IsColorPickerOff = false;
            }
        }

        public ICommand CopyRGBCommand { get; }
        private void CopyRGBToClipboard()
        {
            if (!string.IsNullOrEmpty(SelectedColor.ToString()))
            {
                //CopyButtonsAndResetButtonAndWarningVisibility = "Hidden";
                string rgbString = $"RGB({SelectedColor.R}, {SelectedColor.G}, {SelectedColor.B})";
                Clipboard.SetText(rgbString);
                //_colorPickerService.StartListening();
                //IsColorPickerOff = false;
            }
        }

        public ICommand ResetColorPickerCommand { get; }
        private void ResetColorPicker()
        {
            CopyButtonsAndResetButtonAndWarningVisibility = "Hidden";
            _colorPickerService.StartListening();
            IsColorPickerOff = false;
        }
        #endregion
    }
}
