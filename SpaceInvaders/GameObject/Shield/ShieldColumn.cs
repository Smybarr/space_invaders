using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public class ShieldColumn : ShieldCategory
    {

        // Data: ---------------



        public ShieldColumn(GameObject.Name name, GameSprite.Name spriteName, int index, float posX, float posY)
            : base(name, spriteName, index, ShieldCategory.Type.ShieldColumn)
        {
            this.x = posX;
            this.y = posY;
        }

        ~ShieldColumn()
        {
#if(TRACK_DESTRUCTOR)
            Debug.WriteLine("~ShieldColumn():{0}", this.GetHashCode());
#endif
        }



        public override void Update()
        {
            base.baseUpdateBoundingBox();
            base.Update();
        }


        //--------------------------------------------------------------------------
        //collisions

        public override void Accept(ColVisitor other)
        {
            // Important: at this point we have an Alien
            // Call the appropriate collision reaction            
            other.VisitShieldColumn(this);
        }

        public override void VisitMissile(Missile m)
        {
            // Missile vs ShieldColumn
            ColPair.Collide(m, (GameObject)this.pChild);
        }

        public override void VisitBomb(Bomb b)
        {
            //AlienBomb vs ShieldColumn
            ColPair.Collide(b, (GameObject)this.pChild);
        }

    }
}