using System;
using System.Diagnostics;


namespace SpaceInvaders
{
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