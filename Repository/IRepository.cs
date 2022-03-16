using System.Collections.Generic;
using System.Threading.Tasks;
using BotMedicUa.Models;

namespace BotMedicUa.Repository
{
    public interface IRepository
    {
        public Doctor GetDoctor(string firstName, string lastName);
        public List<Record> GetRecords(long id);
        public Task AddRecord(long id, Record record);
        public Task RemoveRecord(long id, Record record);
        public List<Doctor> GetDoctors(string classification);
        public Patient GetPatient(long id);

        public void AddPatient(long id);
        public void UpdatePatient(Patient patient);
    }
}