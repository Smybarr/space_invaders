using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public abstract class InputObserver : MLink
    {
        // define this in concrete
        abstract public void Notify();

        public InputSubject pSubject;
    }
}