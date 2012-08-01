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
using System.Text; 
using System.Text.RegularExpressions;
using Drinks; 

namespace Caffeine_Calculator
{
    public partial class NewDrink : PhoneApplicationPage
    {
        string drinkName;
        int drinkContent; 

        public NewDrink()
        {
            InitializeComponent();
            DataContext = App.ViewModel; 
        }

        private void AddDrinkButton_Click(object sender, RoutedEventArgs e)
        {
            if (drinkNameTxtBox.Text != "")
            {
                drinkName = drinkNameTxtBox.Text;
            }
            else
            {
                MessageBox.Show("Enter a drink name.");
                return; 
            }

            #region If the text is a valid number, use that 
            if (IsTextValidated(contentTxtBox.Text))           // check if it's numeric 
            {
                drinkContent = Convert.ToInt32(contentTxtBox.Text); 
            }
            else                                        // Stay on the page and get a valid numberic value 
            {
                MessageBoxResult m = MessageBox.Show("Please enter a numeric value.");
                contentTxtBox.Text = "";
                return; 
            }
            #endregion 
            
            Drink newDrink = new Drink
            {
                Name = drinkName, 
                mg = drinkContent
            };
            
            int drinkListSize = App.ViewModel.drinkList.Count;

            
            (DataContext as DrinkViewModel).AddDrink(newDrink); 
            
            // TODO: Add custom Notification Box for Yes/No 
            //MessageBox.Show("Would you like to add this drink to the default drink list?", "", MessageBoxButton.OKCancel); 

            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        #region Helper function to check if text is numeric. Equivalent of VB isNumeric function
        private bool IsTextValidated(string strTextEntry)
        {
            Regex objNotWholePattern = new Regex("[^0-9]");
            return !objNotWholePattern.IsMatch(strTextEntry) && (strTextEntry != "");
        }
        #endregion 


    }
}