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