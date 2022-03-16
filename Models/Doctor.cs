using System.Collections.Generic;

namespace BotMedicUa.Models
{
    public class Doctor
    {
        public long Id { get; set; }
        
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Classification { get; set; }

        public List<Record> Records { get; set; } = new List<Record>();
    }
}