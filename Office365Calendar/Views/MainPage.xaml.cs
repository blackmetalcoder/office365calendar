using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using Windows.UI.Xaml.Controls;

namespace Office365Calendar.Views
{
    public sealed partial class MainPage : Page, INotifyPropertyChanged
    {
        public MainPage()
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

        private void sldUppdatering_ValueChanged(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            //txtMinuter.Text = "Minuter mellan uppdatering från Office 365 är " + sldUppdatering.Value.ToString();
        }

        private void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            localSettings.Values["Konto"] = txtKonto.Text;
            localSettings.Values["Password"] = txtPassword.Password;
            localSettings.Values["Doman"] = txtDoman.Text;
            localSettings.Values["Epost"] = txtKonto.Text;
            //localSettings.Values["Startsida"] = cboSida.SelectedIndex;
            //localSettings.Values["Uppdatering"] = sldUppdatering.Value;
        }

        private void Page_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            //Konto
            Object valueKonto = localSettings.Values["Konto"];
            if (valueKonto == null)
            {
                // No data
            }
            else
            {
                txtKonto.Text = valueKonto.ToString();
            }
            //Password
            Object valuePwd = localSettings.Values["Password"];
            if (valuePwd == null)
            {
                // No data
            }
            else
            {
                txtPassword.Password = valuePwd.ToString();
            }
            //Doman
            Object valueDoman = localSettings.Values["Doman"];
            if (valueDoman == null)
            {
                // No data
            }
            else
            {
                txtDoman.Text = valueDoman.ToString();
            }
            //Konto
            Object valueEpost = localSettings.Values["Epost"];
            if (valueEpost == null)
            {
                // No data
            }
            else
            {
                //txtEpost.Text = valueEpost.ToString();
            }
            //Startsida
            Object valueStart = localSettings.Values["Startsida"];
            if (valueStart == null)
            {
                // No data
            }
            else
            {
                //cboSida.SelectedIndex = int.Parse(valueStart.ToString());
            }
            //Tid mellan uppdateringar
            Object valueUppd = localSettings.Values["Uppdatering"];
            if (valueUppd == null)
            {
                // No data
            }
            else
            {
                //sldUppdatering.Value = int.Parse(valueUppd.ToString());
            }
        }
    }
}
