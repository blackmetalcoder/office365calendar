using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Office365Calendar.Models
{
    public sealed class DataSource
    {
        public async static Task<string> GetKalender(string konto, string password, string doman, string epost)
        {
            try
            {
                var client = new HttpClient();
                var uri = new Uri("https://callendarbackend.azurewebsites.net/api/Kalender?Epost=" + konto + "&Password=" + password + "&Doman=" + doman + "&Discover=" + epost);
                var Response = await client.GetAsync(uri);
                var statusCode = Response.StatusCode;
                Response.EnsureSuccessStatusCode();
                var ResponseText = await Response.Content.ReadAsStringAsync();
                return ResponseText;
            }

            catch (Exception ex)
            {
                return "fel";
            }
        }
        public async static Task<string> GetEvents(string konto, string password, string doman, string epost, string K, DateTime Datum)
        {
            try
            {
                var client = new HttpClient();
                var uri = new Uri("https://callendarbackend.azurewebsites.net/api/Gettoday?Epost=" + konto + "&Password=" + password + "&Doman=" + doman + "&Discover=" + epost + "&Kalender=" + K + "&Datum=" + Datum);
                var Response = await client.GetAsync(uri);
                var statusCode = Response.StatusCode;
                Response.EnsureSuccessStatusCode();
                var ResponseText = await Response.Content.ReadAsStringAsync();
                return ResponseText;
            }

            catch (Exception ex)
            {
                return "fel";
            }
        }
        public async static Task<string> AddEvent(string konto, string password, string doman, string epost, string kalender, string Amne, DateTime start, DateTime slut, string tZon)
        {
            try
            {
                 var client = new HttpClient();
                var uri = new Uri("https://callendarbackend.azurewebsites.net/api/Addevent?Epost=" + konto + "&Password=" + password + "&Doman=" + doman + "&Discover=" + epost + "&kalender=" + kalender + "&Amne=" + Amne + "&start=" + start + "&slut=" + slut + "&tZon=" + tZon);
                var Response = await client.GetAsync(uri);
                var statusCode = Response.StatusCode;
                Response.EnsureSuccessStatusCode();
                var ResponseText = await Response.Content.ReadAsStringAsync();
                return ResponseText;
            }

            catch (Exception ex)
            {
                return "fel";
            }
        }
        public async static Task<string> getAntal(string konto, string password, string doman, string epost, string K, DateTime Datum)
        {
            int i = 0;
            try
            {
                var client = new HttpClient();
                var uri = new Uri("https://callendarbackend.azurewebsites.net/api/CountEvents?Epost=" + konto + "&Password=" + password + "&Doman=" + doman + "&Discover=" + epost + "&Kalender=" + K + "&Datum=" + Datum);
                var Response = await client.GetAsync(uri);
                var statusCode = Response.StatusCode;
                Response.EnsureSuccessStatusCode();
                var ResponseText = await Response.Content.ReadAsStringAsync();
                return ResponseText;
            }

            catch (Exception ex)
            {
                return "fel";
            }
        }
        public static string tidsZon()
        {
            string svar = string.Empty;
            CultureInfo Culture = SystemInformation.Culture;
            return Culture.Name;
        }
        public static DateTime GetFirstDayOfWeek(DateTime dayInWeek)
        {
            CultureInfo defaultCultureInfo = CultureInfo.CurrentCulture;
            return GetFirstDateOfWeek(dayInWeek, defaultCultureInfo);
        }
        public static DateTime GetFirstDateOfWeek(DateTime dayInWeek, CultureInfo cultureInfo)
        {
            DayOfWeek firstDay = cultureInfo.DateTimeFormat.FirstDayOfWeek;
            DateTime firstDayInWeek = dayInWeek.Date;
            while (firstDayInWeek.DayOfWeek != firstDay)
                firstDayInWeek = firstDayInWeek.AddDays(-1);

            return firstDayInWeek;
        }
    }
}
