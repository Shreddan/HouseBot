using Microsoft.Extensions.DependencyInjection;

namespace HouseBot
{
    public class Bot
    {
        
        public static DisBot? db;

        public Bot()
        {
            db = new DisBot();
            
        }

        [STAThread]
        public static void Main(string[] args)
        {
            
            db.DiscordRun().GetAwaiter().GetResult();
        }

        
    }
}
