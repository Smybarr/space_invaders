using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public class RemoveBrickObserver : ColObserver
    {
        // Data: ---------------
        private GameObject pBrick;


        public RemoveBrickObserver()
        {
            this.pBrick = null;
        }

        public RemoveBrickObserver(RemoveBrickObserver b)
        {
            Debug.Assert(b != null);
            this.pBrick = b.pBrick;
        }




        public override void Notify()
        {
            // Delete missile
            //Debug.WriteLine("RemoveMissileObserver: {0} {1}", this.pSubject.pObjA, this.pSubject.pObjB);

            this.pBrick = (ShieldBrick)this.pSubject.pObjB;
            Debug.Assert(this.pBrick != null);

            if (pBrick.markForDeath == false)
            {
                pBrick.markForDeath = true;
                //   Delay
                RemoveBrickObserver pObserver = new RemoveBrickObserver(this);
                DelayedObjectManager.Attach(pObserver);
            }
        }



        public override void Execute()
        {
            //  if this brick removed the last child in the column, then remove column
            // Debug.WriteLine(" brick {0}  parent {1}", this.pBrick, this.pBrick.pParent);
            GameObject pA = (GameObject)this.pBrick;
            GameObject pB = (GameObject)pA.pParent;

            pA.Remove();

            // TODO: Need a better way... 
            if (privCheckParent(pB) == true)
            {
                GameObject pC = (GameObject)pB.pParent;
                pB.Remove();

                if (privCheckParent(pC) == true)
                {
                    pC.Remove();
                }

            }
        }




        private bool privCheckParent(GameObject pObj)
        {
            if (pObj.pChild == null)
            {
                return true;
            }

            return false;
        }


    }
}





//public override void Execute()
//    {
//        // if this brick removed the last child in the column, then remove column
//        GameObject pA = (GameObject)this.pBrick.pParent;
//        Debug.Assert(pA != null);

//        // Let the gameObject deal with this...
//        this.pBrick.Remove();


//        PCSTree pTree = GameObjectMan.GetRootTree();
//        pTree.dumpTree();


//        if(pA.pChild == null)
//        {
//            // We just delete the last child
//            // So delete the column

//            GameObject pB = (GameObject)pA.pParent;
//            pA.Remove();

//            pTree.dumpTree();

//            if(pB.pChild == null)
//            {
//                pB.Remove();

//                pTree.dumpTree();
//            }
//        }
//    }