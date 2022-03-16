using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BotMedicUa.Models
{
    public class Patient
    {
        public Guid Id { get; set; }
        public long UserId { get; set; }

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public List<Record> Records { get; set; } = new List<Record>();
    }
}