using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public class MoveRightObserver : InputObserver
    {
        public override void Notify()
        {
            //Debug.WriteLine("Move Right");

            //move the ship to the right
            Ship pShip = ShipManager.GetShip();
            pShip.MoveRight();
        }
    }
}