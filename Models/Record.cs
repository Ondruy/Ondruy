using System;

namespace BotMedicUa.Models
{
    public class Record
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        
        public Patient Patient { get; set; } 
        public Doctor Doctor { get; set; }        
    }
}