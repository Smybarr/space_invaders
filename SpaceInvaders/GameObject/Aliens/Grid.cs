using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public class Grid : AlienType
    {

        // Data: ---------------
        private float delta;
        private float total;

        public Grid(GameObject.Name name, GameSprite.Name spriteName, int index, float posX, float posY)
            : base(name, spriteName, index, AlienType.Type.AlienGrid)
        {
            this.x = posX;
            this.y = posY;
            this.delta = 2.0f;
            this.total = 0.0f;
        }

        ~Grid()
        {
            #if(TRACK_DESTRUCTOR)
            Debug.WriteLine("~Grid():{0}", this.GetHashCode());
            #endif
        }
        public override void Update()
        {
            base.Update();
            this.MoveGrid();
        }

        public void MoveGrid()
        {
            // Initialize
            PCSTreeIterator pIterator = new PCSTreeIterator(this);
            Debug.Assert(pIterator != null);

            PCSNode pNode = pIterator.First();

            while (pNode != null)
            {
                // delta
                GameObject pGameObj = (GameObject)pNode;
                pGameObj.x += this.delta;

                // Advance
                pNode = pIterator.Next();
            }

            this.total += this.delta;

            if (this.total > 400.0f || this.total < 0.0f)
            {
                this.delta *= -1.0f;
            }
        }


    }
}