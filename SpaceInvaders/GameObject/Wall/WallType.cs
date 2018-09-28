using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public abstract class WallCategory : GameObject
    {
        public enum Type
        {
            WallRoot,
            Right,
            Left,
            Bottom,
            Top,
            Unitialized
        }

        // this is just a placeholder, who knows what data will be stored here
        protected WallCategory.Type type;

        protected WallCategory(GameObject.Name name, GameSprite.Name spriteName, int index, WallCategory.Type wallType)
            : base(name, spriteName, index)
        {
            this.type = wallType;
        }

        // Data: ---------------
        ~WallCategory()
        {
            #if(TRACK_DESTRUCTOR)
            Debug.WriteLine("     ~WallCategory():{0}", this.GetHashCode());
            #endif
        }


        public WallCategory.Type GetWallType()
        {
            return this.type;
        }



    }
}