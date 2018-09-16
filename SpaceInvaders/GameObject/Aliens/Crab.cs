using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public class Crab : AlienType
    {

        public Crab(GameObject.Name name, GameSprite.Name spriteName, float posX, float posY)
            : base(name, spriteName, AlienType.Type.Crab)
        {
            this.x = posX;
            this.y = posY;
        }

        ~Crab()
        {
#if(TRACK_DESTRUCTOR)
            Debug.WriteLine("~Crab():{0}", this.GetHashCode());
#endif
        }

    }
}
