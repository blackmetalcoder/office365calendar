using Newtonsoft.Json;
using Office365Calendar.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using Windows.UI.Xaml.Controls;

namespace Office365Calendar.Views
{
    public sealed partial class CalendarsPage : Page, INotifyPropertyChanged
    {
        public CalendarsPage()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Set<T>(ref T storage, T value, [CallerMemberName]string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return;
            }

            storage = value;
            OnPropertyChanged(propertyName);
        }

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private async void Page_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            pring1.IsActive = true;
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            string Konto = string.Empty;
            string Password = string.Empty;
            string Doman = string.Empty;
            string Epost = string.Empty;
            //Konto
            Object valueKonto = localSettings.Values["Konto"];
            if (valueKonto == null)
            {
                // No data
            }
            else
            {
                Konto = valueKonto.ToString();
            }
            //Password
            Object valuePwd = localSettings.Values["Password"];
            if (valuePwd == null)
            {
                // No data
            }
            else
            {
                Password = valuePwd.ToString();
            }
            //Doman
            Object valueDoman = localSettings.Values["Doman"];
            if (valueDoman == null)
            {
                // No data
            }
            else
            {
                Doman = valueDoman.ToString();
            }
            //Konto
            Object valueEpost = localSettings.Values["Epost"];
            if (valueEpost == null)
            {
                // No data
            }
            else
            {
                Epost = valueEpost.ToString();
            }
            var s = await DataSource.GetKalender(Konto, Password, Doman, Epost);
            var j = JsonConvert.DeserializeObject<List<Kalender>>(s);
            CalendarList.ItemsSource = j;
            pring1.IsActive = false;
        }

        private void CalendarList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var myClickedItem = e.AddedItems[0] as Kalender;
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            localSettings.Values["Kalender"] = myClickedItem.epost;
            localSettings.Values["KalenderNamn"] = myClickedItem.Namn;
        }
    }
}
