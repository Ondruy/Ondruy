using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BotMedicUa.Context;
using BotMedicUa.Models;
using BotMedicUa.Models.Enum;
using Microsoft.EntityFrameworkCore;

namespace BotMedicUa.Repository
{
    public class Repository : IRepository
    {
        private readonly DataContext _db;

        public Repository()
        {
            _db = new DataContext();/*
            var doctor = new Doctor
            {
                FirstName = "Stepan",
                LastName = "Bandera",
                ChatId = 1,
                Classification = DoctorClassification.Anestasiolog,
                Schedule = new Schedule
                {
                    AvaliableDays = new List<Day>()
                    {
                        new Day
                        {
                            day = DayOfWeek.Monday
                        },
                        new Day
                        {
                            day = DayOfWeek.Wednesday
                        }
                    },
                    StartHour = 18,
                    EndHour = 20
                }
            };
            _db.Doctor.Add(doctor);
            _db.SaveChanges();*/
        }
        
        public List<Record> GetRecords(Guid id)
        {
            throw new NotImplementedException();
        }

        public Doctor GetDoctor(string firstName, string lastName, DoctorClassification classification)
        {
            return _db.Doctor.First();
        }

        public List<Record> GetRecords(long id)
        {
            var record = _db.Record.Where(x => x.Patient.UserId == id)
                .Include(x => x.Doctor)
                .Include(x => x.Patient)
                .ToList();
            return record;
        }

        public async Task AddRecord(long id, Record record)
        {
            await AddPatientRecord(id, record);
            await AddDoctorRecord(record.Doctor.Id, record);
        }

        public async Task RemoveRecord(Guid id)
        {
            var record = _db.Record.First(x => x.Id == id);
            _db.Record.Remove(record);
            await _db.SaveChangesAsync();
        }

        public List<Doctor> GetDoctors(DoctorClassification classification)
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
                _db.SaveChanges();
            }
        }

        public void AddDoctor(Doctor doctor)
        {
            _db.Doctor.Add(doctor);
            _db.SaveChanges();
        }

        public List<Doctor> GetDoctors()
        {
            var doctors = _db.Doctor.Include(x => x.Records).Include(x=>x.Schedule).Include(x=>x.Schedule.AvaliableDays).ToList();
            return doctors;
        }

        private async Task AddPatientRecord(long id, Record @record)
        {
            var patient = _db.Patient.FirstOrDefault(x => x.UserId == id);
            patient?.Records.Add(record);
            
            await _db.SaveChangesAsync();
        }
        
        private async Task AddDoctorRecord(Guid id, Record @record)
        {
            var patient = _db.Doctor.FirstOrDefault(x => x.Id == id);
            patient?.Records.Add(record);
            
            await _db.SaveChangesAsync();
        }
    }
}