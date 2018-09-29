using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public class ShieldBrick : ShieldCategory
    {
        // Data: ---------------


        public ShieldBrick(GameObject.Name name, GameSprite.Name spriteName, int index, float posX, float posY)
            : base(name, spriteName, index, ShieldCategory.Type.ShieldBrick)
        {
            this.x = posX;
            this.y = posY;

            //this.SetCollisionColor(1.0f, 1.0f, 1.0f);
        }

        ~ShieldBrick()
        {
            #if(TRACK_DESTRUCTOR)
            Debug.WriteLine("~ShieldBrick():{0}", this.GetHashCode());
            #endif
        }

        public override void Update()
        {
            base.Update();
        }


        //--------------------------------------------------------------------------
        //collisions


        public override void Accept(ColVisitor other)
        {
            // Important: at this point we have an Alien
            // Call the appropriate collision reaction            
            other.VisitShieldBrick(this);
        }


        public override void VisitMissile(Missile m)
        {
            // Missile vs ShieldBrick
            //Debug.WriteLine(" ---> Done");
            ColPair pColPair = ColPairManager.GetActiveColPair();
            pColPair.SetCollision(m, this);
            pColPair.NotifyListeners();
        }


        public override void VisitBomb(Bomb b)
        {
            //Bomb vs ShieldBrick
            //Debug.WriteLine(" -------> END COLLISION: AlienBomb vs ShieldBrick <---------");
            ColPair collisionPair = ColPairManager.GetActiveColPair();
            collisionPair.SetCollision(b, this);
            collisionPair.NotifyListeners();
        }

    }
}