using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public abstract class MissileCategory : GameObject
    {
        public enum Type
        {
            Missile,
            MissileRoot,
            Unitialized
        }

        // this is just a placeholder, who knows what data will be stored here
        protected MissileCategory.Type type;

        protected MissileCategory(GameObject.Name name, GameSprite.Name spriteName, int index, MissileCategory.Type missileType)
            : base(name, spriteName, index)
        {
            this.type = missileType;
        }

        // Data: ---------------
        ~MissileCategory()
        {
            #if(TRACK_DESTRUCTOR)
            Debug.WriteLine("     ~MissileCategory():{0}", this.GetHashCode());
            #endif
        }

        static public GameObject GetMissile(GameObject objA, GameObject objB)
        {
            GameObject pMissile;
            if (objA is MissileCategory)
            {
                pMissile = (GameObject)objA;
            }
            else
            {
                pMissile = (GameObject)objB;
            }

            Debug.Assert(pMissile is MissileCategory);

            return pMissile;
        }




    }
}