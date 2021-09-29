using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using VEDriversLite.NFT;
using VeNftQrVerifier.Services;
using Rg.Plugins.Popup.Services;
using VeNftQrVerifier.Views;
using System.Diagnostics;
using Newtonsoft.Json;

namespace VeNftQrVerifier.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QrScanPage : ContentPage
    {
      

        public QrScanPage()
        {
            InitializeComponent();
            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
        }

        /// <summary>
        /// Cancel scanning process.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void CancelButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
        /// <summary>
        /// Control button for flashlight
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FlashlightButton_Clicked(object sender, EventArgs e)
        {
            Camera.TorchOn = !Camera.TorchOn;
        }

        /// <summary>
        /// Main method for scanning of qr code
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void CameraView_OnDetected(object sender, GoogleVisionBarCodeScanner.OnDetectedEventArg e)
        {
            string txId = string.Empty;

            List<GoogleVisionBarCodeScanner.BarcodeResult> obj = e.BarcodeResults;

            string result = string.Empty;
            for (int i = 0; i < obj.Count; i++)
            {
                result += $"{obj[i].DisplayValue}{Environment.NewLine}";

                
                //Validation of Qr code.
                if (NftQr.ValidateNftFromQr(result).Result)
                {

                    await PopupNavigation.Instance.PushAsync(new BusyPopupPage());

                    try
                    {
                        txId = JsonConvert.DeserializeObject<NftQr>(result).TxId;

                        await App.Current.MainPage.Navigation.PushModalAsync(new InfoPage(txId));
                    }
                    finally
                    {
                        await PopupNavigation.Instance.PopAsync();
                    }
                }
                else
                {

                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        if (!await App.Current.MainPage.DisplayAlert("Invalid Qr code", "Qr code does not cotain valid NFT address!", null, "OK"))
                        {
                            await Navigation.PopModalAsync();
                            
                            

                        }


                    });
                }




                }
        }
    }
}