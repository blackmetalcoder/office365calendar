using Newtonsoft.Json;
using Office365Calendar.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Syncfusion.UI.Xaml.Schedule;
using Windows.Globalization;
using Windows.UI.Popups;
using System.Drawing;
using Windows.UI.Xaml.Media;

namespace Office365Calendar.Views
{
    public sealed partial class weekPage : Page, INotifyPropertyChanged
    {
        Windows.UI.Xaml.DispatcherTimer dispatcherTimer;
        string Konto = string.Empty;
        string Password = string.Empty;
        string Doman = string.Empty;
        string Epost = string.Empty;
        string Kalender = string.Empty;
        string KalenderNamn = string.Empty;
        int Nu = 0;
        int EventsAntal = 0;
        private int oldDay = 0;
        private int minuter = 0;
        public ObservableCollection<Meeting> ListOfMeeting = new ObservableCollection<Meeting>();
        ScheduleAppointmentMapping dataMapping;
        ScheduleViewModel viewModel;
        ObservableCollection<Meeting> meetings;
        public weekPage()
        {
            InitializeComponent();
            ApplicationLanguages.PrimaryLanguageOverride = "sv-se";
            viewModel = new ScheduleViewModel();
            dataMapping = new ScheduleAppointmentMapping();
            dataMapping.SubjectMapping = "EventName";
            dataMapping.StartTimeMapping = "From";
            dataMapping.EndTimeMapping = "To";
            dataMapping.AppointmentBackgroundMapping = "color";
            dataMapping.AllDayMapping = "AllDay";
            KalDag.AppointmentMapping = dataMapping;
            this.DataContext = viewModel;
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
        private async void Update()
        {
            //busy.Content = "Uppdaterar kalender i office365";
            ListOfMeeting.Clear();
            busy.IsBusy = true;
            Nu = System.DateTime.Now.DayOfYear;
            
                dispatcherTimer.Stop();
                oldDay = Nu;
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
                DateTime dCalcDate = DateTime.Now;
                DateTime Start = new DateTime(dCalcDate.Year, dCalcDate.Month, 1);
                //txtInfo.Text = KalenderNamn + ", " + Start.ToString("u").Substring(0, 10);
                var s = await DataSource.GetEvents(Konto, Password, Doman, Epost, Kalender, Start);
                var j = JsonConvert.DeserializeObject<List<Handelse>>(s);
                var test = j.Where(m => m.Datum == Start.ToString("u").Substring(0, 10));
                foreach (var H in j)
                {
                    Meeting meeting = new Meeting();
                    string[] tid = H.Start.Split(':');
                    string sZon = H.Tzon.Substring(5, 2);
                    int iZon = int.Parse(sZon);
                    int timme = int.Parse(tid[0]);
                    int minut = int.Parse(tid[1]);
                    int ar = int.Parse(H.Datum.Substring(0, 4));
                    meeting.From = new DateTime(ar, H.Manad, H.Dag, timme + iZon, minut, 0);
                    tid = H.Slut.Split(':');
                    timme = int.Parse(tid[0]);
                    minut = int.Parse(tid[1]);
                    meeting.To = new DateTime(ar, H.Manad, H.Dag, timme + iZon, minut, 0);
                    meeting.EventName = H.Amne;
                    if (meeting.From < dCalcDate && meeting.To > dCalcDate)
                    {
                        SolidColorBrush NowBrush = new SolidColorBrush(Windows.UI.Colors.Red);
                        meeting.color = NowBrush;
                    }
                    else
                    {
                        SolidColorBrush ElseBrush = new SolidColorBrush(Windows.UI.Colors.Green);
                        meeting.color = ElseBrush;
                    }
                    ListOfMeeting.Add(meeting);
                }
            KalDag.ItemsSource = null;
            KalDag.ItemsSource = ListOfMeeting;
                dispatcherTimer.Start();
                busy.IsBusy = false;
            
        }
        private async void AntalEvents()
        {
            DateTime dCalcDate = DateTime.Now;
            DateTime Start = new DateTime(dCalcDate.Year, dCalcDate.Month, 1);
            var svar = await DataSource.getAntal(Konto, Password, Doman, Epost, Kalender, Start);
            var j = JsonConvert.DeserializeObject<List<AntalEvents>>(svar);
           // int O365Antal = j[0].antal;           
            if (j.Count > 0)
            {
                int iAntalLocal = ListOfMeeting.Count;
                if (iAntalLocal != j.Count)
                {
                    Update();
                }
            }
            
        }
        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        private void dispatcherTimer_Tick(object sender, object e)
        {
            AntalEvents();
        }
        private void Page_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 30);
            dispatcherTimer.Start();
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
            Update();
        }
        /*private async void dispatcherTimerTimme_Tick(object sender, object e)
        {
            dispatcherTimerTimme.Stop();
            dispatcherTimer.Stop();
            oldDay = Nu;
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
            //txtInfo.Text = KalenderNamn + ", " + Start.ToString("u").Substring(0, 10);
            var s = await DataSource.GetEvents(Konto, Password, Doman, Epost, Kalender, Start);
            var j = JsonConvert.DeserializeObject<List<Handelse>>(s);
            var test = j.Where(m => m.Datum == Start.ToString("u").Substring(0, 10));
            foreach(var H in test)
            {
                Meeting meeting = new Meeting();
                int hour = int.Parse(H.Langd);
                meeting.From = DateTime.Parse(H.Start);//new DateTime(date.Year, date.Month, date.Day, hour, 0, 0);
                meeting.To = (meeting.From.AddHours(hour));
                meeting.EventName = H.Amne;
               // meeting.color = ;
                ListOfMeeting.Add(meeting);
            }
            KalDag.ItemsSource = ListOfMeeting;
            //EventsList.SelectedIndex = 1;
            dispatcherTimer.Start();
            dispatcherTimerTimme.Start();
        }*/
        private async void KalDag_AppointmentCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {

            if (e.NewItems != null)
            {
                //var dialog = new MessageDialog("Nytt");
               // await dialog.ShowAsync();
                
            }
        }
        private async void KalDag_AppointmentEditorClosed(object sender, AppointmentEditorClosedEventArgs e)
        {
            TimeZoneInfo localZone = TimeZoneInfo.Local;
            var v = e.EditedAppointment as Meeting;
            string svar = await DataSource.AddEvent(Konto, Password, Doman, Konto, "konferans2@blackmetalcoder.se", v.EventName, v.From.ToLocalTime(), v.To.Date.ToLocalTime(), localZone.Id);
            var dialog = new MessageDialog(svar);
            await dialog.ShowAsync();
            var vv = v;
        }
        private void KalDag_AppointmentEditorOpening(object sender, AppointmentEditorOpeningEventArgs e)
        {
            e.Cancel = true;
        }
        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            KalDag.ScheduleType = ScheduleType.Day;
        }
        private void AppBarButton_Click_1(object sender, RoutedEventArgs e)
        {
            KalDag.ScheduleType = ScheduleType.Week;
        }
        private void AppBarButton_Click_2(object sender, RoutedEventArgs e)
        {
            KalDag.ScheduleType = ScheduleType.Month;
        }
        private async void AppBarButton_Click_3(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var result = await MyContentDialog.ShowAsync();
        }
        private async void P_Click(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            TimeZoneInfo localZone = TimeZoneInfo.Local;
            string Amne = txtAmne.Text;
            string Datum = cboDate.Date.ToString();
            string s = String.Format("{0:yyyy-MM-dd}", cboDate.Date);
            string Start = s + " " + cboTid.Time.ToString();
            string Slut = s + " " + cboSluttid.Time.ToString();
            string K = Kalender;
            busy.Content = "Uppdaterar kalender i offife365";
            busy.IsBusy = true;
            var Svar = await DataSource.AddEvent(Konto, Password, Doman, Epost, Kalender, Amne, DateTime.Parse(Start),
                DateTime.Parse(Slut), localZone.Id);
            /*Vi uppdataerar kalendern visuellt utan hämta från exchange*/
            if(Svar.ToString().Contains("Bokning klar"))
            {
                Meeting meeting = new Meeting();
                int ar = int.Parse(cboDate.Date.ToString().Substring(0, 4));
                int manad = int.Parse(cboDate.Date.ToString().Substring(5, 2));
                int dag = int.Parse(cboDate.Date.ToString().Substring(8, 2));
                string[] tid = cboTid.Time.ToString().Split(':');
                int timme = int.Parse(tid[0]);
                int minut = int.Parse(tid[1]);
                meeting.From = new DateTime(ar, manad, dag, timme, minut, 0);
                tid = cboSluttid.Time.ToString().Split(':');
                timme = int.Parse(tid[0]);
                minut = int.Parse(tid[1]);
                meeting.To = new DateTime(ar, manad, dag, timme, minut, 0);
                meeting.EventName = txtAmne.Text;
                
                ListOfMeeting.Add(meeting);
                KalDag.ItemsSource = null;
                KalDag.ItemsSource = ListOfMeeting;
                busy.IsBusy = false;
                var dialog = new MessageDialog("Kalendern uppdaterad");
                await dialog.ShowAsync();
                //Update();
            }
            else
            {
                busy.IsBusy = false;
                var dialog = new MessageDialog("Oj något gick fel, försök igen!");
                await dialog.ShowAsync();
            }

        }
        
    }
}
