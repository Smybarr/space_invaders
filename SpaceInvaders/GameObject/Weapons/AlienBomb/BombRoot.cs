using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public class BombRoot : BombCategory
    {
        public BombRoot(GameObject.Name name, GameSprite.Name spriteName, int index, float posX, float posY)
            : base(name, spriteName, index, BombCategory.Type.BombRoot)
        {
            this.x = posX;
            this.y = posY;

            //this.poColObj.pColSprite.SetLineColor(0.0f, 0.0f, 0.0f, 0.0f);
            //this.poColObj.pColSprite.SetLineColor(1, 1, 1);
        }

        ~BombRoot()
        {
            #if(TRACK_DESTRUCTOR)
            Debug.WriteLine("~BombRoot():{0}", this.GetHashCode());
            #endif
        }

        public override void Accept(ColVisitor other)
        {
            // Important: at this point we have an Alien
            // Call the appropriate collision reaction            
            other.VisitBombRoot(this);
        }
        public override void Update()
        {
            // Go to first child
            base.baseUpdateBoundingBox();
            base.Update();
        }



        // Data: ---------------


    }
}