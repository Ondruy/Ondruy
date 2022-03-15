using Deployf.Botf;

namespace BotMedicUa
{

    public class MainController : BotControllerBase
    {

        [Action(template: "/start", desc: "Початок роботи з ботом")]
        public void Start()
        {
            PushL("Добрий день! Вас вiтає '💙 TURBO TELEMED 💛'. Оберіть необхідний варіант:");
            RowButton("• Я знаю, який лікар мені потрібен");
            RowButton("• Мені потрібна допомога з вибором спеціалізації лікаря", Q(PressButton));


        }

        [Action]
        public void PressButton()
        {

            PushL("В якій частині тіла локалізовані неприємні відчуття/симптоми?");
            RowButton("Голова", Q(Head));
            RowButton("Тулуб");
            RowButton("Руки");
            RowButton("Ноги");
            RowButton("🔴 Назад 🔴 ",Q(Start));
        }

        [Action]

        public void Head()
        {
            PushL("Що саме турбує?");
            RowButton("Біль, головокружіння",Q(One));
            RowButton("Удар головою",Q(Two));
            RowButton("Травма в районі обличчя, а саме - щоки, брови, губи",Q(Three));
            RowButton("Удар в ніс",Q(Forth));
            RowButton("🔴 Назад 🔴 ", Q(Start));
        }
        [Action]
        public void One()
        {
            PushL("Добрий день! Вас вiтає '💙 TURBO TELEMED 💛'. Оберіть необхідний варіант:");
        }
        [Action]
        public void Two()
        {
            PushL("Добрий день! Вас вiтає '💙 TURBO TELEMED 💛'. Оберіть необхідний варіант:");
        }
        [Action]
        public void Three()
        {
            PushL("Добрий день! Вас вiтає '💙 TURBO TELEMED 💛'. Оберіть необхідний варіант:");
        }
        [Action]
        public void Forth()
        {
            PushL("Добрий день! Вас вiтає '💙 TURBO TELEMED 💛'. Оберіть необхідний варіант:");
        }
    }

}