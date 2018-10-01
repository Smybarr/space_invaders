using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public class ShipReadyObserver : ColObserver
    {
        public override void Notify()
        {
            Ship pShip = ShipManager.GetCurrentShip();
            pShip.SetState(ShipManager.State.Ready);
        }

        // data


    }
}