using System;

namespace PinkPonk
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new PinkPonkGame())
                game.Run();
        }
    }
}
