using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using BotMedicUa.Context;
using BotMedicUa.Models;
using BotMedicUa.Models.Enum;
using BotMedicUa.Repository;
using BotMedicUa.Services;
using Deployf.Botf;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace BotMedicUa
{

    public class Program : BotfProgram
    {
        public static void Main(string[] args) => StartBot(args);

        private readonly IHospitalService _hospitalService = new HospitalService();

        [Action(template: "/start", desc: "Початок роботи з ботом")]
        private async Task Start()
        {
            var date = new DateTime();
            var user = _hospitalService.GetUser(ChatId);
            if (user != null)
            {
                PushL("Добрий день! Вас вiтає '💙 TURBO TELEMED 💛'. Оберіть необхідний варіант:");
                RowButton("• Я знаю, який лікар мені потрібен", Q(ChooseDoctor));
                RowButton("• Мені потрібна допомога з вибором спеціалізації лікаря", Q(RecordToFamalyDoctor));
                RowButton("Мої записи", Q(MyRecords));
                RowButton("Адмін панель");
            }
            else
            {
                PushL("Добрий день! Вас вiтає '💙 TURBO TELEMED 💛'. Давайте пройдемо реєстрацію: ");
                _hospitalService.RegisterPatient(new Patient() { UserId = ChatId });
                await Registration();
            }
        }

        private async Task Registration()
        {
            Send("Як до вас звертатись?");
            var name = await AwaitText();

            _hospitalService.RegisterPatient(new Patient()
            {
                UserId = ChatId,
                FirstName = name
            });
            Start();
        }
        [Action]
        public Task RecordToFamalyDoctor()
        {
            PushL("Запишіться до сімейного лікаря:");
            RowButton("Записатись", Q(WriteDoctorsCard, DoctorClassification.FamalyDoctor));
            RowButton("🔴 Назад 🔴 ", Q(Start));
            return Task.CompletedTask;
        }
        [Action]
        public Task ChooseDoctor()
        {
            var names = Enum.GetValues(typeof(DoctorClassification)).Cast<DoctorClassification>();
            PushL("Виберіть необхідного лікаря:");
            foreach (var name in names)
            {
                RowButton(GetNameOfSpecialization(name), Q(WriteDoctorsCard, name));
            }
            RowButton("На головну", Q(Start));
            return Task.CompletedTask;
        }
        
        [Action]
        private Task WriteDoctorsCard(DoctorClassification classification)
        {
            var doctors = _hospitalService.GetDoctors(classification);
            if (doctors == null || doctors.Count == 0)
            {
                PushL("Наразі немає лікарів.");
            }
            else
            {
                PushL("Виберіть лікаря:");
                foreach (var doctor in doctors)
                {
                    RowButton($"{doctor.FirstName} {doctor.LastName}", Q(SetMonth, doctor.Id.ToString(), classification));
                }
            }
            
            RowButton("На головну", Q(Start));
            return Task.CompletedTask;
        }

        [Action]
        public Task SetMonth(string id, DoctorClassification classification)
        {
            var records = _hospitalService.GetUserRecords(ChatId);
            var recordsClassification = records.Select(x => x.Doctor.Classification);

            if (recordsClassification.Contains(classification))
            {
                PushL("Ви вже записались!");
                RowButton("Переглянути запис", Q(WriteRecord, classification));
            }
            else
            {
                var doctor = _hospitalService.GetDoctor(Guid.Parse(id));

                var avaliableDates = doctor.GetAvaliableDates();
                var avaliableMonth = avaliableDates.Select(x => x.Month)
                    .Distinct()
                    .ToList();
                int firstMonth = avaliableMonth[0];
                int secondMonth = avaliableMonth[1];

                PushL("Виберіть місяць:");
                RowButton($"{GetMonthName(avaliableMonth[0])}",
                    Q(SetDay, GetJson(new DateDTO{Day = 1, Id = id, Month = 1}),firstMonth));
                RowButton($"{GetMonthName(avaliableMonth[1])}",
                    Q(SetDay, id,secondMonth));
            }

            RowButton("На головну", Q(Start));
            return Task.CompletedTask;
        }

        [Action]
        private Task SetDay(string id, int month)
        {
            var dto = JsonConvert.DeserializeObject<DateDTO>(id);
            var doctor = _hospitalService.GetDoctor(Guid.Parse(dto.Id));

            var dates = doctor.GetAvaliableDates().Where(x=>x.Month==month).Select(x=>x.Day).Distinct().ToList();
            
            PushL("Виберіть день:");

            foreach (var date in dates)
            {
                RowButton($"{date}", Q(SetHour,dto));
            }
            
            return Task.CompletedTask;
        }

        [Action]
        public Task SetHour(string json)
        {
            Send($"{json}");
            return Task.CompletedTask;
        }
        [Action]
        public Task WriteOnConsultation(string id, string json)
        {
            var dateTime = JsonConvert.DeserializeObject<DateTime>(json);
            var doctor = _hospitalService.GetDoctor(Guid.Parse(id));
            _hospitalService.AddRecord(ChatId, new Record { Date = dateTime, Doctor = doctor });
            PushL("Ви успішно записались");
            RowButton("На головну", Q(Start));
            return Task.CompletedTask;
        }
        [Action]
        public Task MyRecords()
        {
            var records = _hospitalService.GetUserRecords(ChatId);
            if (records.Count == 0 || records == null)
            {
                PushL("У вас немає записів.");
                RowButton("На головну", Q(Start));
            }

            var recordsClassification = records.Select(x => x.Doctor.Classification).Distinct();
            
            PushL("Ваші записи:");
            foreach (var classification in recordsClassification)
            {
                RowButton($"{GetNameOfSpecialization(classification)}", Q(WriteRecord, classification));
            }
            
            RowButton("На головну", Q(Start));
            return Task.CompletedTask;
        }
        [Action]
        private void WriteRecord(DoctorClassification classification)
        {
            var records = _hospitalService.GetUserRecords(ChatId);
            foreach (var record in records)
            {
                if (record.Doctor.Classification == classification)
                {
                    PushLL($"Запис до {GetNameOfSpecialization(record.Doctor.Classification)} {record.Date.Day} {GetMonthName(record.Date.Month)} {record.Date.Hour} {record.Date.Minute}");
                    RowButton("Відмінити", Q(DeleteRecord, record.Id.ToString()));
                    RowButton("На головну", Q(Start));
                }
            }
        }

        [Action]
        private Task DeleteRecord(string id)
        {
            _hospitalService.RemoveRecord(Guid.Parse(id));
            PushL(" Успішно видалено!");
            RowButton("На головну", Q(Start));
            return Task.CompletedTask;
        }

        public string GetNameOfSpecialization(DoctorClassification classification)
        {
            switch (classification)
            {
                case DoctorClassification.FamalyDoctor:
                    return "Сімейний лікар";
                case DoctorClassification.Pediator:
                    return "Педіатор";
                case DoctorClassification.IntensiveTerapy:
                    return "Інтенсивна терапія";
                case DoctorClassification.Anestasiolog:
                    return "Анастезіолог";
                case DoctorClassification.Gastroenterolog:
                    return "Гастроентеролог";
                case DoctorClassification.ChildrenGastroenterolog:
                    return "Дитячий гатсроентеролог";
                case DoctorClassification.Gematolog:
                    return "Гематолог";
                case DoctorClassification.Genetic:
                    return "Генетик";
                case DoctorClassification.Ginecoloc:
                    return "Гінеколог";
                case DoctorClassification.ChildrenGinecolog:
                    return "Дитячий гінеколог";
                case DoctorClassification.Dermatolog:
                    return "Дерматолог";
                case DoctorClassification.ChildrenDermatolog:
                    return "Дитячий дерматолог";
                case DoctorClassification.Endocrinolog:
                    return "Ендокринолог";
                case DoctorClassification.Endoscopist:
                    return "Ендоскопіст";
                case DoctorClassification.Infectionist:
                    return "інфекцоніст";
                case DoctorClassification.ChildrenInfectionist:
                    return "Дитячий інфекіоніст";
                case DoctorClassification.CardiologAritmolog:
                    return "Кардіолог-аритмолог";
                case DoctorClassification.ChildrenCardiolog:
                    return "Дитяий кардіолог, кардіоревматолог";
                case DoctorClassification.Combustiolog:
                    return "Комбустолог";
                case DoctorClassification.Nevrolog:
                    return "Невролог/невропатолог";
                case DoctorClassification.Nefrolog:
                    return "Нефролог";
                case DoctorClassification.ChildrenNefrolog:
                    return "Дитячий нефролог";
                case DoctorClassification.OnkologOnkohirurg:
                    return "Онколог/онкохірург";
                case DoctorClassification.LOR:
                    return "ЛОР";
                case DoctorClassification.ChildrenLOR:
                    return "Дитячий ЛОР";
                case DoctorClassification.Okulist:
                    return "Окуліст";
                case DoctorClassification.ChildrenOkulist:
                    return "Дитячий окуліст";
                case DoctorClassification.Psyhiatr:
                    return "Психіатр";
                case DoctorClassification.Psyholog:
                    return "Психолог";
                case DoctorClassification.Psuhoterapevt:
                    return "Психотерапевт";
                case DoctorClassification.Pulmonolog:
                    return "Пульмонолог";
                case DoctorClassification.ChildrenPulmonolog:
                    return "Дитячий пульмонолог";
                case DoctorClassification.Reabilitolog:
                    return "Реабілітолог";
                case DoctorClassification.Revmatolog:
                    return "Ревматолог";
                case DoctorClassification.Rengenotolog:
                    return "Рентгенолог";
                case DoctorClassification.Stomatolog:
                    return "Стоматолог";
                case DoctorClassification.ChildrenOrtoped:
                    return "Дитячий ортопед";
                case DoctorClassification.ChildrenTravmatolog:
                    return "Дитячий травматолог";
                case DoctorClassification.UZD:
                    return "УЗД/Ультрозвукова діагностика";
                case DoctorClassification.Yrolog:
                    return "Уролог";
                case DoctorClassification.ChildrenYrolog:
                    return "Дитячий уролог";
                case DoctorClassification.Ftiziatr:
                    return "Фтизіатр";
                case DoctorClassification.ChildrenFtiziatr:
                    return "Дитячий фтизіатр";
                case DoctorClassification.ChildrenStomatolog:
                    return "Дитячий стоматолог";
                case DoctorClassification.Travmatolog:
                    return "Травматолог";
                case DoctorClassification.Ortoped:
                    return "Ортопед";
                default:
                    throw new ArgumentOutOfRangeException(nameof(classification), classification, null);
            }
        }

        private string GetMonthName(int numberMonth)
        {
            switch (numberMonth)
            {
                case 1:
                    return "Січень";
                case 2:
                    return "Лютий";
                case 3:
                    return "Березень";
                case 4:
                    return "Квітень";
                case 5:
                    return "Травень";
                case 6:
                    return "Червень";
                case 7:
                    return "Липень";
                case 8:
                    return "Серпень";
                case 9:
                    return "Вересень";
                case 10:
                    return "Жовтень";
                case 11:
                    return "Листопад";
                case 12:
                    return "Грудень";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public string GetJson(DateDTO dto)
        {
            var json = JsonConvert.SerializeObject(dto);
            return json;
        }
    }
}
