using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace V_Browser.Controls
{
    public sealed partial class Navigator : UserControl
    {
        public Navigator()
        {
            this.InitializeComponent();

            web.NavigationCompleted += web_NavigationCompleted;
            web.FrameNavigationCompleted += web_NavigationCompleted;
            //web.FrameNavigationStarting += web_FrameNavigationStarting;
            web.NavigationStarting += web_FrameNavigationStarting;
            web.UnviewableContentIdentified += web_UnviewableContentIdentified;

            speedDial = new SpeedDial(this);
            content.Content = speedDial;


        }
        
        private WebView web = new WebView();
        private SpeedDial speedDial;

        #region Events
        public event TypedEventHandler<WebView, WebViewNavigationCompletedEventArgs> NavigationCompleted;
        public event TypedEventHandler<WebView, WebViewNavigationStartingEventArgs> NavigationStarting;
        public event TypedEventHandler<WebView, WebViewUnviewableContentIdentifiedEventArgs> UnviewableContentIdentified;

        void web_FrameNavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            if (NavigationStarting != null)
                NavigationStarting(sender, args);
        }
        void web_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            if (NavigationCompleted != null)
                NavigationCompleted(sender, args);
        }
        void web_UnviewableContentIdentified(WebView sender, WebViewUnviewableContentIdentifiedEventArgs args)
        {
            if (UnviewableContentIdentified != null)
                UnviewableContentIdentified(sender, args);
        }
        #endregion


        public string Title
        {
            get 
            {
                if (content.Content is WebView)
                {
                    return web.DocumentTitle;
                }
                else return "Speed Dial";
            }
        }
        public bool CanGoBack
        {
            get { return web.CanGoBack; }
        }
        public void NavigateToSpeedDial()
        {
            content.Content = speedDial;
        }
        public void GoBack()
        {
            if (CanGoBack)
            {
                web.GoBack();
            }
            else
            {
                content.Content = speedDial;
            }
        }
        public async Task<string> GetHtmlCode()
        {
            return System.Net.WebUtility.HtmlDecode(await web.InvokeScriptAsync("eval", new string[] { "document.documentElement.outerHTML;" }));
        }
        public async Task<WriteableBitmap> GetThumbnailAsync()
        {
            InMemoryRandomAccessStream ms = new InMemoryRandomAccessStream();
            await web.CapturePreviewToStreamAsync(ms);

            WriteableBitmap bmp = new WriteableBitmap(300,160);
            await bmp.SetSourceAsync(ms);

            return bmp;
        }
        public Uri Source
        {
            get { return web.Source; }
            set { web.Source = value; }
        }
        public async Task<InMemoryRandomAccessStream> GetThumbnailStreamAsync()
        {
            InMemoryRandomAccessStream ms = new InMemoryRandomAccessStream();
            await web.CapturePreviewToStreamAsync(ms);
            return ms;            
        }
        public void Refresh()
        {
            web.Refresh();
        }
        
        public void Navigate(Uri uri)
        {
            if(content.Content is WebView)
                web.Navigate(uri);
            else
            {
                content.Content = web;
                web.Navigate(uri);
            }

            
        }

    }
}
