using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEDriversLite.NFT;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using VeNftQrVerifier.ViewModel;

namespace VeNftQrVerifier.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InfoPage : ContentPage
    {
        InfoPageVM infoPageVM;

        public InfoPage(OwnershipVerificationCodeDto Dto)
        {
            InitializeComponent();
            infoPageVM = new InfoPageVM(Dto);
            BindingContext = infoPageVM;

        }

        protected override bool OnBackButtonPressed()
        {
            App.Current.MainPage.Navigation.PushModalAsync(new QrScanPage());
            return true;
        }
    }
}