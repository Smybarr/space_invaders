using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    class BombObserver : ColObserver
    {

        public override void Notify()
        {
            //Debug.WriteLine("BombObserver: {0} {1}", this.pSubject.pObjA, this.pSubject.pObjB);
            Bomb pBomb = (Bomb)this.pSubject.pObjA;
            pBomb.Reset();

        }

        // Data

    }
}