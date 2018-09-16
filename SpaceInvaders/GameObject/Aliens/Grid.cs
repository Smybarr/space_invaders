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
            // Go to first child
            PCSNode pNode = (PCSNode)this;
            pNode = pNode.pChild;

            // Set ColTotal to first child
            GameObject pGameObj = (GameObject)pNode;

            CollisionRect ColTotal = this.poColObj.poColRect;
            ColTotal.Set(pGameObj.poColObj.poColRect);

            // loop through sliblings
            while (pNode != null)
            {
                pGameObj = (GameObject)pNode;
                ColTotal.Union(pGameObj.poColObj.poColRect);

                // go to next sibling
                pNode = pNode.pSibling;
            }

            //this.pColObj.pColRect.Set(201, 201, 201, 201);
            this.x = this.poColObj.poColRect.x;
            this.y = this.poColObj.poColRect.y;

           //Debug.WriteLine("x:{0} y:{1} w:{2} h:{3}", ColTotal.x, ColTotal.y, ColTotal.width, ColTotal.height);

            base.Update();
        }





        //public override void Update()
        //{
        //    base.Update();
        //    this.MoveGrid();
        //}

        //public void MoveGrid()
        //{
        //    // Initialize
        //    PCSTreeIterator pIterator = new PCSTreeIterator(this);
        //    Debug.Assert(pIterator != null);

        //    PCSNode pNode = pIterator.First();

        //    while (pNode != null)
        //    {
        //        // delta
        //        GameObject pGameObj = (GameObject)pNode;
        //        pGameObj.x += this.delta;

        //        // Advance
        //        pNode = pIterator.Next();
        //    }

        //    this.total += this.delta;

        //    if (this.total > 400.0f || this.total < 0.0f)
        //    {
        //        this.delta *= -1.0f;
        //    }
        //}


    }
}