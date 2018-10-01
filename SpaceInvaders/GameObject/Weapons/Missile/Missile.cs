using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public class Missile : MissileCategory
    {

        // Data
        //private bool enable;
        public float delta;

        public Missile(GameObject.Name name, GameSprite.Name spriteName, int index, float posX, float posY)
            : base(name, spriteName, index, MissileCategory.Type.Missile)
        {
            this.x = posX;
            this.y = posY;
            //this.enable = false;
            this.delta = 5.0f;
        }

        public override void Remove()
        {
            // Since the Root object is being drawn
            // 1st set its size to zero
            this.poColObj.poColRect.Set(0, 0, 0, 0);
            base.Update();

            // Update the parent (missile root)
            GameObject pParent = (GameObject)this.pParent;
            pParent.Update();

            // Now remove it
            base.Remove();
        }



        ////prototype state pattern with missiles...
        //public void SetActive(bool state)
        //{
        //    this.enable = state;
        //}

        public override void Update()
        {
            base.Update();
            this.y += delta;
        }


        ~Missile()
        {
            #if(TRACK_DESTRUCTOR)
            Debug.WriteLine("~Missile():{0}", this.GetHashCode());
            #endif
        }

        public override void Accept(ColVisitor other)
        {
            // Important: at this point we have an Alien
            // Call the appropriate collision reaction            
            other.VisitMissile(this);
        }

        public void SetPos(float xPos, float yPos)
        {
            this.x = xPos;
            this.y = yPos;
        }




    }

}