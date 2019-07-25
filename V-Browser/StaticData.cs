using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media.Animation;

namespace V_Browser
{
    class StaticData
    {
        public static Downloads Downloads = new Downloads();
        private static Popup settingsPopup = new Popup();
        static StaticData()
        {
            int settingsWidth = 600;
            Rect windowBounds = Window.Current.Bounds;
            

            //settingsPopup.Closed += settingsPopup_Closed;
            //Window.Current.Activated += Current_Activated;
            settingsPopup.IsLightDismissEnabled = false;
            Page settingPage = StaticData.Downloads;


            settingsPopup.Width = settingsWidth;
            settingsPopup.Height = windowBounds.Height;

            // Add the proper animation for the panel.
            settingsPopup.ChildTransitions = new TransitionCollection();
            settingsPopup.ChildTransitions.Add(new PaneThemeTransition()
            {

                Edge = (SettingsPane.Edge == SettingsEdgeLocation.Right) ?
                EdgeTransitionLocation.Right :
                EdgeTransitionLocation.Left
            });

            if (settingPage != null)
            {
                settingPage.Width = settingsWidth;
                settingPage.Height = windowBounds.Height;
            }

            // Place the SettingsFlyout inside our Popup window.
            if (settingsPopup.Child != settingPage)
                settingsPopup.Child = settingPage;

            // Let's define the location of our Popup.
            settingsPopup.SetValue(Canvas.LeftProperty, SettingsPane.Edge == SettingsEdgeLocation.Right ? (windowBounds.Width - settingsWidth) : 0);
            settingsPopup.SetValue(Canvas.TopProperty, 0);
            
        }

        public static void AddDownload(System.Uri url, string name, string ext)
        {
            Downloads.AddDownload(new File(url,name,ext));
        }


        public static void ShowDownloadPanel()
        {
            settingsPopup.IsOpen = true;
        }
    }
}
