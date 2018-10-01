using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public abstract class ColObserver : MLink
    {

        public ColSubject pSubject;


        public abstract void Notify();

        public virtual void Execute()
        {
            // default implementation
        }



    }
}