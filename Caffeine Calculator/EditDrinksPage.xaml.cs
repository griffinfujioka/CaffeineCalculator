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
using Caffeine_Calculator;
using Drinks; 

namespace Caffeine_Calculator
{
    public partial class EditDrinksPage : PhoneApplicationPage
    {

        List<Drink> deleteList = new List<Drink>(); 
        public EditDrinksPage()
        {
            InitializeComponent();
            DataContext = App.ViewModel;

            Drink TapToAdd = new Drink() { Name = "Tap to add a drink", Size = 0, mg = 0, drinkId = 1, IsActive = false };
            Drink AddedDrinks = new Drink() { Name = "------ Added Drinks ------", Size = 0, mg = 0 };

            var drinkList = new List<Drink>(App.ViewModel.drinkList); 

            int i;
            for (i = 0; i < drinkList.Count; i++)
            {
                if ((drinkList[i] as Drink).Name == "Tap to add a drink")
                {
                    var drink = (drinkList[i] as Drink);

                    if (drinkList.Contains(drink))
                        drinkList.Remove(drink); 
                }
                else if ((drinkList[i] as Drink).Name == "------ Added Drinks ------")
                {
                    var drink = (drinkList[i] as Drink);

                    if (drinkList.Contains(drink))
                        drinkList.Remove(drink); 
                }
            }
            drinkList.Remove(TapToAdd);

            if (drinkList.Contains(AddedDrinks))
                drinkList.Remove(AddedDrinks); 


            drinksListBox.ItemsSource = drinkList; 
        }

        private void deleteSelectedBtn_Click(object sender, EventArgs e)
        {

            var tempDrinks = drinksListBox.ItemsSource;
            int size = deleteList.Count; 
            int i = 0; 
            for(; i < size; i++) 
            {
                var drink = (deleteList[i] as Drink);

                if (drinksListBox.Items.Contains(drink))
                {
                    (DataContext as DrinkViewModel).DeleteDrink(drink);
                }


                //drinksListBox.ItemsSource = (DataContext as DrinkViewModel).drinkList; 

                //var tempDrinks2 = drinksListBox.Items;

                
            }

            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative)); 

            //if (NavigationService.CanGoBack)
            //    NavigationService.GoBack(); 
        }

        private void checkBox_Checked(object sender, RoutedEventArgs e)
        {
            ListBoxItem checkedItem = this.drinksListBox.ItemContainerGenerator.ContainerFromItem((sender as CheckBox).DataContext) as ListBoxItem;

            var checkedDrink = (checkedItem.DataContext as Drink);
            if (checkedDrink.Name == "Tap to add a drink")
            {
                return;
            }

            if (checkedDrink.Name == "------ Added Drinks ------")
                return; 


            deleteList.Add(checkedDrink); 
        }

        private void checkBox_Unchecked(object sender, RoutedEventArgs e)
        {
            ListBoxItem checkedItem = this.drinksListBox.ItemContainerGenerator.ContainerFromItem((sender as CheckBox).DataContext) as ListBoxItem;
            if (checkedItem != null)
            {
                checkedItem.IsSelected = false; 
            }
            
        }
    }
}