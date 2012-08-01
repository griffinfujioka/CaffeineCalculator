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
using System.Text;
using System.Text.RegularExpressions;


namespace Caffeine_Calculator
{
    public partial class CustomLimitPage : PhoneApplicationPage
    {

        List<Tolerance> toleranceList = new List<Tolerance>();      // List of tolerances a user can choose from 

        #region Page constructor 
        public CustomLimitPage()
        {
            InitializeComponent();
            initializeToleranceList();                      // Initialize list of available tolerances 
            this.defaultPicker.SelectedIndex = -1;          // nothing selected initally 
            this.defaultPicker.ItemsSource = toleranceList;     // Bind the picker to the tolerance list 
            this.defaultPicker.SelectionChanged += new SelectionChangedEventHandler(defaultPicker_SelectionChanged);
        }
        #endregion 

        #region Initialize the list of available tolerances
        public void initializeToleranceList()
        {
          
            toleranceList.Add(new Tolerance() { LevelName="Low - 250 mg", Content = 250});           // Low: 250 mg
            toleranceList.Add(new Tolerance() { LevelName = "Medium - 500 mg", Content = 500 });     // Medium: 500 mg 
            toleranceList.Add(new Tolerance() { LevelName = "High - 750 mg", Content = 750 });       // High: 750 mg 
            toleranceList.Add(new Tolerance() { LevelName = "Very High - 1000 mg", Content = 1000 }); // Very High: 1000 mg 
        }
        #endregion 

        #region Submit button click 
        private void submitButton_Click(object sender, RoutedEventArgs e)
        {
          
            if (desiredLimitTxtBox.Text == "" && defaultPicker.SelectedIndex != -1)     // if nothing is entered in the textbox but a tolerance is selected 
            {                                                                                   
                GlobalVars.userDefinedLimit = true;                             
                GlobalVars.desiredDailyLimit = (defaultPicker.SelectedItem as Tolerance).Content; // Set desired daily limit to that of the selected tolerance 
                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            }
            else if (desiredLimitTxtBox.Text != "")                                 // if something is entered in the textbox use that as the desired daily limit                         
            {
                #region If the text is a valid number, use that 
                if (IsTextValidated(desiredLimitTxtBox.Text))           // check if it's numeric 
                {
                    GlobalVars.userDefinedLimit = true;
                    GlobalVars.desiredDailyLimit = Convert.ToInt32(desiredLimitTxtBox.Text);

                    NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
                }
                #endregion 
                else                                        // Stay on the page and get a valid numberic value 
                {
                    MessageBoxResult m = MessageBox.Show("Please enter a numeric value.");
                    desiredLimitTxtBox.Text = "";
                }
            }
            
        }
        #endregion 

        #region Helper function to check if text is numeric. Equivalent of VB isNumeric function
        private bool IsTextValidated(string strTextEntry)
        {
            Regex objNotWholePattern = new Regex("[^0-9]");
            return !objNotWholePattern.IsMatch(strTextEntry) && (strTextEntry != "");      
        }
        #endregion 

        #region Default picker selection changed 
        private void defaultPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // This function does nothing, do I even need to define it?
        }
        #endregion 

    }
}