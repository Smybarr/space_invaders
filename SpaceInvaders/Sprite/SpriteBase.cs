﻿using System;
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

        //public float x;
        //public float y;
        //public float sx;
        //public float sy;
        //public float angle;

        public SpriteBase()
            : base()
        {
            this.render = true;

            ////moved to child classes due to proxy pattern
            //this.x = 0.0f;
            //this.y = 0.0f;
            //this.sx = 1.0f;
            //this.sy = 1.0f;
            //this.angle = 0.0f;
        }
        ~SpriteBase()
        {
#if (TRACK_DESTRUCTOR)
            Debug.WriteLine("      ~SpriteBase():{0} ", this.GetHashCode());
#endif
        }

        protected void baseDumpSprite()
        {
            //moved to child classes due to proxy
            //Debug.WriteLine("           (x,y): {0} {1}", this.x, this.y);
            //Debug.WriteLine("         (sx,sy): {0} {1}", this.sx, this.sy);
            //Debug.WriteLine("           angle: {0}", this.angle);
        }

        abstract public Enum GetSpriteName();
        public abstract void Update();
        public abstract void Draw();
    }
}