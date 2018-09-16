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

            if (this.x < ColRect.x)
            {
                minX = this.x;
            }
            else
            {
                minX = ColRect.x;
            }

            if ((this.x + this.width) > (ColRect.x + ColRect.width))
            {
                maxX = (this.x + this.width);
            }
            else
            {
                maxX = (ColRect.x + ColRect.width);
            }

            if (this.y > ColRect.y)
            {
                maxY = this.y;
            }
            else
            {
                maxY = ColRect.y;
            }

            if ((this.y - this.height) < (ColRect.y - ColRect.height))
            {
                minY = (this.y - this.height);
            }
            else
            {
                minY = (ColRect.y - ColRect.height);
            }


            this.x = minX;
            this.y = maxY;
            this.width = (maxX - minX);
            this.height = (maxY - minY);
        }

    }
}