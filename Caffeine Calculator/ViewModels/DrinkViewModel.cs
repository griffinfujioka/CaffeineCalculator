using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Navigation; 

using System.Linq;
using System.IO;
using Drinks;

namespace Caffeine_Calculator
{
    public class DrinkViewModel : INotifyPropertyChanged
    {
        private DrinkDataContext DrinkDB;

        #region Constructor 
        public DrinkViewModel(string DBConnectionString)
        {
            DrinkDB = new DrinkDataContext(DBConnectionString);
        }
        #endregion 

        public void SaveChangesToDB()
        {
            DrinkDB.SubmitChanges(); 
        }

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

        private List<Drink> _drinkList = new List<Drink>(); 
        public List<Drink> drinkList
        {
            get { return _drinkList; }
            set
            {
                _drinkList = value;
                NotifyPropertyChanged("_drinkList"); 
            }
        }

        // Query database and load the collections and list used by the pivot pages.
        public void LoadCollectionsFromDatabase()
        {
            // Specify the query for all to-do items in the database.
            var DrinksInDB = from Drink drink in DrinkDB.Drinks select drink;
            
            // Query the database and load drinks
            drinkList = new List<Drink>(DrinksInDB); 

        }

        public void AddDrink(Drink newDrink)
        {
            // Add to the data context 
            DrinkDB.Drinks.InsertOnSubmit(newDrink); 

            // Save changes to database 
            DrinkDB.SubmitChanges();
 
            // Add to the drink list 
            //drinkList.Add(newDrink);
            drinkList.Insert(1, newDrink); 
            App.ViewModel.SaveChangesToDB(); 

        }


        public void DeleteDrink(Drink drink)
        {
            drinkList.Remove(drink);

            DrinkDB.Drinks.DeleteOnSubmit(drink);

            DrinkDB.SubmitChanges(); 
        }
        public void DeleteWish()
        {
            var drinkForDelete = new Drink();  ; // = this.SelectedDrink

            // Remove from the list 
            drinkList.Remove(drinkForDelete); 

            // Remove from the data context 
            DrinkDB.Drinks.DeleteOnSubmit(drinkForDelete); 

            // Save the database 
            DrinkDB.SubmitChanges(); 
        }

        public void UpdateTolerance(int newTolerance)
        {

        }
        public void PopulateDrinkDB()
        {
            List<Drink> originalDrinkList = new List<Drink>(); 
            if (!GlobalVars.DBHasBeenPopulated)
            {
                originalDrinkList.Add(new Drink() { Name = "Tap to add a drink", Size = 0, mg = 0, IsActive=true});
                originalDrinkList.Add(new Drink() { Name = "5 hour energy", Size = 2, mg = 138, IsActive = true });
                originalDrinkList.Add(new Drink() { Name = "Coca Cola classic - 12oz", Size = 12, mg = 34, IsActive = true });
                originalDrinkList.Add(new Drink() { Name = "Coca Cola zero - 12oz", Size = 12, mg = 45, IsActive = true });
                originalDrinkList.Add(new Drink() { Name = "Coca Cola diet - 12oz", Size = 12, mg = 45, IsActive = true });
                originalDrinkList.Add(new Drink() { Name = "Coffee - 8oz", Size = 8, mg = 108, IsActive = true });
                originalDrinkList.Add(new Drink() { Name = "Coffee - 12oz", Size = 12, mg = 108, IsActive = true });
                originalDrinkList.Add(new Drink() { Name = "Crystal Light Energy - 16oz", Size = 16, mg = 120, IsActive = true });
                originalDrinkList.Add(new Drink() { Name = "Full Throttle Energy Drink - 16oz", Size = 16, mg = 144, IsActive = true });
                originalDrinkList.Add(new Drink() { Name = "Lipton Iced Tea - 20oz", Size = 20, mg = 50, IsActive = true });
                originalDrinkList.Add(new Drink() { Name = "McDonalds Iced Coffee", Size = 16, mg = 200, IsActive = true });
                originalDrinkList.Add(new Drink() { Name = "Monster Energy Drink", Size = 16, mg = 160, IsActive = true });
                originalDrinkList.Add(new Drink() { Name = "Mountain Dew - 12oz", Size = 12, mg = 54, IsActive = true });
                originalDrinkList.Add(new Drink() { Name = "Muscle Milk Refuel 3oz", Size = 16, mg = 160, IsActive = true });
                originalDrinkList.Add(new Drink() { Name = "No Fear Energy Drink", Size = 16, mg = 182, IsActive = true });
                originalDrinkList.Add(new Drink() { Name = "NOS Energy Drink", Size = 16, mg = 260, IsActive = true });
                originalDrinkList.Add(new Drink() { Name = "Pepsi Cola - 12oz", Size = 12, mg = 38, IsActive = true });
                originalDrinkList.Add(new Drink() { Name = "Red Bull - 8.4oz", Size = 9, mg = 80, IsActive = true });
                originalDrinkList.Add(new Drink() { Name = "Rockstar Energy Drink", Size = 16, mg = 160, IsActive = true });
                originalDrinkList.Add(new Drink() { Name = "Starbucks Bottled Frappucino", Size = 9, mg = 90, IsActive = true });
                originalDrinkList.Add(new Drink() { Name = "Starbucks Grande Americano", Size = 16, mg = 225, IsActive = true });
                originalDrinkList.Add(new Drink() { Name = "Starbucks Grande Latte", Size = 16, mg = 150, IsActive = true });
                originalDrinkList.Add(new Drink() { Name = "Tazo Chai (tea) - 8oz", Size = 8, mg = 45, IsActive = true });
                //originalDrinkList.Add(new Drink() { Name = "------ Added Drinks ------", Size = 0, mg = 0, IsActive = true });

                foreach (Drink drink in originalDrinkList)
                {
                    if(!DrinkDB.Drinks.Contains<Drink>(drink))
                        AddDrink(drink); 
                }

                DrinkDB.SubmitChanges();
            }

            GlobalVars.DBHasBeenPopulated = true; 
            
        } 

    }
}