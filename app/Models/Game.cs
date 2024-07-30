using System;

namespace app.Models
{
    public class Game
    {
        public int Hp { get; set; }
        public int MaxHp { get; set; }
        public int Attack { get; set; }
        public int Defence { get; set; }
        public float BlockChance { get; set; } // BlockChance is between 0 and 100

        private Random random = new Random(); // Random number generator

        public Game(int hp, int maxHp, int attack, int defence, float blockChance) {
            Hp = hp;
            MaxHp = maxHp;
            Attack = attack;
            Defence = defence;
            BlockChance = blockChance;
        }

        public int GetHit(int damage) {
            if (IsBlock())
                return Hp;

            int delta = Defence - damage;
            if (delta < 0)
                delta = 0;

            Hp -= delta;

            return Hp;
        }

        private bool IsBlock() {
            double chanceThreshold = BlockChance / 100.0;

            double randomValue = random.NextDouble();
            return randomValue < chanceThreshold;
        }

        public int UseHealing(int amount) {
            Hp += amount;
            if (Hp > MaxHp)
                Hp = MaxHp;

            return Hp;
        }
    }
}
