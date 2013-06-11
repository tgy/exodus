using System;

namespace Exodus
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
       {
            PlayGame.Researchs.Research r = PlayGame.Researchs.Attack;
            for (int i = 1; i < 20; i++)
            {
                r.Level = i;
            }
            using (Application game = new Application())
            {
                game.Run();
            }
            Environment.Exit(0);
        }
    }
#endif
}

