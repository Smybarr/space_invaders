using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public abstract class DropStrategy
    {
        abstract public void Fall(Bomb pBomb);
        abstract public void ResetData(float posY);

    }
}