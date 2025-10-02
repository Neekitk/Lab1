using System;
using Players;

namespace Interfaces
{
    interface IMovable
    {
        void Move();
    }

    interface IDamageable
    {
        void TakeDamage(int amount, Player attacker = null);
    }

    interface IShootable
    {
        void Shoot(Player target);
    }
}