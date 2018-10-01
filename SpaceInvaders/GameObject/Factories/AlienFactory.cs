using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public class AlienFactory
    {
        // Data: ---------------------
        private SpriteBatch pSpriteBatch;
        private PCSTree pTree;
        private PCSNode pParent;


        public AlienFactory(SpriteBatch.Name spriteBatchName, PCSTree pTree)
        {
            this.pSpriteBatch = SpriteBatchManager.Find(spriteBatchName);
            Debug.Assert(this.pSpriteBatch != null);

            Debug.Assert(pTree != null);
            this.pTree = pTree;

            //parent is null by default;
            this.pParent = null;
        }

        ~AlienFactory()
        {
            #if (TRACK_DESTRUCTOR)
            Debug.WriteLine("~AlienFactory():{0}", this.GetHashCode());
            #endif

            //reset the goods;
            this.pSpriteBatch = null;
            this.pParent = null;
        }

        public void SetParent(GameObject parentNode)
        {
            //fine being null (null object)
            this.pParent = parentNode;
        }




        public AlienType Create(AlienType.Type type, GameObject.Name gameObjectName, int index = 0, float posX = 0.0f, float posY = 0.0f)
        {
            AlienType pAlien = null;

            switch (type)
            {
                case AlienType.Type.Crab:
                    pAlien = new Crab(gameObjectName, GameSprite.Name.Crab, index, posX, posY);
                    break;

                case AlienType.Type.Squid:
                    pAlien = new Squid(gameObjectName, GameSprite.Name.Squid, index, posX, posY);
                    break;

                case AlienType.Type.Octopus:
                    pAlien = new Octopus(gameObjectName, GameSprite.Name.Octopus, index, posX, posY);
                    break;


                case AlienType.Type.AlienGrid:
                    //note that the grid HAS a sprite to determine color of the sprite box
                    //it will not render a sprite since the image is a null image object
                    pAlien = new Grid(gameObjectName, GameSprite.Name.AlienGrid, index, posX, posY);
                    //GameObjectManager.AttachTree(pAlien);
                    //todo refactor GridManager to handle at least two grids;
                    GridManager.CreateGridManager((Grid)pAlien);
                    break;

                case AlienType.Type.AlienGridColumn:
                    pAlien = new Column(gameObjectName, GameSprite.Name.AlienColumn, index, posX, posY);
                    break;

                default:
                    // something is wrong
                    Debug.Assert(false);
                    break;
            }


            //insert alien into the PCSTree Hierarchy;
            this.pTree.Insert(pAlien, this.pParent);

            //Activate the GameSprite by attaching the
            //new alien's proxy sprite to the alien sprite batch;
            //also attach a collision box associated with this sprite;
            pAlien.ActivateGameSprite(this.pSpriteBatch);
            pAlien.ActivateCollisionSprite(this.pSpriteBatch);

            return pAlien;
        }








        public static void BuildAlienGrid(AlienFactory pAlienFactory, Grid pAlienGrid, int columnCount)
        {

            //make 5 rows of 10 aliens with the factory and attach them to alienGrid;
            //this makes them row by row - 5 factories to make 5 rows of 10;
            for (int alienCount = 0; alienCount < columnCount; alienCount++)
            {

                //set the parent as the alienGrid;
                pAlienFactory.SetParent(pAlienGrid);

                //create the AlienGridColumns;
                AlienType alienGridColumn;
                alienGridColumn =
                    pAlienFactory.Create(AlienType.Type.AlienGridColumn, GameObject.Name.Column, alienCount);


                //set the (sub) parent as the alienGridColumn;
                pAlienFactory.SetParent(alienGridColumn);


                float gap = 45.0f;
                float proxyX = 50.0f;


                //float leftAlien_X_Position = 75.0f;
                //float columnwidth = 60.0f;
                //float spaceBetweenRows = 50.0f;
                float row_Y_Position = 700.0f;

                // 1 row x 10 squids;
                pAlienFactory.Create(AlienType.Type.Squid, GameObject.Name.Squid, alienCount, proxyX + gap * alienCount,
                    row_Y_Position);
                row_Y_Position -= gap;

                // 2 rows x 10 crabs;
                pAlienFactory.Create(AlienType.Type.Crab, GameObject.Name.Crab, alienCount, proxyX + gap * alienCount,
                    row_Y_Position);
                row_Y_Position -= gap;

                pAlienFactory.Create(AlienType.Type.Crab, GameObject.Name.Crab, alienCount, proxyX + gap * alienCount,
                    row_Y_Position);
                row_Y_Position -= gap;

                // 2 rows x 10 octopus;
                pAlienFactory.Create(AlienType.Type.Octopus, GameObject.Name.Octopus, alienCount,
                    proxyX + gap * alienCount, row_Y_Position);
                row_Y_Position -= gap;

                pAlienFactory.Create(AlienType.Type.Octopus, GameObject.Name.Octopus, alienCount,
                    proxyX + gap * alienCount, row_Y_Position);


            }

            //todo - decide how to decrement/remove columns when destroyed!!!!
            //set the number of columns in the grid;
            pAlienGrid.SetNumColumns(columnCount);
            
        }



    }
}