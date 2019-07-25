using NotificationsExtensions.ToastContent;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Core;
using Windows.UI.Notifications;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace V_Browser
{
    public sealed partial class File : UserControl
    {
        public File(System.Uri Url, string name, string ext)
        {
            this.InitializeComponent();
            this.Url = Url;
            StartDownload(Url,name,ext);
        }
        ResourceLoader LanguageLoader = new Windows.ApplicationModel.Resources.ResourceLoader();
        public System.Uri Url { get; set; }


        #region Download
        StorageFile filedownloaded;
        BackgroundDownloader downloader = new BackgroundDownloader();
        DownloadOperation download;
        bool finished = false;
        private async void StartDownload(System.Uri uri, string name, string ext)
        {
            try
            {

                await Task.Delay(500);
                // set download URI
                // get destination file
                var picker = new FileSavePicker();
                // set allowed extensions
                picker.SuggestedFileName = name;
                picker.SuggestedStartLocation = PickerLocationId.VideosLibrary;
                picker.FileTypeChoices.Add(ext, new List<string> {"."+ ext });
                var file = await picker.PickSaveFileAsync();
                filedownloaded = file;
                filename.Text = file.DisplayName;

                if (file != null)
                {
                    ToastNotification success = CreateToastText02(LanguageLoader.GetString("DownloadSuccess"),LanguageLoader.GetString("DownloadSuccessMessage"));
                    success.Activated += toast_Activated;
                    downloader.SuccessToastNotification = success;

                    downloader.FailureToastNotification = CreateToastText02(LanguageLoader.GetString("DownloadError"),LanguageLoader.GetString("DownloadErrorMessage"));
                                        
                    // create a background download
                    download = downloader.CreateDownload(uri, file);
                    
                    // create progress object
                    var progress = new Progress<DownloadOperation>();                    
                    // attach an event handler to get notified on progress
                    progress.ProgressChanged += (o, operation) =>
                    {
                        // use the progress info in Progress.BytesReceived and Progress.TotalBytesToReceive
                        DownloadProgress(operation);
                    };
                    await download.StartAsync().AsTask(progress);
                    

                    ProgressBar1.Value = 0;
                    finished = true;
                    btnCancel.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    btnPause.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    btnOpen.Visibility = Windows.UI.Xaml.Visibility.Visible;
                }
            }
            catch (Exception e)
            {
                ShowMessage(LanguageLoader.GetString("DownloadErrorMessage"));
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }
        }
        private void DownloadProgress(DownloadOperation download)
        {            
            double percent = 100;
            if (download.Progress.TotalBytesToReceive > 0)
            {
                percent = download.Progress.BytesReceived * 100 / download.Progress.TotalBytesToReceive;
            }
            ProgressBar1.Value = percent;
        }

        private ToastNotification CreateToastText02(string title, string content)
        {
            IToastText02 toastContent = ToastContentFactory.CreateToastText02();

            // Set the launch activation context parameter on the toast.
            // The context can be recovered through the app Activation event
            toastContent.Launch = "Context123";

            toastContent.TextHeading.Text = title;
            toastContent.TextBodyWrap.Text = content;

            return toastContent.CreateNotification();
        }
        private async void toast_Activated(ToastNotification sender, object args)
        {
            await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                LaunchFileAsync();
            });
        }
        private async void LaunchFileAsync()
        {
            try
            {
                var options = new Windows.System.LauncherOptions();
                options.DisplayApplicationPicker = true;
                await Windows.System.Launcher.LaunchFileAsync(filedownloaded, options);
            }
            catch
            {
                ShowMessage(LanguageLoader.GetString("LaunchFileError"));
            }
        }
        private async void ShowMessage(string message)
        {
            MessageDialog md = new MessageDialog(message);
            await md.ShowAsync();
        }

        #endregion

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;

            if ((string)b.Content == "Pause")
            {
                download.Pause();
                b.Content = "Resume";
            }
            else
            {
                download.Resume();
                b.Content = "Pause";
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            download.Pause();
            ProgressBar1.Value = 0;
        }

        private void Border_Tapped(object sender, TappedRoutedEventArgs e)
        {
            
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            if (filedownloaded != null && finished)
            {
                LaunchFileAsync();
            }
        }
    }
}
