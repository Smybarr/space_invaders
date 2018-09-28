using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public class MissileRoot : MissileCategory
    {

        // Data: ---------------


        public MissileRoot(GameObject.Name name, GameSprite.Name spriteName, int index, float posX, float posY)
            : base(name, spriteName, index, MissileCategory.Type.MissileRoot)
        {
            this.x = posX;
            this.y = posY;

            this.poColObj.pColSprite.SetLineColor(0, 0, 1);
        }

        ~MissileRoot()
        {
            #if(TRACK_DESTRUCTOR)
            Debug.WriteLine("~MissileRoot():{0}", this.GetHashCode());
        #endif
        }

        public override void Accept(ColVisitor other)
        {
            // Important: at this point we have an Alien
            // Call the appropriate collision reaction            
            other.VisitMissileRoot(this);
        }
        public override void Update()
        {
            // Go to first child
            base.baseUpdateBoundingBox();
            base.Update();
        }






    }
}