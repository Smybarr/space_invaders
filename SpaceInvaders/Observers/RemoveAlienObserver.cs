using System;
using System.Diagnostics;


namespace SpaceInvaders
{
    public class RemoveAlienObserver : ColObserver
    {

        // Data: ---------------
        private GameObject alien;


        public RemoveAlienObserver()
        {
            this.alien = null;
        }

        public RemoveAlienObserver(RemoveAlienObserver a)
        {
            Debug.Assert(a != null);
            this.alien = a.alien;
        }



        public override void Notify()
        {
            //Delete Alien
            //Debug.WriteLine("RemoveShieldBrickObserver: {0} {1}", this.subject.gameObject_A, this.subject.gameObject_B);

            this.alien = (AlienType) this.pSubject.pObjB;
            Debug.Assert(this.alien != null);



            if (this.alien.markForDeath == false)
            {
                this.alien.markForDeath = true;

                //Delay
                RemoveAlienObserver observer = new RemoveAlienObserver(this);
                DelayedObjectManager.Attach(observer);
            }
            else
            {
                Debug.Assert(false);
            }
        }



        public override void Execute()
        {
            //if the alien that was removed was the last one in the column, delete the column it was assigned to!
            //Debug.WriteLine("alien {0} parentColumn {1}", this.pAlien, this.pAlien.pParent);
            GameObject parentA = (GameObject)this.alien;
            GameObject parentB = (GameObject)parentA.pParent;

            parentA.Remove();

            ////find the AlienGrid game object to increase the march speed;
            //alienGrid.IncreaseAlienMarchSpeed();
            //Debug.WriteLine("Increased Alien Grid March Speed");
            //Debug.WriteLine("Current MarchSpeed: {0}", AlienGrid.marchSpeed);

            //Grid.liveAlienCount--;
            //Grid.deadAlienCount++;

            //Debug.WriteLine("Live AlienCount: {0}", AlienGrid.liveAlienCount);
            //Debug.WriteLine("Dead AlienCount: {0}", AlienGrid.deadAlienCount);


            //TODO: Need a better way... 
            if (CheckNullChildObject(parentB) == true)
            {
                GameObject parentC = (GameObject)parentB.pParent;

                parentB.Remove();

                if (CheckNullChildObject(parentC) == true)
                {

                    parentC.Remove();
                }
            }
        }

        private bool CheckNullChildObject(GameObject parentObject)
        {
            //if child gameObject of parent is null return true;
            if (parentObject.pChild == null)
            {
                return true;
            }
            return false;
        }

    }
}
