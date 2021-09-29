using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using VEDriversLite.NFT;
using VeNftQrVerifier.View;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace VeNftQrVerifier.ViewModel
{
    public class InfoPageVM : INotifyPropertyChanged
    {

        private string _qrLoadedNFT;
        private string _nFTName;
        private TicketNFT _loadedNFT;
        private Color _ticketBackgroundColor;
        private ImageSource _stateOfTicketImage;
        private string _stateOfTicket;

      


        public TicketNFT LoadedNFT
        {
            get { return _loadedNFT; }
            set
            {
                _loadedNFT = value;
                OnPropertyChanged("LoadedNFT");
                NFTName = LoadedNFT.Name;
                Debug.WriteLine(LoadedNFT.ImageLink);

                if (LoadedNFT.Used)
                {
                    TicketBackroudColor = Color.Red;
                    Uri imageUri = new Uri("https://www.shareicon.net/data/256x256/2016/08/20/817726_close_395x512.png");
                    StateOfTicketImage = ImageSource.FromUri(imageUri);
                    StateOfTicket = "Ticket allready used!";
                }
                else
                {
                    TicketBackroudColor = Color.Green;
                    Uri imageUri = new Uri("https://www.shareicon.net/data/256x256/2016/08/20/817720_check_395x512.png");
                    StateOfTicketImage = ImageSource.FromUri(imageUri);
                    StateOfTicket = "Ticket is valid!";
                }



            }
        }

        public Color TicketBackroudColor
        {
            get { return _ticketBackgroundColor; }
            set
            {
                _ticketBackgroundColor = value;
                OnPropertyChanged("TicketBackroudColor");

            }
        }

        public ImageSource StateOfTicketImage
        {
            get { return _stateOfTicketImage; }
            set
            {
                _stateOfTicketImage = value;
                OnPropertyChanged("StateOfTicketImage");

            }
        }

        public string StateOfTicket
        {
            get { return _stateOfTicket; }
            set
            {
                _stateOfTicket = value;
                OnPropertyChanged("StateTicket");

            }
        }

        public string NFTName
        {
            get { return _nFTName; }
            set
            {
                _nFTName = value;
                OnPropertyChanged("NFTName");

            }
        }


        public string QrLoadedNft
        {
            get { return _qrLoadedNFT; }
            set
            {
                _qrLoadedNFT = value;
                OnPropertyChanged("QrLoadedNft");




            }
        }

        public Command ScanNextTicket
        {
            get
            {
                return new Command(() =>

                App.Current.MainPage.Navigation.PushModalAsync(new QrScanPage()));
            }
        }


        public InfoPageVM(string qrLoadedNFT)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                QrLoadedNft = qrLoadedNFT;

                Task.Run(async () => { LoadedNFT = await GetNFTAsync(qrLoadedNFT); });
            }
            else
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await App.Current.MainPage.DisplayAlert("Internet connection", "Internet connection is not available", "OK");



                });
            }

        }

        

        public static async Task<TicketNFT> GetNFTAsync(string txId)
        {

            try
            {
                return (TicketNFT)await NFTFactory.GetNFT("", txId, 0, 0, true);
                //await Task.Run(() => { result = NFTFactory.GetNFT("", txId, 0, 0, true).Result; });
                
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return null;
            }



            
        }

        

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }
    }
}
