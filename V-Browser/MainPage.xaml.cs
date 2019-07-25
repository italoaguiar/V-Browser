using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using V_Browser.Controls;
using Windows.ApplicationModel.Store;
using Windows.UI.Popups;
using Microsoft.Advertising.WinRT.UI;
using System.Globalization;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace V_Browser
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            

            Tab t = new Tab();
            t.RequestForClose += t_RequestForClose;
            _tabs.Add(t);
                
            tabs.ItemsSource = _tabs;

            content.Content = t;

            currentLicense = CurrentApp.LicenseInformation;

            if (!currentLicense.ProductLicenses["VBRA001"].IsActive)
            {
                pp1.IsOpen = true;
            }
            else ad.IsEnabled = false;

            
        }
        LicenseInformation currentLicense;
        public List<Tab> _tabs = new List<Tab>();


        private void tabs_ItemClick(object sender, ItemClickEventArgs e)
        {
            int index = tabs.Items.IndexOf(e.ClickedItem);
            content.Content = _tabs[index];
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Tab t = new Tab();
            t.RequestForClose += t_RequestForClose;
            _tabs.Add(t);
            content.Content = _tabs[_tabs.Count - 1];
            tabs.ItemsSource = null;
            tabs.ItemsSource = _tabs;            
        }

        void t_RequestForClose(object sender, EventArgs e)
        {
            if (_tabs.Count != 1)
            {
                _tabs.Remove(sender as Tab);
                tabs.ItemsSource = null;
                tabs.ItemsSource = _tabs;
                content.Content = _tabs[_tabs.Count - 1];
            }
        }

        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (!currentLicense.ProductLicenses["VBRA001"].IsActive)
            {
                try
                {


                    // The customer doesn't own this feature, so 
                    // show the purchase dialog.

                    var results = await CurrentApp.RequestProductPurchaseAsync("VBRA001",false);
                    //Check the license state to determine if the in-app purchase was successful.
                    if (currentLicense.ProductLicenses["VBRA001"].IsActive)
                        pp1.IsOpen = false;

                }
                catch (Exception)
                {
                    // The in-app purchase was not completed because 
                    // an error occurred.
                }
            }
            else
            {
                // The customer already owns this feature.
            }
        }

        

        private void AdControl_ErrorOccurred(object sender, AdErrorEventArgs e)
        {
            removeAdBtn.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

        private void AdControl_AdRefreshed(object sender, RoutedEventArgs e)
        {
            removeAdBtn.Visibility = Windows.UI.Xaml.Visibility.Visible;
        }
    }
}
