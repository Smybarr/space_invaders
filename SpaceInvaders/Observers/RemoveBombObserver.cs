using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public class RemoveBombObserver : ColObserver
    {
        // Data: ---------------
        private GameObject pBomb;


        public RemoveBombObserver()
        {
            this.pBomb = null;
        }

        public RemoveBombObserver(RemoveBombObserver b)
        {
            this.pBomb = b.pBomb;
        }



        public override void Notify()
        {
            // Delete missile
            //Debug.WriteLine("RemoveBombObserver: {0} {1}", this.pSubject.pObjA, this.pSubject.pObjB);

            //this.pBomb = BombCategory.GetBomb(this.pSubject.pObjA, this.pSubject.pObjB);
            this.pBomb = (Bomb) this.pSubject.pObjA;
            Debug.Assert(this.pBomb != null);
            //Debug.WriteLine("RemoveBombObserver: --> delete bomb {0}", pBomb);

            if (pBomb.markForDeath == false)
            {
                pBomb.markForDeath = true;
                //   Delay

                //todo replace this new with a find from object pool;
                RemoveBombObserver pObserver = new RemoveBombObserver(this);
                DelayedObjectManager.Attach(pObserver);
            }
        }

        public override void Execute()
        {
            // Let the gameObject architecture deal with this...
            // GameObject will remove both sprite AND colbox after removing the game object;
            this.pBomb.Remove();
        }




    }
}