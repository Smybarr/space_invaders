using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public class DropZigZag : DropStrategy
    {
        // Data
        private float oldPosY;


        public DropZigZag()
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

            float targetY = oldPosY - 1.0f * pBomb.GetBoundingBoxHeight();

            if (pBomb.y < targetY)
            {
                pBomb.MultiplyScale(-1.0f, 1.0f);
                oldPosY = targetY;
            }
        }


    }
}