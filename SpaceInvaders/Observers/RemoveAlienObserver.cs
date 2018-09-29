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

            GameObject targetAlien = (GameObject)this.alien;
            GameObject parentColumn = (GameObject)targetAlien.pParent;

            //make sure the parent is a column;
            Debug.Assert(parentColumn.GetName() == GameObject.Name.Column);

            //GameObject pAlienGridObj = GameObjectManager.Find()

            targetAlien.Remove();

            ////find the AlienGrid game object to increase the march speed;
            //alienGrid.IncreaseAlienMarchSpeed();
            //Debug.WriteLine("Increased Alien Grid March Speed");
            //Debug.WriteLine("Current MarchSpeed: {0}", AlienGrid.marchSpeed);

            //Grid.liveAlienCount--;
            //Grid.deadAlienCount++;

            //Debug.WriteLine("Live AlienCount: {0}", AlienGrid.liveAlienCount);
            //Debug.WriteLine("Dead AlienCount: {0}", AlienGrid.deadAlienCount);


            //TODO: Need a better way to check if this is last alien in column/last column in grid;
            //check if last alien in the column
             if (privIsLastChildOf(parentColumn) == true)
            {
                //if so, remove the parent column;

                //get the grid pointer before removing the column (in case this is the last column)
                GameObject parentAlienGrid = (GameObject)parentColumn.pParent;
                parentColumn.Remove();

                //double check that parentAlienGrid is actually an alien grid
                Debug.Assert(parentAlienGrid.GetName() == GameObject.Name.Grid);

                //cast to a grid object and decrement the number of columns;
                Grid alienGrid = (Grid) parentAlienGrid;
                alienGrid.DecrementColumnCount();
  

                //check if the last column in the grid
                if (privIsLastChildOf(parentAlienGrid) == true)
                {
                    //todo - place a next level reload or reward for beating the level here!!!
                    //if so, remove the grid and trigger a reaction
                    parentAlienGrid.Remove();
                }
            }
        }

        //check if this is the last child
        private bool privIsLastChildOf(GameObject parentObject)
        {
            //if this is the last child;
            //here that would be last alien in a column or last column in the grid;
            if (parentObject.pChild == null)
            {
                return true;
            }
            return false;
        }

    }
}
