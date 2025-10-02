using System;
using Exceptions;
using Interfaces;
using Weapons;

namespace Players
{
    abstract class Player : IMovable, IDamageable, IShootable
    {
        public string Name { get; private set; }
        private int Health { get; set; }
        private string Team { get; set; }
        private Weapon Weapon { get; set; }
        private int Money { get; set; }
        public int Kills { get; private set; }
        private int Deaths { get; set; }
        private bool IsAlive => Health > 0;
        public Player(string name, string team, int health = 100, int money = 800)
        {
            Name = name;
            Team = team;
            Health = health;
            Money = money;
        }
        public void Move() => Console.WriteLine($"{Name} движется");
        public void Shoot(Player target)
        {
            if (Weapon == null)
            {
                Console.WriteLine($"{Name} без оружия");
                return;
            }
            if (target == this)
            {
                Console.WriteLine($"{Name} не может стрелять сам в себя");
                return;
            }
            if (target.Team == this.Team)
            {
                Console.WriteLine($"{Name} не стреляет в союзника {target.Name}");
                return;
            }
            Weapon.Fire(target, this);
        }
        public void BuyWeapon(Weapon weapon, int price)
        {
            if (Money < price) throw new NotEnoughMoneyException();
            Weapon = weapon;
            Money -= price;
            Console.WriteLine($"{Name} купил {weapon.Name} за {price}. Осталось денег: {Money}");
        }
        public void TakeDamage(int amount, Player attacker = null)
        {
            if (!IsAlive) return;
            Health -= amount;
            if (Health > 0)
            {
                Console.WriteLine($"{Name} получает {amount} урона (HP: {Health})");
            }
            else
            {
                Health = 0;
                Deaths++;
                Console.WriteLine($"{Name} был убит!");
                if (attacker != null && attacker != this)
                {
                    attacker.Kills++;
                    Console.WriteLine($"{attacker.Name} сделал убийство! (Kills: {attacker.Kills})");
                }
            }
        }
        public void ShowStats()
        {
            Console.WriteLine($"[{Team}] {Name} | HP: {Health} | Kills: {Kills} | Deaths: {Deaths}");
        }
        public override string ToString() => $"[{Team}] {Name} (HP: {Health}, Kills: {Kills}, Deaths: {Deaths})";
        public override bool Equals(object obj) => obj is Player p && Name == p.Name && Team == p.Team;
        public override int GetHashCode() => HashCode.Combine(Name, Team);
    }

    class Terrorist : Player
    {
        public Terrorist(string name) : base(name, "Terrorists")
        {
            Console.WriteLine($"Террорист {name} вступил в команду!");
        }

        public void PlantBomb(Bomb bomb, string spot)
        {
            if (bomb.IsPlanted)
            {
                Console.WriteLine($"{Name} не может поставить бомбу — она уже установлена!");
                return;
            }
            bomb.Plant(spot);
            Console.WriteLine($"{Name} устанавливает бомбу на {spot}");
        }
    }

    class CounterTerrorist : Player
    {
        public CounterTerrorist(string name) : base(name, "Counter-Terrorists")
        {
            Console.WriteLine($"Контр-террорист {name} готов к бою!");
        }

        public void DefuseBomb(Bomb bomb)
        {
            if (!bomb.IsPlanted)
            {
                Console.WriteLine($"{Name} не может раздефьюзить — бомба не установлена!");
                return;
            }
            bomb.Defuse();
            Console.WriteLine($"{Name} раздефьюживает бомбу!");
        }
    }
}