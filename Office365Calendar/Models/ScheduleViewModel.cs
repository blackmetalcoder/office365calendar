using Syncfusion.XPS;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace Office365Calendar.Models
{
    class ScheduleViewModel
    {
        public ObservableCollection<Meeting> ListOfMeeting { get; set; }
        private List<Point> randomTimeCollection = new List<Point>();

        List<string> currentDayMeetings;
        List<Windows.UI.Xaml.Media.Brush> color_collection;

        public ScheduleViewModel()
        {
            ListOfMeeting = new ObservableCollection<Meeting>();
            //InitializeDataForBookings();
            //BookingAppointments();
        }

        #region BookingAppointments

        private void BookingAppointments()
        {
            Random randomTime = new Random();
            randomTimeCollection = GettingTimeRanges();

            DateTime date;
            DateTime DateFrom = DateTime.Now.AddMonths(-1);
            DateTime DateTo = DateTime.Now.AddMonths(1);
            DateTime dateRangeStart = DateTime.Now.AddDays(-3);
            DateTime dateRangeEnd = DateTime.Now.AddDays(3);

            for (date = DateFrom; date < DateTo; date = date.AddDays(1))
            {
                if ((DateTime.Compare(date, dateRangeStart) > 0) && (DateTime.Compare(date, dateRangeEnd) < 0))
                {
                    for (int AdditionalAppointmentIndex = 0; AdditionalAppointmentIndex < 3; AdditionalAppointmentIndex++)
                    {
                        Meeting meeting = new Meeting();
                        int hour = (randomTime.Next((int)randomTimeCollection[AdditionalAppointmentIndex].X, (int)randomTimeCollection[AdditionalAppointmentIndex].Y));
                        meeting.From = new DateTime(date.Year, date.Month, date.Day, hour, 0, 0);
                        meeting.To = (meeting.From.AddHours(1));
                        meeting.EventName = currentDayMeetings[randomTime.Next(9)];
                        meeting.color = color_collection[randomTime.Next(9)];
                        ListOfMeeting.Add(meeting);
                    }
                }
                else
                {
                    Meeting meeting = new Meeting();
                    meeting.From = new DateTime(date.Year, date.Month, date.Day, randomTime.Next(9, 11), 0, 0);
                    meeting.To = (meeting.From.AddHours(1));
                    meeting.EventName = currentDayMeetings[randomTime.Next(9)];
                    meeting.color = color_collection[randomTime.Next(9)];
                    ListOfMeeting.Add(meeting);
                }
            }
        }

        #endregion BookingAppointments

        #region GettingTimeRanges

        private List<Point> GettingTimeRanges()
        {
            randomTimeCollection = new List<Point>();
            randomTimeCollection.Add(new Point(9, 11));
            randomTimeCollection.Add(new Point(12, 14));
            randomTimeCollection.Add(new Point(15, 17));

            return randomTimeCollection;
        }

        #endregion GettingTimeRanges

        #region InitializeDataForBookings


        #endregion InitializeDataForBookings
    }
}
