using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public abstract class AlienType : GameObject
    {
        public enum Type
        {
            AlienRoot,

            AlienGrid,

            AlienGridColumn,

            AlienUFO,

            Squid,
            Crab,
            Octopus,

            AlienExplosion,

            Blank,

        }



        //placeholder data - will eventually change
        protected Type alienType;

        protected AlienType(GameObject.Name name, GameSprite.Name spriteName, Type alienType)
            : base(name, spriteName)
        {
            this.alienType = alienType;
        }

        // Data: ---------------
        ~AlienType()
        {
            #if(TRACK_DESTRUCTOR)
            Debug.WriteLine("     ~Alien():{0}", this.GetHashCode());
            #endif
        }

        public Type GetAlienType()
        {
            return this.alienType;
        }

    }
}