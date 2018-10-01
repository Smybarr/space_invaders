using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public class Column : AlienType
    {

        public Column(GameObject.Name name, GameSprite.Name spriteName, int index, float posX, float posY)
            : base(name, spriteName, index, AlienType.Type.AlienGridColumn)
        {
            this.x = posX;
            this.y = posY;

            this.poColObj.pColSprite.SetLineColor(0, 0, 1);
        }

        ~Column()
        {
            #if(TRACK_DESTRUCTOR)
            Debug.WriteLine("~Column():{0}", this.GetHashCode());
            #endif
        }




        //called from column ( (GameObject)this.pParent );
        //passed to lowest alien ( (GameObject)this.pChild );
        public override void DropBomb()
        {
            //get the lowest alien - in PCS tree structure that's the child of this column;
            GameObject childAlien = (GameObject)this.pChild;

            //cast to an alien object;
            AlienType lowAlien = (AlienType) childAlien;
            
            //drop the bomb from the appropriate spot;
            lowAlien.DropBomb();
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
            other.VisitColumn(this);
        }

        public override void VisitMissileRoot(MissileRoot m)
        {
            // AlienColumn vs MissileRoot
            //       Debug.WriteLine("collide: {0} with {1}", this, m);

            // MissileRoot vs Aliens
            ColPair.Collide(m, (GameObject)this.pChild);
        }

        public override void VisitMissile(Missile m)
        {
            //Missile vs AlienColumn
            ColPair.Collide(m, (GameObject)this.pChild);
        }
    }
}