using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public class ShipRemoveMissileObserver : ColObserver
    {
        // data
        private GameObject pMissile;

        public ShipRemoveMissileObserver()
        {
            this.pMissile = null;
        }

        public ShipRemoveMissileObserver(ShipRemoveMissileObserver m)
        {
            this.pMissile = m.pMissile;
        }

        public override void Notify()
        {
            // Delete missile
            Debug.WriteLine("ShipRemoveMissileObserver: {0} {1}", this.pSubject.pObjA, this.pSubject.pObjB);

            this.pMissile = MissileCategory.GetMissile(this.pSubject.pObjA, this.pSubject.pObjB);
            Debug.WriteLine("MissileRemoveObserver: --> delete missile {0}", pMissile);

            if (pMissile.markForDeath == false)
            {
                pMissile.markForDeath = true;
                //   Delay
                ShipRemoveMissileObserver pObserver = new ShipRemoveMissileObserver(this);
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