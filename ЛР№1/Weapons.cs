using System;
using Players;

namespace Weapons
{
    abstract class Weapon
    {
        public string Name { get; private set; }
        public int Damage { get; private set; }
        public int Ammo { get; set; }
        public int MaxAmmo { get; protected set; }
        public Weapon(string name, int damage, int ammo)
        {
            Name = name;
            Damage = damage;
            Ammo = ammo;
            MaxAmmo = ammo;
        }
        public abstract void Fire(Player target, Player attacker);
        public virtual void Reload()
        {
            Ammo = MaxAmmo;
            Console.WriteLine($"{Name} перезаряжен ({Ammo}/{MaxAmmo})");
        }
        public override string ToString() => $"{Name} (Урон: {Damage}, Патроны: {Ammo}/{MaxAmmo})";
    }

    class Gun : Weapon
    {
        public Gun(string name, int damage, int ammo) : base(name, damage, ammo) { }
        public override void Fire(Player target, Player attacker)
        {
            if (Ammo > 0)
            {
                Ammo--;
                Console.WriteLine($"{attacker.Name} стреляет в {target.Name} из {Name}, урон {Damage}");
                target.TakeDamage(Damage, attacker);
            }
            else
                Console.WriteLine($"{Name} без патронов!");
        }
    }

    class Knife : Weapon
    {
        public Knife(string name, int damage) : base(name, damage, int.MaxValue) { }
        public override void Fire(Player target, Player attacker)
        {
            Console.WriteLine($"{attacker.Name} режет {target.Name} ножом {Name}, урон {Damage}");
            target.TakeDamage(Damage, attacker);
        }
        public override void Reload() => Console.WriteLine($"{Name} не требует перезарядки");
    }

    class Grenade : Weapon
    {
        public Grenade(string name, int damage) : base(name, damage, 1) { }
        public override void Fire(Player target, Player attacker)
        {
            if (Ammo > 0)
            {
                Ammo--;
                Console.WriteLine($"{attacker.Name} бросает {Name} в {target.Name}, урон {Damage}");
                target.TakeDamage(Damage, attacker);
            }
            else
                Console.WriteLine($"{Name} уже использована");
        }
    }
}