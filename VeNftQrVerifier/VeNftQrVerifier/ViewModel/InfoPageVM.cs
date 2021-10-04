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
using VeNftQrVerifier.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace VeNftQrVerifier.ViewModel
{
    public class InfoPageVM : INotifyPropertyChanged
    {

        private OwnershipVerificationCodeDto _dto;
        private string _nFTName;
        private TicketNFT _loadedNFT;
        private Color _ticketBackgroundColor;
        private ImageSource _stateOfTicketImage;
        private string _stateOfTicket;
        private VerifyNFTTicketDto _verifiedTicket;

        private string eventId = "848ed3dc2847dc8aaad544ef93c5e474b9720c63e0c323dc4c136942ce25c30b";
        private string mintingAddress = "NgUzHT14rAs8ft3hanuV2do8nTAtHzSLvJ";

        public VerifyNFTTicketDto VerifiedTicket
        {
            get { return _verifiedTicket; }
            set
            {
                _verifiedTicket = value;
                OnPropertyChanged("VerifiedTicket");

                if (VerifiedTicket != null && VerifiedTicket.IsSignatureValid)
                {
                    Task.Run(async () =>
                    {
                        try
                        {   
                            LoadedNFT = await GetNFTAsync(Dto.TxId);
                            
                        }
                        catch (System.Exception ex)
                        {
                            System.Console.WriteLine(ex.Message);
                        }


                    });

                    Uri imageUri = new Uri("https://www.shareicon.net/data/256x256/2016/08/20/817720_check_395x512.png");
                    StateOfTicketImage = ImageSource.FromUri(imageUri);
                    StateOfTicket = "Ticket is valid!";

                }
                else
                {
                    Uri imageUri = new Uri("https://www.shareicon.net/data/256x256/2016/08/20/817726_close_395x512.png");
                    StateOfTicketImage = ImageSource.FromUri(imageUri);
                    StateOfTicket = "Ticket not used or allready expired!";
                }

            }
        }

        public TicketNFT LoadedNFT
        {
            get { return _loadedNFT; }
            set
            {
                _loadedNFT = value;
                OnPropertyChanged("LoadedNFT");
                NFTName = LoadedNFT.Name;
               



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


        public OwnershipVerificationCodeDto Dto
        {
            get { return _dto; }
            set
            {
                _dto = value;
                OnPropertyChanged("Dto");

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


        public InfoPageVM(OwnershipVerificationCodeDto dto)
        {


            if (CrossConnectivity.Current.IsConnected)
            {
                Dto = dto;

                Task.Run(async () => 
                {
                    try
                    {
                        VerifiedTicket = await NFTTicketVerifier.LoadNFTTicketToVerify(dto, eventId, new List<string> { mintingAddress });
                    }
                    catch (System.Exception ex)
                    {
                        System.Console.WriteLine(ex.Message);
                    }

                
                });
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
