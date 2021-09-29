using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.IO;
using VeNftQrVerifier.View;
using System.Threading.Tasks;

namespace VeNftQrVerifier.ViewModel
{
    public class MainPageVM : INotifyPropertyChanged
    {

        public MainPageVM()
        { 
        
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }

        public async Task ScanQrCode()
        {
            bool allowed = false;
            allowed = await GoogleVisionBarCodeScanner.Methods.AskForRequiredPermission();
            if (allowed)
                await App.Current.MainPage.Navigation.PushModalAsync(new QrScanPage());
            else App.Current.MainPage.DisplayAlert("Alert", "You have to provide Camera permission", "Ok");
        }


        public Command VerifyCommand
        {
            get
            {
                return new Command(async() =>
                {
                    await ScanQrCode();
                });
            }
        }
    }
}
