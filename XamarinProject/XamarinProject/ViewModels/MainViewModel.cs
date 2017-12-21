using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.Windows.Input;
using XamarinProject.Models;
using System;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace XamarinProject.ViewModels
{
    public class MainViewModel: INotifyPropertyChanged
    {
        // Events
        public event PropertyChangedEventHandler PropertyChanged;

        private void Notificar([CallerMemberName] string Propiedad = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Propiedad));
        } 

        // Region Poperties
        private string amount;

        public string Amount
        {
            get { return amount; }
            set { amount = value;
                Notificar();
            }
        }

        private ObservableCollection<Rate> rates;

        public ObservableCollection<Rate> Rates
        {
            get { return rates; }
            set { rates = value;
                Notificar();
            }
        }

        private Rate sourceRate;

        public Rate SourceRate
        {
            get { return sourceRate; }
            set { sourceRate = value;
                Notificar();
            }
        }

        private Rate targetRate;

        public Rate TargetRate
        {
            get { return targetRate; }
            set { targetRate = value;
                Notificar();
            }
        }

        private bool isRunning;

        public bool IsRunning
        {
            get { return isRunning; }
            set { isRunning = value;
                Notificar();
            }
        }

        private bool isEnabled;

        public bool IsEnabled
        {
            get { return isEnabled; }
            set { isEnabled = value;
                Notificar();
            }
        }

        private string result;

       

        public string Result
        {
            get { return result; }
            set { result = value;
                Notificar();
            }
        }


        // Commands
        public ICommand ConvertCommand => new RelayCommand(Convert);

        private async void Convert()
        {
            if (string.IsNullOrEmpty(this.Amount))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "You must enter a value in amount", "Ok");
                return;
            }

            decimal amountLocal = 0;
            if (!decimal.TryParse(this.Amount, out amountLocal))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "You must enter a numeric value in amount",
                    "Ok");
                return;
            }

            if (this.SourceRate == null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "You must select a source rate", "Ok");
                return;
            }

            if (this.TargetRate == null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "You must select a target rate", "Ok");
                return;
            }

            var amountConverted = amountLocal / 
                (decimal)this.SourceRate.TaxRate *
                (decimal)this.TargetRate.TaxRate;

            this.Result = $"{this.SourceRate.Code} {amountLocal:C2} = {this.TargetRate.Code} {amountConverted:C2}";
        }

        //Constructor
        public MainViewModel()
        {
            LoadRate();
        }

        public async void LoadRate()
        {
            this.IsRunning = true;
            this.Result = "Loading rates...";

            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri("http://apiexchangerates.azurewebsites.net");
                var controller = "/api/rates";
                var response = await client.GetAsync(controller);
                var resultAsync = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    this.IsRunning = false;
                    this.Result = resultAsync;
                }

                var ratesAsync = JsonConvert.DeserializeObject<List<Rate>>(resultAsync);
                this.Rates = new ObservableCollection<Rate>(ratesAsync);

                this.IsRunning = false;
                this.Result = "Ready to Convert";
                this.IsEnabled = true;
            }
            catch (Exception e)
            {
                this.IsRunning = false;
                this.Result = $"Error: {e.Message}";
            }
        }
    }
}
