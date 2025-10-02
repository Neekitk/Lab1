using System;
using Exceptions;

namespace Bombs
{
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
}