using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public class RemoveMissileObserver : ColObserver
    {
        // data
        private GameObject pMissile;


        public RemoveMissileObserver()
        {
            this.pMissile = null;
        }

        public RemoveMissileObserver(RemoveMissileObserver m)
        {
            this.pMissile = m.pMissile;
        }

        public override void Notify()
        {
            // Delete missile
            //Debug.WriteLine("RemoveMissileObserver: {0} {1}", this.pSubject.pObjA, this.pSubject.pObjB);

            this.pMissile = (Missile)this.pSubject.pObjA;
            Debug.Assert(this.pMissile != null);

            if (pMissile.markForDeath == false)
            {
                pMissile.markForDeath = true;
                //   Delay
                RemoveMissileObserver pObserver = new RemoveMissileObserver(this);
                DelayedObjectManager.Attach(pObserver);
            }
        }

        public override void Execute()
        {
            // Let the gameObject deal with this... 
            this.pMissile.Remove();
        }


    }
}