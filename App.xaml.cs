﻿using ColorPickerLite.Services.ColorPickerService;
using ColorPickerLite.Views;
using Prism.Ioc;
using System.Windows;

namespace ColorPickerLite
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IColorPickerService, ColorPickerService>();
        }
    }
}
