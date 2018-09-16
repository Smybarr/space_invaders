using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public class CollisionRect : Azul.Rect
    {
        public CollisionRect(float x, float y, float width, float height)
            : base(x, y, width, height)
        {
        }
        public CollisionRect(Azul.Rect pRect)
            : base(pRect)
        {
        }
        public CollisionRect(CollisionRect pRect)
            : base(pRect)
        {
        }
        public CollisionRect()
            : base()
        {
        }
        public bool Intersect(CollisionRect ColRectA, CollisionRect ColRectB)
        {
            return true;
        }




        public void Union(CollisionRect ColRect)
        {
            float minX;
            float minY;
            float maxX;
            float maxY;

            if ((this.x - this.width / 2) < (ColRect.x - ColRect.width / 2))
            {
                minX = (this.x - this.width / 2);
            }
            else
            {
                minX = (ColRect.x - ColRect.width / 2);
            }

            if ((this.x + this.width / 2) > (ColRect.x + ColRect.width / 2))
            {
                maxX = (this.x + this.width / 2);
            }
            else
            {
                maxX = (ColRect.x + ColRect.width / 2);
            }

            if ((this.y + this.height / 2) > (ColRect.y + ColRect.height / 2))
            {
                maxY = (this.y + this.height / 2);
            }
            else
            {
                maxY = (ColRect.y + ColRect.height / 2);
            }

            if ((this.y - this.height / 2) < (ColRect.y - ColRect.height / 2))
            {
                minY = (this.y - this.height / 2);
            }
            else
            {
                minY = (ColRect.y - ColRect.height / 2);
            }

            this.width = (maxX - minX);
            this.height = (maxY - minY);
            this.x = minX + this.width / 2;
            this.y = minY + this.height / 2;
        }

    }
}