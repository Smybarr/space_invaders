using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public class Octopus : AlienType
    {
        public Octopus(GameObject.Name name, GameSprite.Name spriteName, int index, float posX, float posY)
            : base(name, spriteName, index, AlienType.Type.Octopus)
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