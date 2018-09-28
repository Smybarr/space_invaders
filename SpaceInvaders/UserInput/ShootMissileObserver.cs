using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public class ShootMissileObserver : InputObserver
    {
        public override void Notify()
        {
            Debug.WriteLine("Shoot Missile Observer");
            Ship pShip = ShipManager.GetShip();
            pShip.ShootMissile();


            //TEST - drop bomb on trigger;
            
           //get the grid as a game object
            GameObject gridGameObj = GameObjectManager.Find(GameObject.Name.Grid);
            //cast to an alien to drop the bomb
            AlienType alienGrid = (AlienType) gridGameObj;

            //drop the bomb - function cascades to first child of first column
            alienGrid.DropBomb();


        }
    }
}