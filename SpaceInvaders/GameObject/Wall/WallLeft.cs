using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public class WallLeft : WallCategory
    {
        public WallLeft(GameObject.Name name, GameSprite.Name spriteName, int index, float posX, float posY, float width, float height)
            : base(name, spriteName, index, WallCategory.Type.Left)
        {
            this.poColObj.poColRect.Set(posX, posY, width, height);

            this.x = posX;
            this.y = posY;

            this.poColObj.pColSprite.SetLineColor(1, 1, 0);
        }

        ~WallLeft()
        {
            #if(TRACK_DESTRUCTOR)
            Debug.WriteLine("~Right():{0}", this.GetHashCode());
            #endif
        }

        public override void Accept(ColVisitor other)
        {
            // Important: at this point we have an Alien
            // Call the appropriate collision reaction            
            other.VisitWallLeft(this);
        }
        public override void Update()
        {
            // Go to first child
            base.Update();
        }


        public override void VisitGrid(Grid a)
        {
            // AlienGrid vs WallRight
            //       Debug.WriteLine("collide: {0} with {1}", this, a);
            Debug.WriteLine("   --->DONE<----");

            //a.SetDelta(2.0f);

            ColPair pColPair = ColPairManager.GetActiveColPair();
            Debug.Assert(pColPair != null);
            pColPair.SetCollision(a, this);
            pColPair.NotifyListeners();

        }
        // Data: ---------------


    }
}