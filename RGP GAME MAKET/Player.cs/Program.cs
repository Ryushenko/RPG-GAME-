using System.Collections.Generic;

namespace TextRPG
{
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
}