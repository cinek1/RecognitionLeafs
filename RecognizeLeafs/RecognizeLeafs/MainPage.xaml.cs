using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RecognizeLeafs
{
    public partial class MainPage : ContentPage
    {
        private MainPageViewModel viewModel;
        private State state; 
        public MainPage()
        {
            InitializeComponent();
            viewModel = new MainPageViewModel();
            this.BindingContext = viewModel;
            state = State.Choose;
            viewModel.AnalyzeInfo = "Do you want recognize Tree?";
        }


        private async void Button_Choose_Clicked(object sender, EventArgs e)
        {
            if (state == State.Analyze)
            {
                await Analyze(); 
            }
            else
            {
                await ChooseFile(); 
            }
        }

        private async Task Analyze()
        {
            viewModel.AnalyzeInfo = "Tree: OAK \n";
            viewModel.WikipediaLink = "https://en.wikipedia.org/wiki/Oak";
            Button_Choose.IsEnabled = false; 
        }

        private void Exit()
        {
            ChangeButtonsToChoose();
            viewModel.ImageSource = ""; 
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

        private void ChangeButtonsToAnalyze()
        {
            viewModel.Choose = "Analyze";
            viewModel.TakePhoto = "Exit";
            state = State.Analyze;
            viewModel.AnalyzeInfo = "";

        }


        private void ChangeButtonsToChoose()
        {
            viewModel.Choose = "Choose photo";
            viewModel.TakePhoto = "Take photo";
            viewModel.AnalyzeInfo = "Do you want recognize Tree?";
            viewModel.WikipediaLink = "";
            state = State.Choose;
            Button_Choose.IsEnabled = true;

        }
        private async void Button_Take_Clicked(object sender, EventArgs e)
        {
            if (state == State.Analyze)
            {
                 Exit();
            }
            else
            {
                await ChooseFile();
            }
        }
    }
}
