using System;
using System.Collections.Generic;
using System.Linq;
using BotMedicUa.Models.Enum;

namespace BotMedicUa.Models
{
    public class Doctor
    {
        public Guid Id { get; set; }
        public long ChatId { get; set; }
        public string FirstName { get; set; } = String.Empty;
        public string LastName { get; set; } = String.Empty;
        public DoctorClassification Classification { get; set; }
        public List<Record> Records { get; set; } = new List<Record>();
        public Schedule Schedule { get; set; }

        public List<DateTime> GetAvaliableDates()
        {
            var avaliableDates = new List<DateTime>();
            
            var nonAvaliableDates = Records.Select(x=>x.Date)
                    .OrderBy(x=>x.Date)
                    .ToList();
            
            var dateTimeNow = DateTime.Now;

            var startDate = new DateTime(
                dateTimeNow.Year, 
                dateTimeNow.Month, 
                dateTimeNow.Day, 
                Schedule.StartHour, 
                0, 0);
            var endDate = startDate.AddMonths(1);
            var avaliableDays = Schedule.AvaliableDays.Select(x => x.day).ToList();

            for (var date = startDate; date != endDate; date = date.AddDays(1))
            {
                if (avaliableDays.Contains(date.DayOfWeek))
                {
                    for (var dateForTime = date;
                        dateForTime.Hour != Schedule.EndHour;
                        dateForTime = dateForTime.AddMinutes(20))
                    {
                        if (!nonAvaliableDates.Contains(dateForTime))
                        {
                            avaliableDates.Add(dateForTime);
                        }
                    }
                }
            }

            return avaliableDates;
        }
    }
}