using Newtonsoft.Json;
using Office365Calendar.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Office365Calendar.Views
{
    public sealed partial class TodayPage : Page, INotifyPropertyChanged
    {
        Windows.UI.Xaml.DispatcherTimer dispatcherTimer;
        Windows.UI.Xaml.DispatcherTimer dispatcherTimerTimme;
        string Konto = string.Empty;
        string Password = string.Empty;
        string Doman = string.Empty;
        string Epost = string.Empty;
        string Kalender = string.Empty;
        string KalenderNamn = string.Empty;
        int Nu = 0;
        private int oldDay = 0;
        private int minuter = 0;
        public TodayPage()
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
        private async void dispatcherTimer_Tick(object sender, object e)
        {
            Nu = System.DateTime.Now.DayOfYear;
            if (Nu > oldDay)
            {
                dispatcherTimer.Stop();
                oldDay = Nu;
                pring1.IsActive = true;
                var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

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
                //Kalender
                Object valueKalender = localSettings.Values["Kalender"];
                if (valueEpost == null)
                {
                    // No data
                }
                else
                {
                    Kalender = valueKalender.ToString();
                }
                //Kalender
                Object valueKalenderNamn = localSettings.Values["KalenderNamn"];
                if (valueEpost == null)
                {
                    // No data
                }
                else
                {
                    KalenderNamn = valueKalenderNamn.ToString();
                }
                DateTime Start = System.DateTime.Now.Date;
                txtInfo.Text = KalenderNamn + ", " + Start.ToString("u").Substring(0, 10);
                var s = await DataSource.GetEvents(Konto, Password, Doman, Epost, Kalender, Start);
                var j = JsonConvert.DeserializeObject<List<Handelse>>(s);
                var test = j.Where(m => m.Datum == Start.ToString("u").Substring(0, 10));
                EventsList.ItemsSource = test;
                pring1.IsActive = false;
                dispatcherTimer.Start();
            }


        }
        private void Page_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
            /*Vi hämtar antal minuter mellan hämtning från office 365*/
            dispatcherTimerTimme = new DispatcherTimer();
            dispatcherTimerTimme.Tick += dispatcherTimerTimme_Tick;
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            Object valueUppd = localSettings.Values["Uppdatering"];

            if (valueUppd == null)
            {
                minuter = 60;
            }
            else
            {
                minuter = int.Parse(valueUppd.ToString());
            }
            /*Slut hämtning från office 365*/
        }
        private async void dispatcherTimerTimme_Tick(object sender, object e)
        {
            dispatcherTimerTimme.Stop();
            dispatcherTimer.Stop();
            oldDay = Nu;
            pring1.IsActive = true;
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
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
            //Kalender
            Object valueKalender = localSettings.Values["Kalender"];
            if (valueEpost == null)
            {
                // No data
            }
            else
            {
                Kalender = valueKalender.ToString();
            }
            //Kalender
            Object valueKalenderNamn = localSettings.Values["KalenderNamn"];
            if (valueEpost == null)
            {
                // No data
            }
            else
            {
                KalenderNamn = valueKalenderNamn.ToString();
            }
            Object valueUppd = localSettings.Values["Uppdatering"];

            if (valueUppd == null)
            {
                // No data
            }
            else
            {
                minuter = int.Parse(valueUppd.ToString());
            }
            DateTime Start = System.DateTime.Now.Date;
            txtInfo.Text = KalenderNamn + ", " + Start.ToString("u").Substring(0, 10);
            var s = await DataSource.GetEvents(Konto, Password, Doman, Epost, Kalender, Start);
            var j = JsonConvert.DeserializeObject<List<Handelse>>(s);
            var test = j.Where(m => m.Datum == Start.ToString("u").Substring(0, 10));
            EventsList.ItemsSource = test;
            //EventsList.SelectedIndex = 1;
            pring1.IsActive = false;
            dispatcherTimer.Start();
            dispatcherTimerTimme.Start();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            string tid = DataSource.tidsZon();
            var dialog = new MessageDialog(tid);
            await dialog.ShowAsync();
        }
    }
}
