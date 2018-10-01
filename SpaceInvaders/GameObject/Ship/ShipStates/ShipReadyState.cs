using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    //ship can move left, move right, and fire missile in ready state
    public class ShipStateReady : ShipState
    {
        public override void Handle(Ship pShip)
        {
            pShip.SetState(ShipManager.State.MissileFlying);
        }

        public override void MoveRight(Ship pShip)
        {
            pShip.x += pShip.shipSpeed;
        }

        public override void MoveLeft(Ship pShip)
        {
            pShip.x -= pShip.shipSpeed;
        }

        public override void ShootMissile(Ship pShip)
        {
            Missile pMissile = ShipManager.ActivateMissile();

            pMissile.SetPos(pShip.x, pShip.y + 20);
            //pMissile.SetActive(true);

            // switch states
            this.Handle(pShip);
        }

    }
}