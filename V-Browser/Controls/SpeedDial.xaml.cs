using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
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
    public sealed partial class SpeedDial : UserControl
    {
        public SpeedDial(Navigator nav)
        {
            this.InitializeComponent();

            _parent = nav;

            if (settings.Values["SpeedDialBackground"] == null)
            {
                MainGrid.Background = new ImageBrush() { 
                    ImageSource = new BitmapImage(new Uri("ms-appx:///Html/img/W1.jpg", UriKind.Absolute)),
                    Stretch=Stretch.UniformToFill
                };
            }
            else
            {
                MainGrid.Background = new ImageBrush() { 
                    ImageSource = new BitmapImage(new Uri(settings.Values["SpeedDialBackground"].ToString(), UriKind.Absolute)),
                    Stretch=Stretch.UniformToFill
                };
            }
            setSpeedDial();
        }
        private Navigator _parent;

        Windows.Storage.ApplicationDataContainer settings = Windows.Storage.ApplicationData.Current.LocalSettings;
        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            bckPp.IsOpen = true;

        }

        private void Image_Tapped(object sender, TappedRoutedEventArgs e)
        {
            MainGrid.Background = new ImageBrush() { ImageSource = (sender as Image).Source, Stretch=Stretch.UniformToFill };
            settings.Values["SpeedDialBackground"] = ((sender as Image).Source as BitmapImage).UriSource.ToString();            

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Uri u = new System.Uri(txtUrlBackground.Text);
                MainGrid.Background = new ImageBrush() { ImageSource = new BitmapImage(u) };
                settings.Values["SpeedDialBackground"] = u.ToString();
            }
            catch { bckPp.IsOpen = false; }
        }
        private async void showMessageDialog(string message)
        {
            MessageDialog md = new MessageDialog(message);
            await md.ShowAsync();
        }
        private void setSpeedDial()
        {
            Tiles.ItemsSource = SpeedDialManager.GetSpeedDial();
        }

        void item_Tapped(object sender, TappedRoutedEventArgs e)
        {
            
        }

        private void Tiles_ItemClick(object sender, ItemClickEventArgs e)
        {
            if(e.ClickedItem is SpeedDialTile)
                _parent.Navigate((e.ClickedItem as SpeedDialTile).DestinationUri);
        }

        private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            SpeedDialManager.RemoveTile((sender as MenuFlyoutItem).CommandParameter as SpeedDialTile);
            Tiles.ItemsSource = SpeedDialManager.GetSpeedDial();
        }

        private void Grid_Holding(object sender, HoldingRoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout(sender as FrameworkElement);
        }

        private void Grid_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout(sender as FrameworkElement);
        }

        private void TextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter && !string.IsNullOrEmpty((sender as TextBox).Text))
            {
                _parent.Navigate(new Uri(string.Format("http://www.google.com/search?q={0}", (sender as TextBox).Text)));
            }
        }
    }
    public class SpeedDialTile
    {
        public Uri ImageSource { get; set; }
        public Uri DestinationUri { get; set; }
        public string Name { get; set; }
    }
    public class SpeedDialManager
    {
        static Windows.Storage.ApplicationDataContainer settings = Windows.Storage.ApplicationData.Current.LocalSettings;


        public static List<SpeedDialTile> GetSpeedDial()
        {
            try
            {
                var tiles = settings.Values["SpeedDialTiles"];
                if (tiles == null)
                {
                    settings.Values["SpeedDialTiles"] = "[{\"ImageSource\":\"ms-appx:///Html/img/YouTube_logo.png\",\"DestinationUri\":\"http://www.youtube.com\",\"Name\":\"YouTube\"},{\"ImageSource\":\"ms-appx:///Html/img/Google-logo.jpg\",\"DestinationUri\":\"http://www.google.com\",\"Name\":\"Google\"},{\"ImageSource\":\"ms-appx:///Html/img/facebook-logo.jpg\",\"DestinationUri\":\"http://www.facebook.com\",\"Name\":\"Facebook\"},{\"ImageSource\":\"ms-appx:///Html/img/twitter.png\",\"DestinationUri\":\"http://www.twitter.com\",\"Name\":\"Twitter\"},{\"ImageSource\":\"ms-appx:///Html/img/vimeo_logo.jpg\",\"DestinationUri\":\"http://www.vimeo.com\",\"Name\":\"Vimeo\"},{\"ImageSource\":\"ms-appx:///Html/img/dailymotion-logo.png\",\"DestinationUri\":\"http://www.dailymotion.com\",\"Name\":\"Dailymotion\"},{\"ImageSource\":\"ms-appx:///Html/img/yahoo.png\",\"DestinationUri\":\"http://www.yahoo.com\",\"Name\":\"Yahoo!\"},{\"ImageSource\":\"ms-appx:///Html/img/Bing-logo.png\",\"DestinationUri\":\"http://www.bing.com\",\"Name\":\"Bing\"},{\"ImageSource\":\"ms-appx:///Html/img/wiki-logo.png\",\"DestinationUri\":\"http://www.wikipedia.org\",\"Name\":\"Wikipedia\"}]";
                    tiles = settings.Values["SpeedDialTiles"];
                }
                return Newtonsoft.Json.JsonConvert.DeserializeObject<List<SpeedDialTile>>(tiles.ToString());
            }
            catch { return null; }
        }
        public static void AddTile(SpeedDialTile tile)
        {
            try
            {
                var tiles = settings.Values["SpeedDialTiles"];
                
                var listTiles = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SpeedDialTile>>(tiles.ToString());
                listTiles.Add(tile);
                settings.Values["SpeedDialTiles"] = Newtonsoft.Json.JsonConvert.SerializeObject(listTiles);
            }
            catch { }
        }
        public async static void AddTile(InMemoryRandomAccessStream stream, Uri url, string Title)
        {

            Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            string filename = string.Format("Thumb{0}.png", DateTime.Now.ToString("dMyyyyHmsffffff"));
            StorageFile file = await localFolder.CreateFileAsync(filename);


            var fileStream = await file.OpenAsync(FileAccessMode.ReadWrite);
            await RandomAccessStream.CopyAndCloseAsync(stream.GetInputStreamAt(0),  fileStream.GetOutputStreamAt(0));

            System.Diagnostics.Debug.WriteLine("foi/n/n");

            SpeedDialTile tile = new SpeedDialTile();
            tile.DestinationUri = url;
            tile.ImageSource = new Uri("ms-appdata:///local/" + filename);
            tile.Name = Title;
            AddTile(tile);
        }
        public static void RemoveTile(SpeedDialTile tile)
        {
            try
            {
                var tiles = settings.Values["SpeedDialTiles"];
                var listTiles = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SpeedDialTile>>(tiles.ToString());
                listTiles.Remove(listTiles.Where(p => p.DestinationUri == tile.DestinationUri).First());
                settings.Values["SpeedDialTiles"] = Newtonsoft.Json.JsonConvert.SerializeObject(listTiles);
            }
            catch { }
        }
    }
}
