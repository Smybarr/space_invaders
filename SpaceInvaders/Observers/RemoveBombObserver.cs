using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public class RemoveBombObserver : ColObserver
    {
        // data
        private GameObject pBomb;

        public RemoveBombObserver()
        {
            this.pBomb = null;
        }

        public RemoveBombObserver(RemoveBombObserver m)
        {
            this.pBomb = m.pBomb;
        }

        public override void Notify()
        {
            // Delete missile
            Debug.WriteLine("RemoveBombObserver: {0} {1}", this.pSubject.pObjA, this.pSubject.pObjB);

            this.pBomb = BombCategory.GetBomb(this.pSubject.pObjA, this.pSubject.pObjB);
            Debug.WriteLine("RemoveBombObserver: --> delete bomb {0}", pBomb);

            if (pBomb.markForDeath == false)
            {
                pBomb.markForDeath = true;
                //   Delay
                RemoveBombObserver pObserver = new RemoveBombObserver(this);
                DelayedObjectManager.Attach(pObserver);
            }
        }

        public override void Execute()
        {
            // Let the gameObject deal with this... 
            this.pBomb.Remove();
        }




    }
}