using System;
using System.Windows;

namespace Trees
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            // Init resources from Cyber.Core
            var dictionary = new ResourceDictionary { Source = new Uri("/Resources/MyResources.xaml", UriKind.RelativeOrAbsolute) };
            this.Resources.MergedDictionaries.Add(dictionary);
        }
    }
}
