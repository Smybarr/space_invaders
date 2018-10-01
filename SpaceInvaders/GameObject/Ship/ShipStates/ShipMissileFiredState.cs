using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    //ship can only move left/right after missile has fired

    class ShipStateMissileFlying : ShipState
    {
        public override void Handle(Ship pShip)
        {

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

        }
    }
}