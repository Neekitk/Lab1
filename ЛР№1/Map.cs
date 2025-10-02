using System;

namespace Maps
{
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
}