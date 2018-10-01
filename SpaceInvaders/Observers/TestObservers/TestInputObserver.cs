using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public class TestInputObserver : InputObserver
    {
        //Data-------------------


        public TestInputObserver()
        {
            //initialize any data here;

        }


        public override void Notify()
        {
            //call any test functions here by pressing the T key;
            
            //-------------------------------------------
            //test getting a new ship if old one is blown up;
            Debug.WriteLine("TEST INPUT OBSERVER - Test ShipManager.GetNextShip()");            
            privTestGetNextShip();
            Debug.WriteLine("END TEST ShipManager.GetNextShip()\n\n");
            ////--------------------------------------------
            ////test alien column bomb drop
            //privTestAlienColumnBombDrop();

        }


        private void privTestGetNextShip()
        {
            ShipManager.GetNextShip();
        }




        private void privTestAlienColumnBombDrop()
        {
            //TEST - drop bomb on trigger;

            //get the grid as a game object
            GameObject gridGameObj = GameObjectManager.Find(GameObject.Name.Grid);
            //cast to an alien to drop the bomb
            AlienType alienGrid = (AlienType)gridGameObj;

            //drop the bomb - function cascades to first child of first column
            alienGrid.DropBomb();
        }

    }
}