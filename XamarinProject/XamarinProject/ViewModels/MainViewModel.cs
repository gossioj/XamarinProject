using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.Windows.Input;
using XamarinProject.Models;
using System;

namespace XamarinProject.ViewModels
{
    public class MainViewModel
    {
        // Region Poperties
        private string amount;

        public string Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        private ObservableCollection<Rate> rates;

        public ObservableCollection<Rate> Rates
        {
            get { return rates; }
            set { rates = value; }
        }

        private Rate sourceRate;

        public Rate SourceRate
        {
            get { return sourceRate; }
            set { sourceRate = value; }
        }

        private Rate targetRate;

        public Rate TargetRate
        {
            get { return targetRate; }
            set { targetRate = value; }
        }

        private bool isRunning;

        public bool IsRunning
        {
            get { return isRunning; }
            set { isRunning = value; }
        }

        private bool isEnabled;

        public bool IsEnabled
        {
            get { return isEnabled; }
            set { isEnabled = value; }
        }

        private string result;

        public string Result
        {
            get { return result; }
            set { result = value; }
        }


        // Commands
        public ICommand ConvertCommand => new RelayCommand(Convert);

        private void Convert()
        {
            throw new NotImplementedException();
        }

        public MainViewModel()
        {
            
        }        
    }
}
