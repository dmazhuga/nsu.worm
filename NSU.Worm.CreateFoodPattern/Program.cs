using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using NSU.Worm.Data;

namespace NSU.Worm.CreateFoodPattern
{
    class Program
    {
        private const int ModeArgumentIndex = 0;
        private const int NameArgumentIndex = 1;
        private const int IterationsArgumentIndex = 2;
        private const int ConnectionArgumentIndex = 3;

        private const string RandomModeArgument = "-r";
        private const string ManualModeArgument = "-m";

        static void Main(string[] args)
        {
            var mode = args[ModeArgumentIndex];
            var name = args[NameArgumentIndex];
            var iterations = int.Parse(args[IterationsArgumentIndex]);
            var connection = args[ConnectionArgumentIndex];

            var context = CreateContext(connection);
            var patternCreator = new PatternCreator(new RandomFoodGenerator());

            if (mode == RandomModeArgument)
            {
                Console.WriteLine("Creating random pattern...");

                patternCreator.CreateAndSaveRandom(name, iterations, context);
            }
            else if (mode == ManualModeArgument)
            {
                Console.WriteLine("Creating manual pattern...");

                var positions = new List<Position>();

                for (var i = 0; i < iterations; i++)
                {
                    Console.Write($"Input X for iteration {i + 1}: ");
                    var x = int.Parse(Console.ReadLine());

                    Console.Write($"Input Y for iteration {i + 1}: ");
                    var y = int.Parse(Console.ReadLine());

                    positions.Add(new Position(x, y));
                }
                
                patternCreator.CreateAndSaveFromPositionList(name, iterations, positions, context);
            }

            Console.WriteLine("Success");
        }

        private static EnvironmentContext CreateContext(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<EnvironmentContext>();
            optionsBuilder.UseNpgsql(connectionString);

            return new EnvironmentContext(optionsBuilder.Options);
        }
    }
}