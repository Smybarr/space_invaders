using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public abstract class ShipCategory : GameObject
    {
        public enum Type
        {
            Ship,
            ShipRoot,
            Unitialized
        }

        protected ShipCategory(GameObject.Name name, GameSprite.Name spriteName, int index, ShipCategory.Type shipType)
            : base(name, spriteName, index)
        {
            this.type = shipType;
        }

        // Data: ---------------
        ~ShipCategory()
        {
#if(TRACK_DESTRUCTOR)
            Debug.WriteLine("     ~ShipCategory():{0}", this.GetHashCode());
#endif
        }

        // this is just a placeholder, who knows what data will be stored here
        protected ShipCategory.Type type;

    }
}