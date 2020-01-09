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
        /// <summary>
        /// From https://docs.microsoft.com/en-us/windows/win32/winauto/uiauto-event-ids
        /// </summary>
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
            // Query to ensure we have a definition for the LiveRegionChanged event.
            _liveRegionsSupported = AutomationEvent.LookupById(UIA_LiveRegionChangedEventId) != null;

            if (_liveRegionsSupported)
            {
                // Query and store the LiveSettingProperty to later apply to our TextBlock.
                if (_liveSettingProperty == null)
                {
                    _liveSettingProperty = typeof(AutomationProperties).GetField("LiveSettingProperty", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public).GetValue(null) as DependencyProperty;
                }

                if (_liveSettingProperty != null)
                {
                    // Apply "Assertive" to our TextBlock.
                    // See: https://docs.microsoft.com/en-us/dotnet/api/system.windows.automation.automationlivesetting?view=netframework-4.8
                    Updater.SetValue(_liveSettingProperty, Enum.Parse(_liveSettingProperty.PropertyType, "2"));
                }
            }
        }

        int count = 0;

        // Store the correct WPF AutomationEvent
        // See: https://docs.microsoft.com/en-us/dotnet/api/system.windows.automation.peers.automationevents?view=netframework-4.8
        const AutomationEvents LiveRegionChangedEvent = (AutomationEvents)18;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Updater.Text = count++.ToString();

            // Raise the event if supported.
            if (_liveRegionsSupported)
            {
                UIElementAutomationPeer.FromElement(Updater).RaiseAutomationEvent(LiveRegionChangedEvent);
            }
        }
    }
}
