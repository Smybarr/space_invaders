using System;
using System.Diagnostics;


namespace SpaceInvaders
{
    public abstract class SpriteBase : DLink
    {
        // Create a single sprite and all dynamic objects ONCE and ONLY ONCE (OOO- tm)
        public SpriteBase()
            : base()
        {
            this.x = 0.0f;
            this.y = 0.0f;
            this.sx = 1.0f;
            this.sy = 1.0f;
            this.angle = 0.0f;
        }

        abstract public void Update();
        abstract public void Draw();

        // Data: -------------------------------------------

        public float x;
        public float y;
        public float sx;
        public float sy;
        public float angle;
    }
}