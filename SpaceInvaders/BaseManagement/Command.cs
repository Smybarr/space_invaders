using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public abstract class Command
    {
        // define this in concrete 
        public abstract void Execute(float deltaTime);
    }
}
