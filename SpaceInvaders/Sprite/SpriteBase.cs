using System;
using System.Diagnostics;


namespace SpaceInvaders
{
    /* Patterns Incorporated
     *
     * Flyweight Pattern
     *      - Purpose: reduce memory usage by sharing as much
     *          common data as possible
     */


    public abstract class SpriteBase : MLink
    {
        public Boolean render;

        public float x;
        public float y;
        public float sx;
        public float sy;
        public float angle;

        public SpriteBase()
            : base()
        {
            this.render = true;

            this.x = 0.0f;
            this.y = 0.0f;
            this.sx = 1.0f;
            this.sy = 1.0f;
            this.angle = 0.0f;
        }

        public abstract void Update();
        public abstract void Draw();
    }
}