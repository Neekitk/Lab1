using System;

namespace Exceptions
{
    class NotEnoughMoneyException : Exception
    {
        public override string Message => "Недостаточно денег для покупки оружия!";
    }

    class BombAlreadyPlantedException : Exception
    {
        public override string Message => "Бомба уже установлена!";
    }
}
