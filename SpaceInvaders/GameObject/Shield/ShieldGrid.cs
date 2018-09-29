using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public class ShieldGrid : ShieldCategory
    {

        // Data: ---------------


        public ShieldGrid(GameObject.Name name, GameSprite.Name spriteName, int index, float posX, float posY)
            : base(name, spriteName, index, ShieldCategory.Type.ShieldGrid)
        {
            this.x = posX;
            this.y = posY;
        }

        ~ShieldGrid()
        {
            #if(TRACK_DESTRUCTOR)
            Debug.WriteLine("~ShieldGrid():{0}", this.GetHashCode());
            #endif
        }

        public override void Update()
        {
            // Go to first child
            base.baseUpdateBoundingBox();
            base.Update();
        }


        //--------------------------------------------------------------------------
        //collisions

        public override void Accept(ColVisitor other)
        {
            // Important: at this point we have an Alien
            // Call the appropriate collision reaction            
            other.VisitShieldGrid(this);
        }

        public override void VisitMissile(Missile m)
        {
            // Missile vs ShieldGrid
            ColPair.Collide(m, (GameObject)this.pChild);
        }

        public override void VisitBomb(Bomb b)
        {
            //AlienBomb vs ShieldGrid
            ColPair.Collide(b, (GameObject)this.pChild);
        }
    }
}