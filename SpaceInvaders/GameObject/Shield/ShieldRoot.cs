using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public class ShieldRoot : ShieldCategory
    {
        // Data: ---------------


        public ShieldRoot(GameObject.Name name, GameSprite.Name spriteName, int index, float posX, float posY)
            : base(name, spriteName, index, ShieldCategory.Type.Root)
        {
            this.x = posX;
            this.y = posY;

        }

        ~ShieldRoot()
        {
#if(TRACK_DESTRUCTOR)
            Debug.WriteLine("~ShieldRoot():{0}", this.GetHashCode());
#endif
        }

        public override void Accept(ColVisitor other)
        {
            // Important: at this point we have an Alien
            // Call the appropriate collision reaction            
            other.VisitShieldRoot(this);
        }
        public override void Update()
        {
            // Go to first child
            base.baseUpdateBoundingBox();
            base.Update();
        }

        //--------------------------------------------------------------------------
        //collisions


        public override void VisitMissileRoot(MissileRoot m)
        {
            // MissileRoot vs ShieldRoot
            ColPair.Collide((GameObject)m.pChild, this);
        }

        public override void VisitMissile(Missile m)
        {
            // Missile vs ShieldRoot
            ColPair.Collide(m, (GameObject)this.pChild);
        }


        public override void VisitBombRoot(BombRoot b)
        {
            //AlienBombRoot vs ShieldRoot
            ColPair.Collide((GameObject)b.pChild, this);
        }

        public override void VisitBomb(Bomb b)
        {
            //AlienBomb vs ShieldColumn
            ColPair.Collide(b, (GameObject)this.pChild);
        }

    }
}