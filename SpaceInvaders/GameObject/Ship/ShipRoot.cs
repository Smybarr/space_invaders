using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public class ShipRoot : ShipCategory
    {
        public ShipRoot(GameObject.Name name, GameSprite.Name spriteName, int index, float posX, float posY)
            : base(name, spriteName, index, ShipCategory.Type.ShipRoot)
        {
            this.x = posX;
            this.y = posY;

            this.poColObj.pColSprite.SetLineColor(0, 0, 1);
        }

        ~ShipRoot()
        {
#if(TRACK_DESTRUCTOR)
            Debug.WriteLine("~MissileRoot():{0}", this.GetHashCode());
#endif
        }

        public override void Accept(ColVisitor other)
        {
            // Important: at this point we have an Alien
            // Call the appropriate collision reaction            
            other.VisitShipRoot(this);
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