using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public class ShootMissileObserver : InputObserver
    {
        public override void Notify()
        {
            Debug.WriteLine("Shoot Missile Observer");
            Ship pShip = ShipManager.GetCurrentShip();
            pShip.ShootMissile();


        }
    }
}