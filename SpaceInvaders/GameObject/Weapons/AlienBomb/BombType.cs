using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public abstract class BombCategory : GameObject
    {
        public enum Type
        {
            Bomb,

            RollingBomb,
            ZigZagBomb,
            CrossBomb,

            BombRoot,
            Unitialized
        }

        // Data: ---------------
        // this is just a placeholder, who knows what data will be stored here
        protected BombCategory.Type type;

        protected BombCategory(GameObject.Name name, GameSprite.Name spriteName, int index, BombCategory.Type bombType)
            : base(name, spriteName, index)
        {
            this.type = bombType;
        }


        ~BombCategory()
        {
            #if(TRACK_DESTRUCTOR)
            Debug.WriteLine("     ~BombCategory():{0}", this.GetHashCode());
            #endif
        }


        static public GameObject GetBomb(GameObject objA, GameObject objB)
        {
            GameObject pBomb;
            if (objA is BombCategory)
            {
                pBomb = (GameObject)objA;
            }
            else
            {
                pBomb = (GameObject)objB;
            }

            Debug.Assert(pBomb is BombCategory);

            return pBomb;
        }



    }
}