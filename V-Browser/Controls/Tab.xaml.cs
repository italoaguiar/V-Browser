using MyToolkit.Multimedia;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
using Windows.System;
using Windows.UI.Popups;
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
    public sealed partial class Tab : UserControl, INotifyPropertyChanged
    {
        public Tab()
        {
            this.InitializeComponent();

            timer.Interval = new TimeSpan(0, 0, 0, 0, 20);
            timer.Tick += (o, p) =>
            {
                progress.Value = currentPercentage+= 0.2;
            };
            regexes.Add(new KeyValuePair<string, string>("vidigvideo.com", "(?<=file=)(.*?)(?=&)"));
            
        }
        public Tab(System.Uri uri)
        {
            this.InitializeComponent();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 20);
            timer.Tick += (o, p) =>
            {
                progress.Value = currentPercentage += 0.2;
            };
            webview.Navigate(uri);
        }
        List<KeyValuePair<string, string>> regexes = new List<KeyValuePair<string, string>>();
        private void startTimer()
        {
            currentPercentage = 75;
            timer.Start();
        }
        DispatcherTimer timer = new DispatcherTimer();
        double currentPercentage = 75;

        public string Title
        {
            get { return webview.Title; }
            set
            {
                OnPropertyChanged("Title");
            }
        }
        

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            webview.Refresh();
        }

        private void AppBarButton_Click_1(object sender, RoutedEventArgs e)
        {
            webview.GoBack();
        }

        private void TextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                                
                try
                {
                    Uri u = new Uri((sender as TextBox).Text);
                    webview.Navigate(u);
                }
                catch
                {
                    string query = (sender as TextBox).Text;
                    if(!string.IsNullOrWhiteSpace(query))
                        webview.Navigate(new Uri(string.Format("http://www.google.com/search?ie=UTF-8&oe=UTF-8&sourceid=navclient&gfns=1&q={0}", query)));
                }
                finally
                {
                    webview.Focus(Windows.UI.Xaml.FocusState.Pointer);
                }
            }
        }

        private void webview_FrameNavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            try
            {
                

                startTimer();
                urlstodownload.Visibility = Visibility.Collapsed;
                urls.Items.Clear();
                if(!args.Uri.ToString().StartsWith("ms-appx"))
                txtAdress.Text = args.Uri.ToString();
            }
            catch { }
        }

        private async void webview_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            try
            {
                timer.Stop();
                pr1.IsActive = true;
                progress.Value = 0;
                string html = await webview.GetHtmlCode();
                                
                Regex regx = new Regex("http://([\\w+?\\.\\w+])+([a-zA-Z0-9\\~\\!\\@\\#\\$\\%\\^\\&amp;\\*\\(\\)_\\-\\=\\+\\\\\\/\\?\\.\\:\\;\\'\\,]*)?", RegexOptions.IgnoreCase);
                MatchCollection mactches = regx.Matches(html);

                var videomatches = mactches.Cast<Match>().Where(p => p.Value.Contains(".mp4") || p.Value.Contains(".flv") ||
                p.Value.Contains(".avi") || p.Value.EndsWith(".mov") || p.Value.Contains(".mpg") || p.Value.Contains(".mpeg")
                || p.Value.Contains(".wmv") || p.Value.Contains("youtube.com/embed") || p.Value.Contains("youtu.be/"));

                if (videomatches.Count() > 0)
                {
                    urlCounter.Text = videomatches.Count().ToString();
                    urlstodownload.Visibility = Visibility.Visible;
                    urls.Items.Clear();
                    foreach (Match m in videomatches)
                    {
                        urls.Items.Add(await ResolveUriUsingPlugin(m.Value));
                    }
                }

                //string url = webview.Source.ToString();
                //if (url.Contains("youtube.com/embed/") || url.Contains("youtu.be/"))
                //{
                //    urls.Items.Add(await ResolveUriUsingPlugin(url));
                //}
            }
            catch (Exception e) { System.Diagnostics.Debug.WriteLine(e.ToString()); }
            finally { pr1.IsActive = false; Title = ""; }            
            
        }

        private void AppBarButton_Click_2(object sender, RoutedEventArgs e)
        {
            webview_NavigationCompleted(null, null);
            FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);  
        }

        private void urls_ItemClick(object sender, ItemClickEventArgs e)
        {
            StaticData.ShowDownloadPanel();
            DownloadUri u = (e.ClickedItem) as DownloadUri;
            StaticData.AddDownload(u.Url,u.FileName,u.FileType);
        }

        public async Task<DownloadUri> ResolveUriUsingPlugin(string matche)
        {
            if (matche.Contains("youtube.com/embed/") || matche.Contains("youtu.be/"))
            {
                Regex r = new Regex(@"youtu(?:\.be|be\.com)/(?:.*v(?:/|=)|(?:.*/)?)([a-zA-Z0-9-_]+)", RegexOptions.IgnoreCase);
                Match youtubeMatch = r.Match(matche);
                string id = youtubeMatch.Groups[0].Value.Replace("youtube.com/embed/", "").Replace("youtu.be/", "");
                System.Diagnostics.Debug.WriteLine("YouTube URI = " + matche);
                System.Diagnostics.Debug.WriteLine("YouTube ID = " + id);
                YouTubeUri[] uris = await YouTube.GetUrisAsync(id);
                var u = uris.Where(p => p.HasAudio == true).First();
                string title = await YouTube.GetVideoTitleAsync(id);

                return new DownloadUri() { AlternativeFileName = title + ".mp4", AlternativeFileType = "MP4", Url = u.Uri };
            }
            else
            {
                return new DownloadUri() { Url = new Uri(matche) };
            }
        }

        private void txtAdress_GotFocus(object sender, RoutedEventArgs e)
        {
            txtAdress.SelectAll();
        }


        private void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler RequestForClose;

        private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            if (RequestForClose != null)
                RequestForClose(this, null);
        }

        private void AppBarButton_Click_3(object sender, RoutedEventArgs e)
        {
            webview.NavigateToSpeedDial();
        }

        private void MenuFlyoutItem_Click_1(object sender, RoutedEventArgs e)
        {
            StaticData.ShowDownloadPanel();
        }

        private async void webview_UnviewableContentIdentified(WebView sender, WebViewUnviewableContentIdentifiedEventArgs args)
        {
            try
            {
                var c = System.IO.Path.GetFileName(args.Uri.AbsolutePath).Split('.');

                ResourceLoader LanguageLoader = new Windows.ApplicationModel.Resources.ResourceLoader();

                MessageDialog md = new MessageDialog(LanguageLoader.GetString("IncompatibleFile"), LanguageLoader.GetString("Warning"));
                md.Commands.Add(new UICommand(LanguageLoader.GetString("DownloadFile"), new UICommandInvokedHandler((cmd) =>
                {
                    StaticData.AddDownload(args.Uri, System.IO.Path.GetFileName(args.Uri.AbsolutePath), c[c.Length - 1]);
                    StaticData.ShowDownloadPanel();
                })));                
                md.Commands.Add(new UICommand(LanguageLoader.GetString("DefaultBrowser"), new UICommandInvokedHandler(async (cmd) =>
                {
                    await Windows.System.Launcher.LaunchUriAsync(args.Uri);
                })));
                md.Commands.Add(new UICommand(LanguageLoader.GetString("Cancel")));
                await md.ShowAsync();
                        
            }
            catch { }
        }

        private async void MenuFlyoutItem_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                SpeedDialManager.AddTile(await webview.GetThumbnailStreamAsync(), webview.Source, webview.Title);
            }
            catch { }
        }
    }
    public class DownloadUri
    {
        public Uri Url { get; set; }
        public string AlternativeFileName { get; set; }
        public string AlternativeFileType { get; set; }

        public string FileName
        {
            get { if (AlternativeFileName == null) return System.IO.Path.GetFileName(Url.AbsolutePath); else return AlternativeFileName; }
        }
        public string FileType
        {
            get
            {
                if (AlternativeFileType != null) return AlternativeFileType;
                else
                {
                    var c = System.IO.Path.GetFileName(Url.AbsolutePath).Split('.');
                    return c[c.Length - 1];
                }
            }
        }

    }
}
