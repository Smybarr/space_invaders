using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public class Squid : AlienType
    {

        public Squid(GameObject.Name name, GameSprite.Name spriteName, int index, float posX, float posY)
            : base(name, spriteName, index, AlienType.Type.Squid)
        {
            this.x = posX;
            this.y = posY;
        }

        ~Squid()
        {
            #if(TRACK_DESTRUCTOR)
            Debug.WriteLine("~AlienCrab():{0}", this.GetHashCode());
            #endif
        }

    }
}