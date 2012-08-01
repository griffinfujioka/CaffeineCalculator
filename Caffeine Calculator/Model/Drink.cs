using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using Microsoft.Devices;
using Microsoft.Phone;
using Microsoft.Phone.Tasks;
using Microsoft.Phone.Controls;
using System.IO;
using System.IO.IsolatedStorage;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Drinks
{
    [Table]
    public class Drink : INotifyPropertyChanged, INotifyPropertyChanging
    {
        #region drinkId
        private int _drinkId;

        
        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int drinkId
        {
            get { return _drinkId; }
            set
            {
                if (_drinkId != value)
                {
                    NotifyPropertyChanging("_drinkId");
                    _drinkId = value;
                    NotifyPropertyChanged("_drinkId");
                }
            }

        }

        #endregion 

        #region name
        private string _name; 
        [Column]
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    NotifyPropertyChanging("_name");
                    _name = value;
                    NotifyPropertyChanged("_name"); 
                }
            }
        }
        #endregion 

        #region size 
        private int _size; 
        [Column]
        public int Size
        {
            get { return _size; }
            set
            {
                if (_size != value)
                {
                    NotifyPropertyChanging("_size");
                    _size = value;
                    NotifyPropertyChanged("_size");
                }
            }
        }
        #endregion 

        #region mg 
        private int _mg; 
        [Column]
        public int mg
        {
            get { return _mg; }
            set
            {
                if (_mg != value)
                {
                    NotifyPropertyChanging("_mg");
                    _mg = value;
                    NotifyPropertyChanged("_mg");
                }
            }
        }
        #endregion 

        #region isActive
        // Define completion value: private field, public property, and database column.
        private bool _isActive;

        [Column]
        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                if (_isActive != value)
                {
                    _isActive = value;
                    NotifyPropertyChanged("IsActive");
                }
            }
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

        public event PropertyChangingEventHandler PropertyChanging;

        // Used to notify that a property is about to change
        private void NotifyPropertyChanging(string propertyName)
        {
            if (PropertyChanging != null)
            {
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }
        #endregion
    }

    #region Drink Data Context
    public class DrinkDataContext : DataContext
    {
        public DrinkDataContext(string connectionString)
            : base(connectionString) { }

        public Table<Drink> Drinks; 
    }
#endregion 
}