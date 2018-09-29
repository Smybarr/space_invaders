using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public class Bomb : BombCategory
    {
        // Data
        public float fallRate;
        private DropStrategy pStrategy;

        public Bomb(GameObject.Name name, GameSprite.Name spriteName, DropStrategy strategy, int index, float posX, float posY)
            : base(name, spriteName, index, BombCategory.Type.Bomb)
        {
            this.x = posX;
            this.y = posY;
            this.fallRate = 4.0f;

            Debug.Assert(strategy != null);
            this.pStrategy = strategy;

            this.pStrategy.ResetData(this.y);

            this.poColObj.pColSprite.SetLineColor(1, 1, 0);
        }
        ~Bomb()
        {
            #if (TRACK_DESTRUCTOR)
            Debug.WriteLine("~Bomb():{0}", this.GetHashCode());
            #endif
        }
        public void Reset()
        {
            this.y = 800.0f;
            this.pStrategy.ResetData(this.y);
        }


        public float GetBoundingBoxHeight()
        {
            return this.poColObj.poColRect.height;
        }


        public override void Remove()
        {
            // Since the Root object is being drawn
            // 1st set its size to zero
            this.poColObj.poColRect.Set(0, 0, 0, 0);
            base.Update();

            // Update the parent (bomb root)
            GameObject pParent = (GameObject)this.pParent;
            pParent.Update();

            // Now remove it
            base.Remove();
        }

        public override void Update()
        {
            base.Update();
            //this dictates that all bombs fall at the same rate
            this.y -= fallRate;

            this.pStrategy.Fall(this);
        }

        public void SetPos(float xPos, float yPos)
        {
            this.x = xPos;
            this.y = yPos;
        }


        public void MultiplyScale(float sx, float sy)
        {
            Debug.Assert(this.pProxySprite != null);

            this.pProxySprite.sx *= sx;
            this.pProxySprite.sy *= sy;
        }




        //--------------------------------------------------------------------------
        //collisions


        public override void Accept(ColVisitor other)
        {
            // Important: at this point we have an Alien
            // Call the appropriate collision reaction            
            other.VisitBomb(this);
        }
    }
}