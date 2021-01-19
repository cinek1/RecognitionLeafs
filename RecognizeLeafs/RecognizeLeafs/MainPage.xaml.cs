using Plugin.Media;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Newtonsoft.Json;
using Xamarin.Essentials;

namespace RecognizeLeafs
{
    public partial class MainPage : ContentPage
    {
        private MainPageViewModel viewModel;
        private State state;
        private SendImage sendImage; 
        public MainPage()
        {
            InitializeComponent();
            viewModel = new MainPageViewModel();
            this.BindingContext = viewModel;
            state = State.Choose;
            viewModel.AnalyzeInfo = "Do you want recognize Tree?";
            sendImage = new SendImage(); 
        }
        public async void GoWikipedia(object sender, EventArgs e)
        {
            if(viewModel.WikipediaLink != "")
            {
                await Browser.OpenAsync(new Uri(viewModel.WikipediaLink), BrowserLaunchMode.SystemPreferred);
            }
        }

        private async void Button_Choose_Clicked(object sender, EventArgs e)
        {
            switch (state)
            {
                case State.Analyze:
                    await Analyze();
                    break;
                case State.Choose:
                    await ChooseFile();
                    break;
                default:
                    Console.WriteLine("Default case");
                    break;
            }
        }

        
        private async Task Analyze()
        {

            var client = new HttpClient();
            wait.IsRunning = true; 
            var image = File.ReadAllBytes(viewModel.ImageSource);
            try
            {
                var response = await sendImage.SendImageToApi(image);
                var leaf = Leaf.Deserialize(response);
                viewModel.AnalyzeInfo = $"Specie: {leaf.Name}, \n Probability: {leaf.Probability}%";
                viewModel.WikipediaLink = leaf.HyperLink;
            }
            catch (HttpRequestException)
            {
                viewModel.AnalyzeInfo = $"Problem with server. Try another time";
            }
            finally
            {
                ChangeButtonsToAfterAnalyze();
                wait.IsRunning = false;
                Label_Text.IsVisible = true;
            }

        }

        private void Exit()
        {
            ChangeButtonsToChoose();
        }

        private async Task ChooseFile()
        {
            var file = await CrossMedia.Current.PickPhotoAsync();
            if(file != null)
            {
                this.viewModel.ImageSource = file.Path;
                ChangeButtonsToAnalyze(); 
            }
        }

        private async Task TakePhoto()
        {
            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium
            });
        }

        private void ChangeButtonsToAnalyze()
        {
            viewModel.Choose = "Analyze";
            viewModel.TakePhoto = "Exit";
            state = State.Analyze;
            viewModel.AnalyzeInfo = "";
            Label_Text.IsVisible = false; 

        }
        private void ChangeButtonsToAfterAnalyze()
        {
            Button_Choose.IsEnabled = false;
            state = State.AfterAnalyze; 
        }


        private void ChangeButtonsToChoose()
        {
            viewModel.Choose = "Choose photo";
            viewModel.TakePhoto = "Take photo";
            viewModel.AnalyzeInfo = "Do you want recognize Tree?";
            viewModel.WikipediaLink = "";
            viewModel.ImageSource = @"screenTwo.jpg";
            state = State.Choose;
            Button_Choose.IsEnabled = true;
            Label_Text.IsVisible = true;

        }
        private async void Button_Take_Clicked(object sender, EventArgs e)
        {
            if (state != State.Choose)
            {
                 Exit();
            }
            else
            {
                await TakePhoto();
            }
        }
    }
}
