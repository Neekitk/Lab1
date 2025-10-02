using System;
using Players;

namespace Rounds
{
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
}