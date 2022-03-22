using System;
using System.Collections.Generic;
using BotMedicUa.Models.Enum;

namespace BotMedicUa.Models
{
    public class Schedule
    {
        public Guid Id { get; set; }
        public int StartHour { get; set; }
        public int EndHour { get; set; }
        public List<Day> AvaliableDays { get; set; }
    }
}