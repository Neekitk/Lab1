using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
