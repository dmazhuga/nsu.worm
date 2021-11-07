using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace NSU.Worm
{
    public class NameGenerator : INameGenerator
    {
        private Random _random;

        private HashSet<string> _usedNames;

        private ImmutableList<string> _namePool = ImmutableList.Create("Arkady", "Agafia", "Abram", "Albina",
            "Albert", "Alena", "Alexander", "Alexandra", "Alexei", "Alina", "Anatoly", "Alisa", "Andrei", "Alla",
            "Anton", "Anastasia", "Arseny", "Anna", "Artur", "Asya", "Artyom", "Darya", "Boris", "Diana", "Daniil",
            "Dina", "Denis", "Ekaterina", "Dmitry", "Elena", "Eduard", "Elvira", "Evgeniy", "Eugenia", "Fedor",
            "Eva", "Foma", "Galina", "Georgy", "Inga", "German", "Inna", "Gleb", "Irina", "Grigory", "Karina",
            "Ignat", "Kira", "Igor", "Klara", "Ilya", "Klavdia", "Ivan", "Kristina", "Kirill", "Kseniya", "Konstantin",
            "Larisa", "Leonid", "Lidiya", "Makar", "Liliya", "Maksim", "Lubov", "Mark", "Ludmila", "Matvey",
            "Margarita", "Mikhail", "Maria", "Mitrofan", "Marina", "Nikita", "Nadezhda", "Nikolay", "Natalia", "Oleg",
            "Nika", "Pavel", "Nina", "Pyotr", "Oksana", "Rodion", "Olesya", "Roman", "Olga", "Polina", "Rostislav",
            "Regina", "Ruslan", "Renata", "Saveliy", "Sofia", "Semyon", "Sofya", "Sergei", "Svetlana", "Stanislav",
            "Tamara", "Stepan", "Tatiana", "Taras", "Ulyana", "Timofei", "Valentina", "Timur", "Valeria", "Vadim",
            "Varvara", "Valentin", "Vasilisa", "Valeriy", "Veronika", "Vasiliy", "Viktoriya", "Viktor", "Yana",
            "Vitaliy", "Yaroslava", "Vladimir", "Yulia", "Yakov", "Zhanna", "Yaroslav", "Zinaida", "Yuri", "Zlata");

        public NameGenerator()
        {
            _random = new Random();

            _usedNames = new HashSet<string>();
        }

        public string NextName()
        {
            var randomIndex = _random.Next(0, _namePool.Count);

            while (_usedNames.Contains(_namePool[randomIndex]))
            {
                randomIndex = _random.Next(0, _namePool.Count);
            }

            _usedNames.Add(_namePool[randomIndex]);

            return _namePool[randomIndex];
        }
    }
}