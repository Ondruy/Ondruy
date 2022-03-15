
using Deployf.Botf;

namespace BotMedicUa
{

    public class Program
    {
        public static void Main(string[] args)
        {
            BotfProgram.StartBot(args, onConfigure: (svc, cfg) =>
                {
                }, onRun: (app, cfg)=>
                {
                });
           
        }
    }
}
