using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public class Grid : AlienType
    {

        // Data: ---------------
        private float delta;
        private float total;

        private float marchSpeed;

        private int numColumns;

        public Grid(GameObject.Name name, GameSprite.Name spriteName, int index, float posX, float posY)
            : base(name, spriteName, index, AlienType.Type.AlienGrid)
        {
            this.x = posX;
            this.y = posY;
            this.delta = 2.0f;
            this.total = 0.0f;
            this.numColumns = 0;
        }

        ~Grid()
        {
            #if (TRACK_DESTRUCTOR)
            Debug.WriteLine("~Grid():{0}", this.GetHashCode());
            #endif
            this.delta = 2.0f;
            this.total = 0.0f;
            this.numColumns = 0;
        }

        public override void Accept(ColVisitor other)
        {
            // Important: at this point we have an Alien
            // Call the appropriate collision reaction            
            other.VisitGrid(this);
        }

        public override void VisitMissileRoot(MissileRoot m)
        {
            // AlienGrid vs MissileRoot
            //     Debug.WriteLine("collide: {0} with {1}", this, m);

            // MissileRoot vs Columns
            ColPair.Collide(m, (GameObject)this.pChild);
        }

        public override void VisitWallRoot(WallRoot w)
        {
            // AlienGrid vs WallRoot
            //     Debug.WriteLine("collide: {0} with {1}", this, w);

            // WallRight vs Grid
            ColPair.Collide(this, (GameObject)w.pChild);
        }

        public override void DropBomb()
        {
            //get a random existing column;
            //needs to create a new random every time to
            //keep up with current number of columns;
            Random r = new Random();
            int randomColumnIndex = r.Next(0, this.numColumns);


            ////check that the child exists - can't drop a bomb if no columns/aliens!
            //Debug.Assert(randomChildColumn != null);
            ////cast to an alien object;
            //AlienType randomColumn = (AlienType)randomChildColumn;
            //todo Clean this up - remove if-else special case for first column bomb drop - without this first column never drops a bomb
            if (randomColumnIndex != 0)
            {
                AlienType randomColumn = privGetRandomColumn(randomColumnIndex);
                Debug.Assert(randomColumn != null);
                //drop the bomb from the appropriate spot;
                //call passed through this->column->alien
                randomColumn.DropBomb();
            }
            else
            {
                GameObject firstColumnObj = (GameObject)this.pChild;
                AlienType firstColumn = (AlienType) firstColumnObj;

                Debug.Assert(firstColumn != null);
                firstColumn.DropBomb();
            }
        }


        //todo: FIX THE LINKS WHEN THE ALIEN COLUMN IS REMOVED!!!!
        private AlienType privGetRandomColumn(int columnIndex)
        {
            GameObject randomChildColumn = (GameObject)this.pChild;
            //check that the child exists - can't drop a bomb if no columns/aliens!
            Debug.Assert(randomChildColumn != null);

            GameObject pNext = (GameObject)randomChildColumn.pSibling;

            //null check for early out
            if (pNext != null)
            {
                //iterate through siblings of child column until at index of random child;
                for (int i = 0; i < columnIndex-1; i++)
                {
                    //set next as sibling
                    pNext = (GameObject)pNext.pSibling;
                }

                //set the random child as the last column selected
                AlienType result = (AlienType)pNext;
                Debug.Assert(result != null);
                return result;
            }
            else
            {
                //no siblings - only one column
                AlienType result = (AlienType)randomChildColumn;
                Debug.Assert(result != null);
                return result;
            }

        }



        public void IncreaseAlienMarchSpeed()
        {
            if (this.marchSpeed > 0.23f)
            {
                this.marchSpeed -= 0.02f;
            }
            else
            {
                this.marchSpeed -= 0.01f;
            }
        }


        public void MoveGrid()
        {
            // Initialize
            PCSTreeForwardIterator pIterator = new PCSTreeForwardIterator(this);
            Debug.Assert(pIterator != null);

            PCSNode pNode = pIterator.First();

            while (!pIterator.IsDone())
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


        public float GetDelta()
        {
            return this.delta;
        }

        public void SetDelta(float inDelta)
        {
            this.delta = inDelta;
        }

        public int GetNumColumns()
        {
            return this.numColumns;
        }

        public void SetNumColumns(int columnCount)
        {
            this.numColumns = columnCount;
        }



        public void IncrementColumnCount()
        {
            this.numColumns++;
        }

        public void DecrementColumnCount()
        {
            this.numColumns--;
        }

        public override void Update()
        {
            //privMoveGrid

            // Go to first child
            PCSNode pNode = (PCSNode)this;
            pNode = pNode.pChild;

            // Set ColTotal to first child
            GameObject pGameObj = (GameObject)pNode;

            ColRect ColTotal = this.poColObj.poColRect;
            ColTotal.Set(pGameObj.GetColObject().poColRect);

            // loop through sliblings
            while (pNode != null)
            {
                pGameObj = (GameObject)pNode;
                ColTotal.Union(pGameObj.GetColObject().poColRect);

                // go to next sibling
                pNode = pNode.pSibling;
            }

            //this.pColObj.pColRect.Set(201, 201, 201, 201);
            this.x = this.poColObj.poColRect.x;
            this.y = this.poColObj.poColRect.y;

            //Debug.WriteLine("x:{0} y:{1} w:{2} h:{3}", ColTotal.x, ColTotal.y, ColTotal.width, ColTotal.height);

            base.baseUpdateBoundingBox();
            base.Update();
        }


        public void CountNumColumns()
        {
            //get the first column (far right column);
            GameObject firstChildColumn = (GameObject)this.pChild;
            //only count if child exists;
            if (firstChildColumn != null)
            {
                this.numColumns++;
            }

            GameObject pNext = (GameObject)firstChildColumn.pSibling;

            //null check for early out
            if (pNext != null)
            {
                //iterate through siblings of child column, increment numColumns until no more siblings of child;
                while (pNext != null)
                {
                    //increment
                    this.numColumns++;
                    //set next as sibling
                    pNext = (GameObject)pNext.pSibling;
                }
            }

            //pNext is null, no more columns
        }


        //--------------------------------------------------------------------------
        //collisions

        //public override void Accept(ColVisitor other)
        //{
        //    //call the collision reaction to visit this object;
        //    other.VisitGrid(this);
        //}


        //public override void VisitMissile(Missile m)
        //{
        //    //Missile vs AlienGrid
        //    ColPair.Collide(m, (GameObject)this.pChild);
        //}



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