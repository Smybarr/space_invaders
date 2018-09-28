using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public class DropStraight : DropStrategy
    {
        // Data
        private float oldPosY;

        public DropStraight()
        {
            this.oldPosY = 0.0f;
        }

        public override void ResetData(float posY)
        {
            this.oldPosY = posY;
        }

        public override void Fall(Bomb pBomb)
        {
            Debug.Assert(pBomb != null);

            // Do nothing for this strategy
        }


    }
}