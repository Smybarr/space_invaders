using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public class Octopus : AlienType
    {
        public Octopus(GameObject.Name name, GameSprite.Name spriteName, float posX, float posY)
            : base(name, spriteName)
        {
            this.x = posX;
            this.y = posY;
        }

        ~Octopus()
        {
            #if(TRACK_DESTRUCTOR)
            Debug.WriteLine("~Octopus():{0}", this.GetHashCode());
            #endif
        }

    }
}