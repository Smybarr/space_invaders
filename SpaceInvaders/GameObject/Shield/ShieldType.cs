using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public abstract class ShieldCategory : GameObject
    {
        // this is just a placeholder, who knows what data will be stored here
        protected ShieldCategory.Type type;


        public enum Type
        {
            Root,

            ShieldGrid,

            ShieldColumn,

            ShieldBrick,

            ShieldBrickLeft_Top,
            ShieldBrickRight_Top,
            ShieldBrickMidLeft_Bottom,
            ShieldBrickMid_Bottom,
            ShieldBrickMidRight_Bottom,


            Blank
        }

        protected ShieldCategory(GameObject.Name name, GameSprite.Name spriteName, int index, ShieldCategory.Type shieldType)
            : base(name, spriteName, index)
        {
            this.type = shieldType;
        }

        // Data: ---------------
        ~ShieldCategory()
        {
            #if(TRACK_DESTRUCTOR)
            Debug.WriteLine("     ~ShieldCategory():{0}", this.GetHashCode());
            #endif
        }

        public ShieldCategory.Type GetCategoryType()
        {
            return this.type;
        }



    }
}