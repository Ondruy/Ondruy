using System;
using System.Collections.Generic;
using BotMedicUa.Models;

namespace BotMedicUa.Services
{
    public interface IHospitalService
    {
        public void RegisterPatient(Patient patient);
        public void AddRecord(long id, Record record);
        public List<Doctor> GetAvaliableDoctors(DateTime dateTime);

        public Patient GetUser(long id);
    }
}