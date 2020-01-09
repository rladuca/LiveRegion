using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LiveRegionCustom
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const int UIA_LiveRegionChangedEventId = 20024;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        bool _liveRegionsSupported = false;

        DependencyProperty _liveSettingProperty = null;

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _liveRegionsSupported = AutomationEvent.LookupById(UIA_LiveRegionChangedEventId) != null;

            if (_liveRegionsSupported)
            {
                if (_liveSettingProperty == null)
                {
                    _liveSettingProperty = typeof(AutomationProperties).GetField("LiveSettingProperty", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public).GetValue(null) as DependencyProperty;
                }

                if (_liveSettingProperty != null)
                {
                    Updater.SetValue(_liveSettingProperty, Enum.Parse(_liveSettingProperty.PropertyType, "2"));
                }
            }
        }

        int count = 0;

        const AutomationEvents LiveRegionChangedEvent = (AutomationEvents)18;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Updater.Text = count++.ToString();
            UIElementAutomationPeer.FromElement(Updater).RaiseAutomationEvent(LiveRegionChangedEvent);
        }
    }
}
