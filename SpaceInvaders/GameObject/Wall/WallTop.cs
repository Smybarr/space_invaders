using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public class WallTop : WallCategory
    {
        // Data: ---------------




        public WallTop(GameObject.Name name, GameSprite.Name spriteName, int index, float posX, float posY, float width, float height)
            : base(name, spriteName, index, WallCategory.Type.Top)
        {
            this.poColObj.poColRect.Set(posX, posY, width, height);

            this.x = posX;
            this.y = posY;

            this.poColObj.pColSprite.SetLineColor(1, 1, 0);
        }

        ~WallTop()
        {
            #if(TRACK_DESTRUCTOR)
            Debug.WriteLine("~Top():{0}", this.GetHashCode());
            #endif
        }
        public override void Accept(ColVisitor other)
        {
            // Important: at this point we have an Alien
            // Call the appropriate collision reaction            
            other.VisitWallTop(this);
        }
        public override void Update()
        {
            // Go to first child
            base.Update();
        }

        public override void VisitMissileRoot(MissileRoot m)
        {
            // MissileRoot vs WallTop
            ColPair.Collide((GameObject)m.pChild, this);
        }

        public override void VisitMissile(Missile m)
        {
            // Missile vs WallTop
            //Debug.WriteLine(" ---> Done");
            ColPair pColPair = ColPairManager.GetActiveColPair();
            pColPair.SetCollision(m, this);
            pColPair.NotifyListeners();
        }

    


    }
}