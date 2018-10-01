using System;
using System.Diagnostics;


namespace SpaceInvaders
{
    public class Ship : ShipCategory
    {
        // Data: --------------------
        public float shipSpeed;
        private ShipState state;



        public Ship(GameObject.Name name, GameSprite.Name spriteName, int index, float posX, float posY)
            : base(name, spriteName, index, ShipCategory.Type.Ship)
        {
            this.x = posX;
            this.y = posY;

            this.shipSpeed = 3.0f;
            this.state = null;
        }

        public override void Update()
        {
            base.Update();
        }

        ////TEST----NOT GURANTEED TO WORK!!
        //public override void Remove()
        //{
        //    // Since the Root object is being drawn
        //    // 1st set its size to zero
        //    this.poColObj.poColRect.Set(0, 0, 0, 0);
        //    this.x = 0.0f;
        //    this.y = 0.0f;
        //    //this.SetState(ShipManager.State.End);
        //    base.Update();

        //    // Update the parent (Ship root)
        //    GameObject pParent = (GameObject)this.pParent;
        //    pParent.Update();

        //    // Now remove it
        //    base.Remove();
        //}

        public override void Accept(ColVisitor other)
        {
            // Important: at this point we have an Bomb
            // Call the appropriate collision reaction
            other.VisitShip(this);
        }

        public void MoveRight()
        {
            this.state.MoveRight(this);
        }

        public void MoveLeft()
        {
            this.state.MoveLeft(this);
        }

        public void ShootMissile()
        {
            this.state.ShootMissile(this);
        }

        public void SetState(ShipManager.State inState)
        {
            this.state = ShipManager.GetState(inState);
        }


    }
}