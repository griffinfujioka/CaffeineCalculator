using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Drinks; 

namespace Caffeine_Calculator
{
    #region Global variables to be used throughout the application 
    public static class GlobalVars
    {
        public static bool userDefinedLimit = false;        // Bool helper variable: true if user defined their own caffeine limit (actual number or tolerance)
        public static int desiredDailyLimit;                // Daily caffeine limit    
        public static bool DBHasBeenPopulated = false;           
    }
    #endregion 

    public partial class MainPage : PhoneApplicationPage, INotifyPropertyChanged
    {
        List<Drink> drinkList = new List<Drink>();                   // List of all available drinks 
        ObservableCollection<Drink> todayList = new ObservableCollection<Drink>();      // List of consumed drinks 
        int caffeineMG = 0;             // Caffeine mg counter 
        int caffeineDailyLimit = 500;       // TO-DO: Figure out if this is accurate
        double dailyLimitPercent = 0;

        #region MainPage constructor 
        public MainPage()
        {
            InitializeComponent();

            if(App.ViewModel.drinkList.Count == 0)
                App.ViewModel.PopulateDrinkDB(); 

            this.DataContext = App.ViewModel;
            
            drinkList = new List<Drink>(App.ViewModel.drinkList);
            
            this.defaultPicker.ItemsSource = drinkList; 
            this.defaultPicker.SelectionChanged += new SelectionChangedEventHandler(defaultPicker_SelectionChanged);
            
            outputTxtBlock.Text = "0"; 
            
        }
        #endregion 

        #region Initalize available drinks
        public void initializeDrinkList()
        {
            //drinkList.Add(new Drink() { Name = "Tap to add a drink", Size = 0, mg = 0 }); 
            //drinkList.Add(new Drink() { Name= "5 hour energy", Size=2, mg = 138});
            //drinkList.Add(new Drink() { Name = "Coca Cola classic - 12oz", Size = 12, mg = 34 });
            //drinkList.Add(new Drink() { Name = "Coca Cola zero - 12oz", Size = 12, mg = 45 });
            //drinkList.Add(new Drink() { Name = "Coca Cola diet - 12oz", Size = 12, mg = 45 }); 
            //drinkList.Add(new Drink() { Name = "Coffee - 8oz", Size = 8, mg = 108 });
            //drinkList.Add(new Drink() { Name = "Coffee - 12oz", Size = 12, mg = 108 });
            //drinkList.Add(new Drink() { Name = "Crystal Light Energy - 16oz", Size = 16, mg = 120 });
            //drinkList.Add(new Drink() { Name = "Full Throttle Energy Drink - 16oz", Size = 16, mg = 144 });
            //drinkList.Add(new Drink() { Name = "Lipton Iced Tea - 20oz", Size = 20, mg = 50 });
            //drinkList.Add(new Drink() { Name = "McDonalds Iced Coffee", Size = 16, mg = 200 });
            //drinkList.Add(new Drink() { Name = "Monster Energy Drink", Size = 16, mg = 160 });
            //drinkList.Add(new Drink() { Name = "Mountain Dew - 12oz", Size = 12, mg = 54 });
            //drinkList.Add(new Drink() { Name = "Muscle Milk Refuel 3oz", Size = 16, mg = 160 });
            //drinkList.Add(new Drink() { Name = "No Fear Energy Drink", Size = 16, mg = 182 });
            //drinkList.Add(new Drink() { Name = "NOS Energy Drink", Size = 16, mg = 260 });
            //drinkList.Add(new Drink() { Name = "Pepsi Cola - 12oz", Size = 12, mg = 38});
            //drinkList.Add(new Drink() { Name = "Red Bull - 8.4oz", Size = 9, mg = 80 });
            //drinkList.Add(new Drink() { Name = "Rockstar Energy Drink", Size = 16, mg = 160 });
            //drinkList.Add(new Drink() { Name = "Starbucks Bottled Frappucino", Size = 9, mg = 90 });
            //drinkList.Add(new Drink() { Name = "Starbucks Grande Americano", Size = 16, mg = 225 });
            //drinkList.Add(new Drink() { Name = "Starbucks Grande Latte", Size = 16, mg = 150 });
            //drinkList.Add(new Drink() { Name = "Tazo Chai (tea) - 8oz", Size = 8, mg = 45 });
        }
        #endregion 

        #region ListPicker selection changed event 
        private void defaultPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            #region Check if user defined their own limit 
            if (GlobalVars.userDefinedLimit)                            // If user defined their own limit, use that 
                caffeineDailyLimit = GlobalVars.desiredDailyLimit;
            #endregion 

            ddcTxtBlock.Text = caffeineDailyLimit.ToString() + " mg";                   // Print daily limit 
            bool TapToAdd = false;
            bool Added = false;


            if ((defaultPicker.SelectedItem as Drink).Name == "Tap to add a drink")
                return; //  TapToAdd = true;

            if ((defaultPicker.SelectedItem as Drink).Name == "------ Added Drinks ------")
                Added = true; 
            
            if (defaultPicker.SelectedIndex != -1 && !TapToAdd && !Added)        // A drink is selected and it's not the "Select a drink" option
            {
                #region Make all textblocks visible
                mgTxtBlock.Visibility = Visibility.Visible;
                textBlock2.Visibility = Visibility.Visible;
                outputTxtBlock.Visibility = Visibility.Visible;
                #endregion

                todayList.Add(defaultPicker.SelectedItem as Drink);                         // Add selected drink to todayList
                caffeineMG += (defaultPicker.SelectedItem as Drink).mg;                     // Increment mg counter 
                outputTxtBlock.Text = caffeineMG.ToString();                                // Print current caffeine mg 
                this.listBox1.ItemsSource = todayList;                                      // Bind listbox to todayList of drinks
                this.defaultPicker.SelectedIndex = 0;


                dailyLimitPercent = 100 * ((double)caffeineMG / caffeineDailyLimit);        // Calculate percentage of daily limit 
                if (dailyLimitPercent >= 100.0)
                    MessageBox.Show("Warning: You have exceeded your caffeine limit.");
                percentTxtBlock.Text = string.Format("{0:N1}", dailyLimitPercent) + "%";    // Format percent and call it 

                defaultSlider.Value = dailyLimitPercent;                                    // Update the slider 

            }
            else
            {
                defaultPicker.SelectedIndex = 0; 
                return; 
            }
        }
        #endregion 

        #region Clear caffeine calculator
        private void ApplicationBarMenuItem_Click(object sender, EventArgs e)
        {
            MessageBoxResult m = MessageBox.Show("Are you sure you want to clear the caffeine calculator?", "", MessageBoxButton.OKCancel);
            if (m == MessageBoxResult.Cancel)
            {
                return;
            } 
            todayList.Clear();          // Empty the todayList 
            caffeineMG = 0;             // Reset mg counter 

            #region Hide all textblocks 
            mgTxtBlock.Visibility = Visibility.Collapsed;
            textBlock2.Visibility = Visibility.Collapsed;
            outputTxtBlock.Visibility = Visibility.Collapsed;
            #endregion 

            defaultSlider.Value = 0;
            dailyLimitPercent = 0;
            percentTxtBlock.Text = string.Format("{0:N1}", dailyLimitPercent) + "%";
        }
        #endregion 

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify Silverlight that a property has changed.
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        #region Visit the about page
        private void ApplicationBarMenuItem_Click_1(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/AboutPage.xaml", UriKind.Relative));
        }
        #endregion 

        #region Visit the custom caffeine limit page 
        private void ApplicationBarMenuItem_Click_2(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/CustomLimitPage.xaml", UriKind.Relative));
        }
        #endregion 

        #region Add a new drink 
        private void newDrinkAppBarButton_Click(object sender, EventArgs e)
        {
          
            NavigationService.Navigate(new Uri("/NewDrink.xaml", UriKind.Relative));
        }
        #endregion 

        #region On Navigated To: Check if user customized daily limit
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (GlobalVars.userDefinedLimit)
            {
                caffeineDailyLimit = GlobalVars.desiredDailyLimit;
            }

            ddcTxtBlock.Text = caffeineDailyLimit.ToString() + " mg";
            
            this.defaultPicker.ItemsSource = App.ViewModel.drinkList;

        }
        #endregion 

        private void editDrinksBtn_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/EditDrinksPage.xaml", UriKind.Relative)); 
        }

    }


    
}