using SysProgDrugiDeo19461.Logging;
using SysProgDrugiDeo19461.WebServer;

namespace SysProgDrugiDeo19461
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            WebListener listener = new WebListener();
            Logger.Log("Pokretanje servera.");
            await listener.ListenAsync();
            Logger.Log("Server je pokrenut.");
        }
    }
}
