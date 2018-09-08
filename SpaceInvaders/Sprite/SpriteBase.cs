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


    public abstract class SpriteBase : DLink
    {
        public float x;
        public float y;
        public float sx;
        public float sy;
        public float angle;

        public abstract void Update();
        public abstract void Draw();
    }
}