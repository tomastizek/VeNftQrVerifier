using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using VeNftQrVerifier.ViewModel;
using Xamarin.Forms.Xaml;

namespace VeNftQrVerifier.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {

        MainPageVM mainPageVM;

        public MainPage()
        {
            InitializeComponent();
            mainPageVM = new MainPageVM();
            BindingContext = mainPageVM;
        }
    }
}
