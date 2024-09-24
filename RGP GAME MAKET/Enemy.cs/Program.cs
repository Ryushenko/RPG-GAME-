namespace TextRPG
{
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
            ExperienceValue = health / 2; // Опыт за победу равен половине здоровья врага.
        }
    }
}