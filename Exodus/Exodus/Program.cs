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
            using (Application game = new Application())
            {
                try
                {
                    game.Run();
                }
                catch
                {
                    Environment.Exit(0);
                }
            }
            Environment.Exit(0);
        }
    }
#endif
}

