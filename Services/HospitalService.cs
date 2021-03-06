using System;
using System.Collections.Generic;
using System.Linq;
using BotMedicUa.Context;
using BotMedicUa.Models;
using BotMedicUa.Models.Enum;
using BotMedicUa.Repository;

namespace BotMedicUa.Services
{
    public class HospitalService:IHospitalService
    {
        private readonly DataContext _db;
        private readonly IRepository _repository;

        public HospitalService()
        {
            _db = new DataContext();
            _repository = new Repository.Repository();
        }

        public void RegisterPatient(Patient client)
        {
            var patient = _repository.GetPatient(client.UserId);
            if (patient == null)
            {
                _repository.AddPatient(client.UserId);
            }
            else
            {
                _repository.UpdatePatient(client);
            }
        }

        public void AddRecord(long id, Record record)
        {
            _repository.AddRecord(id, record);
        }

        public List<Doctor> GetAvaliableDoctors(DateTime dateTime)
        {
            throw new NotImplementedException();
        }

        public Patient GetUser(long id)
        {
            return _repository.GetPatient(id);
        }

        public List<Record> GetUserRecords(long id)
        {
            var records = _repository.GetRecords(id);
            return records;
        }

        public Doctor GetDoctor(Guid id)
        {
            var doctor = _repository.GetDoctors().First(x => x.Id == id);
            return doctor;
        }

        public void CreateDoctor(Doctor doctor)
        {
            _repository.AddDoctor(doctor);
        }

        public void RemoveRecord(Guid id)
        {
            _repository.RemoveRecord(id);
        }

        public List<Doctor> GetDoctors(DoctorClassification classification)
        {
            var doctors = _repository.GetDoctors()
                .Where(x=>x.Classification == classification)
                .ToList();
            
            return doctors;
        }
    }
}