using System;
using System.Collections;
using System.Collections.Generic;
using Interfaces;
using Exceptions;
using Weapons;

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

class Bomb
{
    public bool IsPlanted { get; private set; }
    private int Timer { get; set; }
    private string SpotPlanted { get; set; }
    public void Plant(string spotPlanted)
    {
        if (IsPlanted) throw new BombAlreadyPlantedException();
        IsPlanted = true;
        SpotPlanted = spotPlanted;
        Timer = 40;
        Console.WriteLine($"Бомба установлена на {SpotPlanted}, таймер: {Timer}");
    }
    public void Defuse()
    {
        if (IsPlanted)
        {
            IsPlanted = false;
            Console.WriteLine("Бомба успешно раздефьюжена");
        }
    }
    public void Explode()
    {
        if (IsPlanted)
        {
            Console.WriteLine("Бомба взорвалась!");
            IsPlanted = false;
        }
    }
}

class Map
{
    public string Name { get; set; }
    public string Size { get; set; }
    public string SpotA { get; set; }
    public string SpotB { get; set; }
    public void Load() => Console.WriteLine($"Загрузка карты {Name}...\n");
    public void ShowInfo()
    {
        Console.WriteLine($"Карта: {Name} | Размер: {Size} | A: {SpotA}, B: {SpotB}");
    }
    public override string ToString() => $"{Name} ({Size})";
}

enum RoundStatus { NotStarted, InProgress, Finished }

class Round
{
    private int RoundNumber { get; set; }
    private RoundStatus Status { get; set; }
    public Round(int number)
    {
        RoundNumber = number;
        Status = RoundStatus.NotStarted;
    }
    public void Start()
    {
        Status = RoundStatus.InProgress;
        Console.WriteLine($"\nРаунд {RoundNumber} начался!\n");
    }
    public void End()
    {
        Status = RoundStatus.Finished;
        Console.WriteLine($"Раунд {RoundNumber} закончился!");
    }
    public class MVP
    {
        private Player Player { get; }
        public MVP(Player player) => Player = player;
        public void Show() => Console.WriteLine($"MVP раунда: {Player.Name} ({Player.Kills} убийств(-а))");
    }
}

record Purchase(Player Buyer, Weapon Weapon, int Price);

class Program
{
    static void Main(string[] args)
    {
        // Карты
        var maps = new List<Map>
        {
            new Map { Name = "Dust2", Size = "Medium", SpotA = "A Site", SpotB = "B Site" },
            new Map { Name = "Mirage", Size = "Medium", SpotA = "A Site", SpotB = "B Site" },
            new Map { Name = "Inferno", Size = "Medium", SpotA = "A Site", SpotB = "B Site" },
            new Map { Name = "Nuke", Size = "Medium", SpotA = "A Site", SpotB = "B Site" },
            new Map { Name = "Train", Size = "Big", SpotA = "A Site", SpotB = "B Site" },
            new Map { Name = "Overpass", Size = "Big", SpotA = "A Site", SpotB = "B Site" },
            new Map { Name = "Ancient", Size = "Medium", SpotA = "A Site", SpotB = "B Site" }
        };

        // Выбор карты
        Map mirage = null;
        foreach (var m in maps)
        {
            if (m.Name == "Mirage")
            {
                mirage = m;
                break;
            }
        }
        if (mirage != null)
        {
            mirage.ShowInfo();
            mirage.Load();
        }

        // Игроки
        var terrorists = new List<Terrorist>
        {
            new Terrorist("apEX"),
            new Terrorist("ropz"),
            new Terrorist("ZywOo"),
            new Terrorist("flameZ"),
            new Terrorist("mezii")
        };
        var cts = new List<CounterTerrorist>
        {
            new CounterTerrorist("Aleksib"),
            new CounterTerrorist("iM"),
            new CounterTerrorist("b1t"),
            new CounterTerrorist("wOnderful"),
            new CounterTerrorist("makazze")
        };

        // Оружие
        var usp = new Gun("USP-S", 43, 12);
        var glock = new Gun("Glock-18", 37, 20);
        var deagle = new Gun("Desert Eagle", 77, 10);
        var ak = new Gun("AK-47", 44, 30);
        var m4 = new Gun("M4A4", 40, 30);
        var m4s = new Gun("M4A1-S", 41, 25);
        var knife = new Knife("Knife", 34);

        // Закупка
        try
        {
            terrorists[0].BuyWeapon(glock, 0);
            terrorists[1].BuyWeapon(deagle, 700);
            terrorists[2].BuyWeapon(deagle, 700);
            terrorists[3].BuyWeapon(glock, 0);
            terrorists[4].BuyWeapon(glock, 0);

            cts[0].BuyWeapon(usp, 0);
            cts[1].BuyWeapon(usp, 0);
            cts[2].BuyWeapon(deagle, 700);
            cts[3].BuyWeapon(usp, 700);
            cts[4].BuyWeapon(deagle, 700);
        }
        catch (NotEnoughMoneyException ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }

        // Раунд
        Round round1 = new Round(1);
        round1.Start();

        // Движение игроков
        foreach (var t in terrorists) t.Move();
        foreach (var ct in cts) ct.Move();

        // Перестрелка
        terrorists[1].Shoot(cts[2]); // ropz → b1t
        cts[2].Shoot(terrorists[1]); // b1t → ropz
        terrorists[1].Shoot(cts[2]); // ropz → b1t

        cts[1].Shoot(terrorists[0]); // iM → apEX
        terrorists[0].Shoot(cts[1]); // apEX → iM
        cts[1].Shoot(terrorists[0]); // iM → apEX

        cts[0].Shoot(terrorists[4]); // Aleksib → mezii
        cts[1].Shoot(terrorists[0]); // iM → apEX

        // Бомба
        Bomb bomb = new Bomb();
        try
        {
            terrorists[4].PlantBomb(bomb, "A Site");
        }
        catch (BombAlreadyPlantedException ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }

        // Продолжение перестрелки
        terrorists[4].Shoot(cts[4]); // mezii → makazze
        cts[4].Shoot(terrorists[1]); // makazze → ropz
        terrorists[4].Shoot(cts[4]); // mezii → makazze
        cts[4].Shoot(terrorists[4]); // makazze → mezii
        cts[4].Shoot(terrorists[3]); // makazze → flameZ
        cts[4].Shoot(terrorists[3]); // makazze → flameZ
        cts[2].Shoot(terrorists[3]); // b1t → flameZ
        cts[3].Shoot(terrorists[2]); // wOnderful → ZywOo
        cts[0].Shoot(terrorists[2]); // Aleksib → ZywOo
        cts[0].Shoot(terrorists[2]); // Aleksib → ZywOo
        cts[1].Shoot(terrorists[3]); // iM → flameZ

        // Раздефьюз
        cts[0].DefuseBomb(bomb);

        round1.End();

        // MVP раунда
        var mvp = new Round.MVP(cts[4]);
        mvp.Show();

        // Статистика
        Console.WriteLine("\nСтатистика раунда:");
        foreach (var t in terrorists) t.ShowStats();
        foreach (var ct in cts) ct.ShowStats();
    }
}