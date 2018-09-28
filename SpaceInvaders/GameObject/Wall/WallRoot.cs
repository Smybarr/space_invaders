using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public class WallRoot : WallCategory
    {
        // Data: ---------------


        public WallRoot(GameObject.Name name, GameSprite.Name spriteName, int index, float posX, float posY)
            : base(name, spriteName, index, WallCategory.Type.WallRoot)
        {
            this.x = posX;
            this.y = posY;

            this.poColObj.pColSprite.SetLineColor(1, 1, 1);
        }

        ~WallRoot()
        {
            #if(TRACK_DESTRUCTOR)
            Debug.WriteLine("~WallRoot():{0}", this.GetHashCode());
            #endif
        }

        public override void Accept(ColVisitor other)
        {
            // Important: at this point we have an Alien
            // Call the appropriate collision reaction            
            other.VisitWallRoot(this);
        }
        public override void Update()
        {
            // Go to first child
            base.baseUpdateBoundingBox();
            base.Update();
        }





        public override void VisitGrid(Grid a)
        {
            // AlienGrid vs WallRoot
            //     Debug.WriteLine("collide: {0} with {1}", this, w);

            // Grid vs WallRight
            ColPair.Collide(a, (GameObject)this.pChild);
        }




        public override void VisitMissileRoot(MissileRoot m)
        {
            // MissileRoot vs WallRoot
            ColPair.Collide((GameObject)m.pChild, this);
        }

        public override void VisitMissile(Missile m)
        {
            // Missile vs WallRoot
            ColPair.Collide(m, (GameObject)this.pChild);
        }






        public override void VisitBombRoot(BombRoot b)
        {
            // BombRoot vs WallRoot
            ColPair.Collide((GameObject)b.pChild, this);
        }

        public override void VisitBomb(Bomb b)
        {
            // Bomb vs WallRoot
            ColPair.Collide(b, (GameObject)this.pChild);
        }


    }
}