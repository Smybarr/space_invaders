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
            GameObject targetBrick = (GameObject)this.pBrick;
            GameObject parentColumn = (GameObject)targetBrick.pParent;

            targetBrick.Remove();

            // TODO: Need a better way... 
            //check if last brick in the column
            if (privIsLastChildOf(parentColumn) == true)
            {
                //if so, remove the parent column;

                //get the grid pointer before removing the column (in case this is the last column)
                GameObject parentShieldGrid = (GameObject)parentColumn.pParent;
                parentColumn.Remove();


                //check if the last column in the grid
                if (privIsLastChildOf(parentShieldGrid) == true)
                {
                    //if so, remove the grid pointer
                    parentShieldGrid.Remove();
                }

            }
        }



        //check if this is the last child
        private bool privIsLastChildOf(GameObject pObj)
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