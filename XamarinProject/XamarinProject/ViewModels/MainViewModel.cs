using ApiService;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinProject.Models;
using System;
using Plugin.Connectivity;

namespace XamarinProject.ViewModels
{
    public class MainViewModel: INotifyPropertyChanged
    {
        #region Events
        public event PropertyChangedEventHandler PropertyChanged;

        private void Notificar([CallerMemberName] string Propiedad = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Propiedad));
        }
        #endregion

        #region Properties

        private string statusValue;

        public string StatusValue
        {
            get { return statusValue; }
            set { statusValue = value;
                Notificar();
            }
        }


        private bool status;

        public bool Status
        {
            get { return status; }
            set { status = value;
                Notificar();
            }
        }


        public ApiServiceRest ApiService { get; set; }

        private string amount;

        public string Amount
        {
            get { return amount; }
            set
            {
                amount = value;
                Notificar();
            }
        }

        private ObservableCollection<Rate> rates;

        public ObservableCollection<Rate> Rates
        {
            get { return rates; }
            set
            {
                rates = value;
                Notificar();
            }
        }

        private bool isLabelSourceRateVisible;

        public bool IsLabelSourceRateVisible
        {
            get { return isLabelSourceRateVisible; }
            set
            {
                isLabelSourceRateVisible = value;
                Notificar();
            }
        }


        private Rate sourceRate;

        public Rate SourceRate
        {
            get { return sourceRate; }
            set
            {
                sourceRate = value;
                if (sourceRate != null)
                {
                    this.IsLabelSourceRateVisible = true;
                    this.IsGridVisibleTaxRate = true;
                }
                else
                {
                    this.IsLabelSourceRateVisible = false;
                    if (this.TargetRate == null)
                    {
                        this.IsGridVisibleTaxRate = false;
                    }
                }

                Notificar();
            }
        }


        private string flagSourcetUrl;

        public string FlagSourcetUrl
        {
            get { return flagSourcetUrl; }
            set
            {
                flagSourcetUrl = value;
                Notificar();
            }
        }

        private string flagTargetUrl;

        public string FlagTargetUrl
        {
            get { return flagTargetUrl; }
            set { flagTargetUrl = value; }
        }

        private bool isLabelTargetRateVisible;

        public bool IsLabelTargetRateVisible
        {
            get { return isLabelTargetRateVisible; }
            set
            {
                isLabelTargetRateVisible = value;
                Notificar();
            }
        }

        private Rate targetRate;

        public Rate TargetRate
        {
            get { return targetRate; }
            set
            {
                targetRate = value;
                if (targetRate != null)
                {
                    this.IsLabelTargetRateVisible = true;
                    this.IsGridVisibleTaxRate = true;
                }
                else
                {
                    this.IsLabelTargetRateVisible = false;
                    if (this.SourceRate == null)
                    {
                        this.IsGridVisibleTaxRate = false;
                    }
                }

                Notificar();
            }
        }

        private bool isRunning;

        public bool IsRunning
        {
            get { return isRunning; }
            set
            {
                isRunning = value;
                Notificar();
            }
        }

        private bool isEnabled;

        public bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                isEnabled = value;
                Notificar();
            }
        }

        private string result;



        public string Result
        {
            get { return result; }
            set
            {
                result = value;
                Notificar();
            }
        }

        private List<Country> countriesAsync;

        public List<Country> CountriesAsync
        {
            get { return countriesAsync; }
            set
            {
                countriesAsync = value;
                Notificar();
            }
        }

        private bool isGridVisibleTaxRate;

        public bool IsGridVisibleTaxRate
        {
            get { return isGridVisibleTaxRate; }
            set
            {
                isGridVisibleTaxRate = value;
                Notificar();
            }
        }
        #endregion

        #region Commands
        public ICommand UpdateCommand => new RelayCommand(Update);
        public ICommand ConvertCommand => new RelayCommand(Convert);
        public ICommand SwitchCommand => new RelayCommand(Switch);
        #endregion

        #region Methods
        private void Update()
        {
            // System.Threading.Thread.Sleep(2000);            
            ConnectionWifi();
            LoadRate();
            /* await Application.Current.MainPage.DisplayAlert(
                     "Updated",
                     "Tax Rates have been updated.",
                     "Ok");*/
        }

        private void Switch()
        {
            var aux = this.SourceRate;
            this.SourceRate = this.TargetRate;
            this.TargetRate = aux;
            //Convert();
        }

        private async void Convert()
        {
            if (string.IsNullOrEmpty(this.Amount))
            {
                await Application.Current.MainPage.DisplayAlert("Oops", "You must enter a value in amount", "Ok");
                return;
            }

            decimal amountLocal = 0;
            if (!decimal.TryParse(this.Amount, out amountLocal))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Oops",
                    "You must enter a numeric value in amount",
                    "Ok");
                return;
            }

            if (this.SourceRate == null)
            {
                await Application.Current.MainPage.DisplayAlert("Oops", "You must select a source rate", "Ok");
                return;
            }

            if (this.TargetRate == null)
            {
                await Application.Current.MainPage.DisplayAlert("Oops", "You must select a target rate", "Ok");
                return;
            }

            SetFlag();

            var amountConverted = amountLocal /
                (decimal)this.SourceRate.TaxRate *
                (decimal)this.TargetRate.TaxRate;

            this.Result = $"{this.SourceRate.Code} {amountLocal:C2} = {this.TargetRate.Code} {amountConverted:C2}";
        }

        void SetFlag()
        {

            foreach (var itemCountry in this.CountriesAsync)
            {
                foreach (var itemCurrency in itemCountry.currencies)
                {
                    if (itemCurrency.code == this.SourceRate.Code)
                    {
                        this.FlagSourcetUrl = itemCountry.flag;
                    }

                    if (itemCurrency.code == this.TargetRate.Code)
                    {
                        this.FlagTargetUrl = itemCountry.flag;
                    }
                }
            }
        }

        public async void LoadFlag()
        {
            if (!this.Status)
            {
                return;
            }

            this.CountriesAsync = new List<Country>();
            var ratesAsync = await this.ApiService.GetListRest<Country>("http://restcountries.eu", "/rest/v2/all");
            this.CountriesAsync = ratesAsync.Result as List<Country>;
        }

        public async void LoadRate()
        {
            this.StatusValue = string.Empty;
            if (!this.Status)
            {
                return;
            }

            this.IsRunning = true;
            this.Result = "Loading rates...";
            var ratesAsync = await this.ApiService.GetListRest<Rate>("http://apiexchangerates.azurewebsites.net", "/api/rates");
            this.Rates = new ObservableCollection<Rate>(ratesAsync.Result as List<Rate>);
            this.IsRunning = false;
            this.Result = ratesAsync.IsSuccesful ? "Ready to Convert" : ratesAsync.Message;
            this.IsEnabled = true;
            this.StatusValue = "Rate loaded from internet";
        }
        #endregion

        #region Constructor
        public MainViewModel()
        {
            this.IsLabelTargetRateVisible = false;
            this.IsLabelSourceRateVisible = false;
            this.IsRunning = false;
            this.IsGridVisibleTaxRate = false;           
            this.StatusValue = string.Empty;
            this.ApiService = new ApiServiceRest();
            ConnectionWifi();           
            LoadRate();
            LoadFlag();
        }

        private async void ConnectionWifi()
        {
            this.Status = true;
            this.IsRunning = true;
            if (!CrossConnectivity.Current.IsConnected)
            {
                this.Status = CrossConnectivity.Current.IsConnected;
                this.Result = $"Check your Internet settings";
                this.IsRunning = false;
                return;
            }

            bool hasInternet = await CrossConnectivity.Current.IsReachable("http://www.google.com", 5000);
            if (!hasInternet)
            {
                this.Status = hasInternet;
                this.Result = $"Check your Internet connection";
                this.IsRunning = false;
                return;
            }           
        }
        #endregion
    }
}
