using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace RecognizeLeafs
{
    class MainPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand TapCommand => new Command<string>(async (url) => await Launcher.OpenAsync(url));



        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string takePhoto; 

        public string TakePhoto { get { return takePhoto; } set { takePhoto = value; OnPropertyChanged("TakePhoto"); } }

        private string imageSource; 

        public string ImageSource { get { return imageSource; } set { imageSource = value; OnPropertyChanged("ImageSource"); } }

        private string choose;

        public string Choose { get { return choose; } set { choose = value; OnPropertyChanged("Choose"); } }

        private string analyzeInfo;

        public string AnalyzeInfo { get { return analyzeInfo; } set { analyzeInfo = value; OnPropertyChanged("AnalyzeInfo"); } }

        public string wikipediaLink;

        public string WikipediaLink { get { return wikipediaLink; } set { wikipediaLink = value; OnPropertyChanged("WikipediaLink"); } }

        public MainPageViewModel()
        {
            TakePhoto = "Take photo";
            ImageSource = @"screenTwo.jpg";
            AnalyzeInfo = "Do you want recognize tree?";
            Choose = "Chose photo";
            WikipediaLink = ""; 
        }
    }
}
