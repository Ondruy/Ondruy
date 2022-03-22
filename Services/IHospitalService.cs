using System;
using System.Collections.Generic;
using BotMedicUa.Models;
using BotMedicUa.Models.Enum;

namespace BotMedicUa.Services
{
    public interface IHospitalService
    {
        public void RegisterPatient(Patient patient);
        public void AddRecord(long id, Record record);
        public List<Doctor> GetAvaliableDoctors(DateTime dateTime);

        public Patient GetUser(long id);
        public List<Record> GetUserRecords(long id);
        public Doctor GetDoctor(Guid id);
        public void CreateDoctor(Doctor doctor);
        public void RemoveRecord(Guid id);
        public List<Doctor> GetDoctors(DoctorClassification classification);
    }
}