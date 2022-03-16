using System;
using System.Threading.Tasks;
using BotMedicUa.Context;
using BotMedicUa.Models;
using BotMedicUa.Repository;
using BotMedicUa.Services;
using Deployf.Botf;
using Microsoft.AspNetCore.Identity;

namespace BotMedicUa
{

    public class Program : BotfProgram
    {
        public static void Main(string[] args) => StartBot(args);

        private readonly IHospitalService _hospitalService = new HospitalService();

        [Action(template: "/start", desc: "Початок роботи з ботом")]
        private async Task Start()
        {
            var user = _hospitalService.GetUser(ChatId);
            if (user != null)
            {
                PushL("Добрий день! Вас вiтає '💙 TURBO TELEMED 💛'. Оберіть необхідний варіант:");
                RowButton("• Я знаю, який лікар мені потрібен", Q(Start));
                RowButton("• Мені потрібна допомога з вибором спеціалізації лікаря", Q(PressButton));
            }
            else
            {
                Send("Добрий день! Вас вiтає '💙 TURBO TELEMED 💛'. Давайте пройдемо реєстрацію: ");
                _hospitalService.RegisterPatient(new Patient(){UserId = ChatId});
                await Registration();
            }
        }
        private async Task Registration()
        {
            Send("Введіть своє ім'я");
            var name = await AwaitText();

            Send("Введіть своє прізвище:");
            var lastName = await AwaitText();

            Send("Введіть свій номер:");
            var phoneNumber = await AwaitText();

            _hospitalService.RegisterPatient(new Patient()
            {
                UserId = ChatId,
                FirstName = name,
                LastName = lastName,
                PhoneNumber = phoneNumber
            });
            Start();
        }

        [Action]
        public Task PressButton()
        {
            PushL("В якій частині тіла локалізовані неприємні відчуття/симптоми?");
            RowButton("Голова", Q(Head));
            RowButton("Тулуб");
            RowButton("Руки");
            RowButton("Ноги");
            RowButton("🔴 Назад 🔴 ",Q(Start));
            return Task.CompletedTask;
        }

        [Action]

        public Task Head()
        {
            PushL("Що саме турбує?");
            RowButton("Біль, головокружіння",Q(One));
            RowButton("Удар головою",Q(Two));
            RowButton("Травма в районі обличчя, а саме - щоки, брови, губи",Q(Three));
            RowButton("Удар в ніс",Q(Forth));
            RowButton("🔴 Назад 🔴 ", Q(Start));
            return Task.CompletedTask;
        }
        [Action]
        public Task One()
        {
            PushL("Добрий день! Вас вiтає '💙 TURBO TELEMED 💛'. Оберіть необхідний варіант:");
            return Task.CompletedTask;
        }
        [Action]
        public Task Two()
        {
            PushL("Добрий день! Вас вiтає '💙 TURBO TELEMED 💛'. Оберіть необхідний варіант:");
            return Task.CompletedTask;
        }
        [Action]
        public Task Three()
        {
            PushL("Добрий день! Вас вiтає '💙 TURBO TELEMED 💛'. Оберіть необхідний варіант:");
            return Task.CompletedTask;
        }
        [Action]
        public Task Forth()
        {
            PushL("Добрий день! Вас вiтає '💙 TURBO TELEMED 💛'. Оберіть необхідний варіант:");
            return Task.CompletedTask;
        }
    }
}
