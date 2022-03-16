using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BotMedicUa.Context;
using BotMedicUa.Models;

namespace BotMedicUa.Repository
{
    public class Repository : IRepository
    {
        private readonly DataContext _db;

        public Repository()
        {
            _db = new DataContext();
        }

        public Doctor GetDoctor(string firstName, string lastName)
        {
            var doctor = _db.Doctor.FirstOrDefault(x => x.FirstName == firstName && x.LastName == lastName);
            return doctor;
        }

        public List<Record> GetRecords(Guid id)
        {
            throw new NotImplementedException();
        }

        public List<Record> GetRecords(long id)
        {
            var patient = _db.Patient.FirstOrDefault(x => x.UserId == id);
            return patient?.Records;
        }

        public async Task AddRecord(long id, Record record)
        {
            await AddPatientRecord(id, record);
            await AddDoctorRecord(record.Doctor.Id, record);
        }

        public async Task RemoveRecord(long id, Record record)
        {
            var patient = _db.Patient.FirstOrDefault(x => x.UserId == id);
            patient?.Records.Remove(record);
            await _db.SaveChangesAsync();
        }

        public List<Doctor> GetDoctors(string classification)
        {
            return _db.Doctor.Where(x => x.Classification == classification).ToList();
        }

        public Patient GetPatient(long id)
        {
            var user = _db.Patient.FirstOrDefault(x => x.UserId == id);
            return user;
        }

        public void AddPatient(long id)
        {
            var patient = _db.Patient.FirstOrDefault(x => x.UserId == id);
            if (patient == null)
            {
                patient = new Patient() { UserId = id };
                _db.Patient.Add(patient);
                _db.SaveChanges();
            }
        }

        public void UpdatePatient(Patient patient)
        {
            var user = _db.Patient.FirstOrDefault(x => x.UserId == patient.UserId);
            if (user != null)
            {
                if (!string.IsNullOrEmpty(patient.FirstName))
                {
                    user.FirstName = patient.FirstName;
                }
                if (!string.IsNullOrEmpty(patient.LastName))
                {
                    user.LastName = patient.LastName;
                }
                if (!string.IsNullOrEmpty(patient.PhoneNumber))
                {
                    user.PhoneNumber = patient.PhoneNumber;
                }

                _db.SaveChanges();
            }
        }

        private async Task AddPatientRecord(long id, Record @record)
        {
            var patient = _db.Patient.FirstOrDefault(x => x.UserId == id);
            patient?.Records.Add(record);
            
            await _db.SaveChangesAsync();
        }
        
        private async Task AddDoctorRecord(long id, Record @record)
        {
            var patient = _db.Doctor.FirstOrDefault(x => x.Id == id);
            patient?.Records.Add(record);
            
            await _db.SaveChangesAsync();
        }
    }
}