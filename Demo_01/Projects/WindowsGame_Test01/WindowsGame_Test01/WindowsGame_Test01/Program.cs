using System;
using WindowsGame_Test01.Helper;
using WindowsGame_Test01.Data;

namespace WindowsGame_Test01
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (SpriteManagerTests game = new SpriteManagerTests())
            {
                game.Run();
            }
        }
    }
#endif
}

