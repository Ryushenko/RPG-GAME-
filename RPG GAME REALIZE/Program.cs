using System;
using System.Collections.Generic;

namespace TextRPG
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            game.Start();
        }
    }

    class Game
    {
        private Player player;
        private List<Enemy> enemies;
        private List<Item> items;
        private bool isRunning;

        public Game()
        {
            player = new Player("Hero", 100, 20);
            enemies = new List<Enemy>
            {
                new Enemy("Goblin", 30, 5),
                new Enemy("Orc", 50, 10),
                new Enemy("Dragon", 100, 20)
            };
            items = new List<Item>
            {
                new Item("Health Potion", "Восстанавливает 20 HP", 20),
                new Item("Strength Potion", "Увеличивает силу атаки на 5", 0)
            };
            isRunning = true;
        }

        public void Start()
        {
            Console.WriteLine("Добро пожаловать в текстовую RPG!");
            ChooseDifficulty(); // Выбор уровня сложности
            while (isRunning)
            {
                DisplayMenu();
                string choice = Console.ReadLine();
                HandleMenuChoice(choice);
            }
        }

        private void ChooseDifficulty()
        {
            Console.WriteLine("Выберите уровень сложности:");
            Console.WriteLine("1. Легкий");
            Console.WriteLine("2. Средний");
            Console.WriteLine("3. Сложный");

            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    player.Health += 20; // Легкий уровень дает больше здоровья
                    break;
                case "2":
                    break; // Средний уровень без изменений
                case "3":
                    player.Health -= 20; // Сложный уровень уменьшает здоровье
                    break;
                default:
                    Console.WriteLine("Неверный выбор. Установлен средний уровень.");
                    break;
            }
        }

        private void DisplayMenu()
        {
            Console.WriteLine("\nВыберите действие:");
            Console.WriteLine("1. Исследовать");
            Console.WriteLine("2. Посмотреть характеристики");
            Console.WriteLine("3. Использовать предмет");
            Console.WriteLine("4. Выйти из игры");
        }

        private void HandleMenuChoice(string choice)
        {
            switch (choice)
            {
                case "1":
                    Explore();
                    break;
                case "2":
                    ShowPlayerStats();
                    break;
                case "3":
                    UseItem();
                    break;
                case "4":
                    isRunning = false;
                    break;
                default:
                    Console.WriteLine("Неверный выбор. Попробуйте снова.");
                    break;
            }
        }

        private void Explore()
        {
            Random random = new Random();
            int encounterChance = random.Next(1, 101);

            if (encounterChance <= 50)
            {
                Console.WriteLine("Вы исследуете мир и не находите врагов.");
                FindItem();
                RandomEvent(); // Случайное событие во время исследования
            }
            else
            {
                Enemy enemy = enemies[random.Next(enemies.Count)];
                Console.WriteLine($"Вы встретили {enemy.Name}!");
                Battle(enemy);
            }
        }

        private void RandomEvent()
        {
            Random random = new Random();
            int eventChance = random.Next(1, 101);

            if (eventChance <= 30) // 30% шанс на случайное событие
            {
                Console.WriteLine("Вы нашли скрытую тропу и получили дополнительные 10 опыта!");
                player.Experience += 10; // Получаем опыт за случайное событие
                CheckLevelUp();
            }
        }

        private void FindItem()
        {
            Random random = new Random();
            Item foundItem = items[random.Next(items.Count)];
            Console.WriteLine($"Вы нашли предмет: {foundItem.Name} - {foundItem.Description}");

            // Добавляем предмет в инвентарь игрока
            player.Inventory.Add(foundItem);
        }

        private void Battle(Enemy enemy)
        {
            Console.WriteLine($"Начинается битва с {enemy.Name}!");

            while (player.Health > 0 && enemy.Health > 0)
            {
                Console.WriteLine($"Ваши HP: {player.Health}, HP {enemy.Name}: {enemy.Health}");
                Console.WriteLine("Выберите действие:");
                Console.WriteLine("1. Атаковать");
                Console.WriteLine("2. Убежать");

                string choice = Console.ReadLine();

                if (choice == "1")
                {
                    enemy.Health -= player.AttackPower;
                    Console.WriteLine($"Вы атаковали {enemy.Name} и нанесли {player.AttackPower} урона!");

                    if (enemy.Health > 0)
                    {
                        player.Health -= enemy.AttackPower;
                        Console.WriteLine($"{enemy.Name} атаковал вас и нанёс {enemy.AttackPower} урона!");
                    }
                    else
                    {
                        Console.WriteLine($"Вы победили {enemy.Name}!");
                        player.Experience += enemy.ExperienceValue; // Получаем опыт за победу
                        CheckLevelUp(); // Проверяем уровень
                    }
                }
                else if (choice == "2")
                {
                    Console.WriteLine("Вы убежали от боя!");
                    break;
                }
                else
                {
                    Console.WriteLine("Неверный выбор. Попробуйте снова.");
                }
            }

            if (player.Health <= 0)
            {
                Console.WriteLine("Вы погибли! Игра окончена.");
                isRunning = false;
            }
        }

        private void CheckLevelUp()
        {
            if (player.Experience >= player.Level * 100) // Уровень увеличивается каждые 100 очков опыта
            {
                player.Level++;
                player.AttackPower += 5; // Увеличиваем силу атаки при повышении уровня
                player.Health += 20; // Восстанавливаем здоровье
                player.Experience = 0; // Сбрасываем опыт
                Console.WriteLine($"Поздравляем! Вы достигли уровня {player.Level}!");
            }
        }

        private void ShowPlayerStats()
        {
            Console.WriteLine($"\nИмя: {player.Name}");
            Console.WriteLine($"Здоровье: {player.Health}");
            Console.WriteLine($"Сила атаки: {player.AttackPower}");
            Console.WriteLine($"Уровень: {player.Level}");
            Console.WriteLine($"Опыт: {player.Experience}");

            if (player.Inventory.Count > 0)
            {
                Console.WriteLine("\nВаш инвентарь:");
                foreach (var item in player.Inventory)
                {
                    Console.WriteLine($"- {item.Name}: {item.Description}");
                }
            }
            else
            {
                Console.WriteLine("\nВаш инвентарь пуст.");
            }
        }

        private void UseItem()
        {
            if (player.Inventory.Count == 0)
            {
                Console.WriteLine("У вас нет предметов для использования.");
                return;
            }

            for (int i = 0; i < player.Inventory.Count; i++)
            {
                var item = player.Inventory[i];
                Console.WriteLine($"{i + 1}. {item.Name}: {item.Description}");
            }

            Console.Write("Выберите номер предмета для использования: ");

            if (int.TryParse(Console.ReadLine(), out int itemIndex) && itemIndex > 0 && itemIndex <= player.Inventory.Count)
            {
                var selectedItem = player.Inventory[itemIndex - 1];
                ApplyItemEffect(selectedItem);
                player.Inventory.RemoveAt(itemIndex - 1); // Удаляем использованный предмет
            }
            else
            {
                Console.WriteLine("Неверный номер предмета.");
            }
        }

        private void ApplyItemEffect(Item item)
        {
            switch (item.Name)
            {
                case "Health Potion":
                    player.Health += item.Value; // Восстанавливаем здоровье
                    if (player.Health > 100) player.Health = 100; // Ограничиваем здоровье максимумом
                    Console.WriteLine($"Вы использовали зелье здоровья и восстановили {item.Value} HP!");
                    break;

                case "Strength Potion":
                    player.AttackPower += item.Value; // Увеличиваем силу атаки
                    Console.WriteLine($"Вы использовали зелье силы и увеличили свою атаку на {item.Value}!");
                    break;

                default:
                    Console.WriteLine("Этот предмет не имеет эффекта.");
                    break;
            }
        }
    }

    class Player
    {
        public string Name { get; set; }
        public int Health { get; set; }
        public int AttackPower { get; set; }
        public int Level { get; set; }
        public int Experience { get; set; }
        public List<Item> Inventory { get; set; }

        public Player(string name, int health, int attackPower)
        {
            Name = name;
            Health = health;
            AttackPower = attackPower;
            Level = 1;
            Experience = 0;
            Inventory = new List<Item>();
        }
    }

    class Enemy
    {
        public string Name { get; set; }
        public int Health { get; set; }
        public int AttackPower { get; set; }
        public int ExperienceValue { get; set; }

        public Enemy(string name, int health, int attackPower)
        {
            Name = name;
            Health = health;
            AttackPower = attackPower;
            ExperienceValue = health / 2; // Опыт за победу равен половине здоровья врага
        }
    }

    class Item
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Value { get; set; } // Значение может быть использовано для восстановления здоровья или увеличения атаки

        public Item(string name, string description, int value)
        {
            Name = name;
            Description = description;
            Value = value;
        }
    }
}