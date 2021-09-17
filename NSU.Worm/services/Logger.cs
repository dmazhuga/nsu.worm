using System;
using System.IO;

namespace NSU.Worm
{
    public class Logger
    {
        private readonly bool _inConsole;

        private readonly bool _inFile;

        private const string Filename = "log.txt";

        public Logger(bool inConsole, bool inFile)
        {
            _inConsole = inConsole;
            _inFile = inFile;

            File.Create(Filename).Dispose();
        }

        public void log(string logString)
        {
            if (_inConsole)
            {
                Console.Write(logString);
            }

            if (_inFile)
            {
                using StreamWriter writer = new("log.txt", true);
                writer.Write(logString);
            }
        }
    }
}