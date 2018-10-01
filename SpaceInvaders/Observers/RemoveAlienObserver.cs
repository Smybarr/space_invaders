using System;
using System.Diagnostics;
using Azul;


namespace SpaceInvaders
{
    public class RemoveAlienObserver : ColObserver
    {

        // Data: ---------------
       // private AlienType pAlienObj;
        private GameObject pAlienObj;

        public RemoveAlienObserver()
        {
            this.pAlienObj = null;
        }

        public RemoveAlienObserver(RemoveAlienObserver a)
        {
            Debug.Assert(a != null);
            this.pAlienObj = a.pAlienObj;
        }


        
        public override void Notify()
        {
            //Delete Alien
            //Debug.WriteLine("RemoveShieldBrickObserver: {0} {1}", this.subject.gameObject_A, this.subject.gameObject_B);

            //set this observer's alien object as the subject's pointer object that got hit
            //this.pAlienObj = (AlienType)this.pSubject.pObjB;
            this.pAlienObj = (GameObject) this.pSubject.pObjB;
            Debug.Assert(this.pAlienObj != null);

            //set the alien object as markedForDeath
            if (this.pAlienObj.markForDeath == false)
            {
                this.pAlienObj.markForDeath = true;

                ////hold the x, y coordinates of the target alien;
                //int index = pAlienObj.index;
                //float target_X = pAlienObj.pProxySprite.x;
                //float target_Y = pAlienObj.pProxySprite.y;

                ////create the alien explosion object to temporarily replace the normal alien object in the grid
                //ExplodingAlien pExplodeAlien = new ExplodingAlien(GameObject.Name.ExplodingAlien, GameSprite.Name.AlienExplosion, index, target_X, target_Y);

                ////get the right sprite batch and activate the explosion sprite
                //SpriteBatch pSB_GameSprites = SpriteBatchManager.Find(SpriteBatch.Name.GameSprites);
                //SpriteBatch pSB_Boxes = SpriteBatchManager.Find(SpriteBatch.Name.SpriteBoxes);

                ////activate the game and collision sprites
                //pExplodeAlien.ActivateGameSprite(pSB_GameSprites);
                //pExplodeAlien.ActivateCollisionSprite(pSB_Boxes);

                ////hold a pointer to old alien and its parent;
                //GameObject targetAlien = (GameObject)this.pAlienObj;
                //GameObject parentColumn = (GameObject)targetAlien.pParent;

                ////make sure the parent exists;
                //Debug.Assert(parentColumn != null);

                ////remove the alien
                //targetAlien.Remove();
                //Debug.WriteLine("removing alien {0} at x:{1}, y: {2}, sx: {3}, sy: {4}", targetAlien.pProxySprite.pSprite.GetName(),
                //    targetAlien.pProxySprite.x,
                //    targetAlien.pProxySprite.y,
                //    targetAlien.pProxySprite.sx,
                //    targetAlien.pProxySprite.sy
                //);

                ////hot swap the object pointers!
                ////set this remove alien's pAlienObj pointer to explosion alien;
                //this.pAlienObj = pExplodeAlien;

                ////insert the explosion alien as a child of column (where old alien used to be)
                //PCSTree rootGamObjTree = GameObjectManager.GetRootTree();
                //Debug.Assert(rootGamObjTree != null);

                ////update the coordinate data to render in old spot (GameObjectManager already called update)
                //this.pAlienObj.Update();

                //rootGamObjTree.Insert(this.pAlienObj, parentColumn);

                //Debug.WriteLine("added exploding alien {0} at x:{1}, y: {2}, sx: {3}, sy: {4}", pExplodeAlien.pProxySprite.pSprite.GetName(),
                //    pExplodeAlien.pProxySprite.x,
                //    pExplodeAlien.pProxySprite.y,
                //    pExplodeAlien.pProxySprite.sx,
                //    pExplodeAlien.pProxySprite.sy
                //);

                //Debug.WriteLine("added this pAlienObj {0} at x:{1}, y: {2}, sx: {3}, sy: {4}", this.pAlienObj.pProxySprite.pSprite.GetName(),
                //    this.pAlienObj.pProxySprite.x,
                //    this.pAlienObj.pProxySprite.y,
                //    this.pAlienObj.pProxySprite.sx,
                //    this.pAlienObj.pProxySprite.sy
                //);


                ////before removal, swap the sprite of JUST THIS ALIEN'S sprite image to the explosion sprite
                ////this forces ALL sprites to do the pop animation. dumb proxy sprites
                //this.pAlienObj.ChangeImage(Image.Name.AlienExplosionPop);

                //test - try and hot swap proxy sprite with this game sprite?
                //Azul.Rect pProxySpriteRect = this.pAlienObj.pProxySprite.pSprite.GetScreenRect();
                //Azul.Color white = new Azul.Color(1, 1, 1);
                //Image pImage = ImageManager.Find(Image.Name.AlienExplosionPop);

                float currentTime = Simulation.GetTimeStep();
                float totalTime = Simulation.GetTotalTime();


                //Delay
                //todo create an ObserverManager or refactor DelayObjectManager to pool observer objects - avoid using new at all cost!
                RemoveAlienObserver observer = new RemoveAlienObserver(this);
                DelayedObjectManager.Attach(observer);

            }
            else
            {
                Debug.Assert(false);
            }
        }


        //execute the Alien Removal - and potentially the removal of a column and the alien grid object
        public override void Execute()
        {
            //if the alien that was removed was the last one in the column, delete the column it was assigned to!
            //Debug.WriteLine("alien {0} parentColumn {1}", this.pAlien, this.pAlien.pParent);

            GameObject targetAlien = (GameObject)this.pAlienObj;
            GameObject parentColumn = (GameObject)targetAlien.pParent;

            //make sure the parent is a column;
            Debug.Assert(parentColumn.GetName() == GameObject.Name.Column);

            Debug.WriteLine("removing alien {0} at x:{1}, y: {2}, sx: {3}, sy: {4}", targetAlien.pProxySprite.pSprite.GetName(), 
                                                                                     targetAlien.pProxySprite.x,
                                                                                     targetAlien.pProxySprite.y, 
                                                                                     targetAlien.pProxySprite.sx, 
                                                                                     targetAlien.pProxySprite.sy
            );

            //hold the x, y coordinates of the target alien;
            //float target_X = targetAlien.pProxySprite.x;
            //float target_Y = targetAlien.pProxySprite.y;

            //remove the alien
            targetAlien.Remove();

            //gameobject and proxysprite gone;
            //after alien removal, place an explosion sprite in the last location of its proxy sprite;

            //GameSprite pExplodeSprite = GameSpriteManager.Find(GameSprite.Name.AlienExplosion);
            
            //pExplodeSprite.x = target_X;
            //pExplodeSprite.y = target_Y;
            //pExplodeSprite.sx = 1.0f;
            //pExplodeSprite.sy = 1.0f;

            

            //pExplodeSprite.Update();
            //pExplodeSprite.Draw();


            ////find the AlienGrid game object to increase the march speed;
            //GameObject pAlienGridObj = GameObjectManager.Find()
            //alienGrid.IncreaseAlienMarchSpeed();
            //Debug.WriteLine("Increased Alien Grid March Speed");
            //Debug.WriteLine("Current MarchSpeed: {0}", AlienGrid.marchSpeed);

            //Grid.liveAlienCount--;
            //Grid.deadAlienCount++;

            //Debug.WriteLine("Live AlienCount: {0}", AlienGrid.liveAlienCount);
            //Debug.WriteLine("Dead AlienCount: {0}", AlienGrid.deadAlienCount);


            //TODO: Need a better way to check if this is last alien in column/last column in grid;
            //check if last alien in the column
            if (privIsLastChildOf(parentColumn) == true)
            {
                //if so, remove the parent column;

                //get the grid pointer before removing the column (in case this is the last column)
                GameObject parentAlienGrid = (GameObject)parentColumn.pParent;
                parentColumn.Remove();

                //double check that parentAlienGrid is actually an alien grid
                Debug.Assert(parentAlienGrid.GetName() == GameObject.Name.Grid);

                //cast to a grid object and decrement the number of columns;
                Grid alienGrid = (Grid) parentAlienGrid;
                alienGrid.DecrementColumnCount();
  

                //check if the last column in the grid
                if (privIsLastChildOf(parentAlienGrid) == true)
                {
                    //todo - place a next level reload or reward for beating the level here!!!
                    //if so, remove the grid and trigger a reaction
                    parentAlienGrid.Remove();
                }
            }
        }

        //check if this is the last child
        private bool privIsLastChildOf(GameObject parentObject)
        {
            //if this is the last child;
            //here that would be last alien in a column or last column in the grid;
            if (parentObject.pChild == null)
            {
                return true;
            }
            return false;
        }

    }
}
