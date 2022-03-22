using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BotMedicUa.Models;
using BotMedicUa.Models.Enum;

namespace BotMedicUa.Repository
{
    public interface IRepository
    {
        public Doctor GetDoctor(string firstName, string lastName, DoctorClassification classification);
        public List<Record> GetRecords(long id);
        public Task AddRecord(long id, Record record);
        public Task RemoveRecord(Guid id);
        public List<Doctor> GetDoctors(DoctorClassification classification);
        public Patient GetPatient(long id);

        public void AddPatient(long id);
        public void UpdatePatient(Patient patient);
        public void AddDoctor(Doctor doctor);
        public List<Doctor> GetDoctors();
    }
}