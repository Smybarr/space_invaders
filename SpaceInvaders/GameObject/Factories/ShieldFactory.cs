using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public class ShieldFactory
    {

        // Data: ---------------------
        private SpriteBatch pSpriteBatch;
        private SpriteBatch pCollisionSpriteBatch;
        private GameObject pParent;
        private PCSTree pTree;

        public ShieldFactory(SpriteBatch.Name spriteBatchName, SpriteBatch.Name collisionSpriteBatch, PCSTree pTree)
        {
            this.pSpriteBatch = SpriteBatchManager.Find(spriteBatchName);
            Debug.Assert(this.pSpriteBatch != null);

            this.pCollisionSpriteBatch = SpriteBatchManager.Find(collisionSpriteBatch);
            Debug.Assert(this.pCollisionSpriteBatch != null);

            Debug.Assert(pTree != null);
            this.pTree = pTree;
        }

        public void setParent(GameObject parentNode)
        {
            // OK being null
            this.pParent = parentNode;
        }

        ~ShieldFactory()
        {
            #if(TRACK_DESTRUCTOR)
            Debug.WriteLine("~ShieldFactory():{0}", this.GetHashCode());
            #endif
            this.pSpriteBatch = null;
            this.pParent = null;

        }

        public ShieldCategory Create(ShieldCategory.Type type, GameObject.Name gameObjName, int index = 0, float posX = 0.0f, float posY = 0.0f)
        {
            ShieldCategory pShield = null;

            switch (type)
            {
                case ShieldCategory.Type.Root:
                    pShield = new ShieldRoot(gameObjName, GameSprite.Name.NullObject, index, posX, posY);
                    //pShield.SetCollisionColor(0.0f, 0.0f, 1.0f);
                    //GameObjectManager.AttachTree(pShield, this.pTree);
                    //pShield.ActivateCollisionSprite(this.pCollisionSpriteBatch);
                    break;

                case ShieldCategory.Type.ShieldGrid:
                    pShield = new ShieldGrid(gameObjName, GameSprite.Name.NullObject, index, posX, posY);
                    //pShield.SetCollisionColor(1.0f, 0.0f, 0.0f);
                    pShield.ActivateCollisionSprite(this.pCollisionSpriteBatch);
                    break;

                case ShieldCategory.Type.ShieldColumn:
                    pShield = new ShieldColumn(gameObjName, GameSprite.Name.NullObject, index, posX, posY);
                    //pShield.SetCollisionColor(1.0f, 0.0f, 0.0f);
                    pShield.ActivateCollisionSprite(this.pCollisionSpriteBatch);
                    break;


                case ShieldCategory.Type.ShieldBrick:
                    pShield = new ShieldBrick(gameObjName, GameSprite.Name.ShieldBrick, index, posX, posY);
                    break;

                case ShieldCategory.Type.ShieldBrickLeft_Top:
                    pShield = new ShieldBrick(gameObjName, GameSprite.Name.ShieldBrickLeft_Top, index, posX, posY);
                    break;


                case ShieldCategory.Type.ShieldBrickRight_Top:
                    pShield = new ShieldBrick(gameObjName, GameSprite.Name.ShieldBrickRight_Top, index, posX, posY);
                    break;


                case ShieldCategory.Type.ShieldBrickMidLeft_Bottom:
                    pShield = new ShieldBrick(gameObjName, GameSprite.Name.ShieldBrickMidLeft_Bottom, index, posX, posY);
                    break;

                case ShieldCategory.Type.ShieldBrickMid_Bottom:
                    pShield = new ShieldBrick(gameObjName, GameSprite.Name.ShieldBrickMid_Bottom, index, posX, posY);
                    break;

                case ShieldCategory.Type.ShieldBrickMidRight_Bottom:
                    pShield = new ShieldBrick(gameObjName, GameSprite.Name.ShieldBrickMidRight_Bottom, index, posX, posY);
                    break;

                default:
                    // something is wrong
                    Debug.Assert(false);
                    break;
            }

            // add to the tree
            this.pTree.Insert(pShield, this.pParent);

            // Attached to Group
            pShield.ActivateGameSprite(this.pSpriteBatch);
            pShield.ActivateCollisionSprite(this.pCollisionSpriteBatch);

            return pShield;
        }



        public void BuildShield(ShieldCategory pShieldRoot, float startPos_x, float startPos_y, float brickSpriteWidth, float brickSpriteHeight)
        {
            // set the parent for hierarchy inside the factory, grid is root, so parent is null
            this.setParent(pShieldRoot);

            // create and attach grid to Root  
            ShieldCategory pShieldGrid = this.Create(ShieldCategory.Type.ShieldGrid, GameObject.Name.ShieldGrid);

            // load the shield by column, bottom to top, left to right;
            int j = 0;

            ShieldCategory pColumn;

            this.setParent(pShieldGrid);

            //Each Shield has 7 Columns and 5 Rows
            //-----------------------------------------------------------------------------------

            pColumn = this.Create(ShieldCategory.Type.ShieldColumn, GameObject.Name.ShieldColumn, j++);
            this.setParent(pColumn);

            int i = 0;

            //float start_x = 75.0f;
            //float start_y = 100.0f;
            float gap = 0;

            //-----------------------------------------------------------------------------------
            //Column 1: row 1 = topLeft, row 2-5 = normal bricks

            this.Create(ShieldCategory.Type.ShieldBrick, GameObject.Name.ShieldBrick, i++, startPos_x, startPos_y);
            this.Create(ShieldCategory.Type.ShieldBrick, GameObject.Name.ShieldBrick, i++, startPos_x, startPos_y + brickSpriteHeight);
            this.Create(ShieldCategory.Type.ShieldBrick, GameObject.Name.ShieldBrick, i++, startPos_x, startPos_y + 2 * brickSpriteHeight);
            this.Create(ShieldCategory.Type.ShieldBrick, GameObject.Name.ShieldBrick, i++, startPos_x, startPos_y + 3 * brickSpriteHeight);
            this.Create(ShieldCategory.Type.ShieldBrickLeft_Top, GameObject.Name.ShieldBrick, i++, startPos_x, startPos_y + 4 * brickSpriteHeight);


            this.setParent(pShieldGrid);

            //-----------------------------------------------------------------------------------
            //Column 2: row 1-5 = normal bricks

            pColumn = this.Create(ShieldCategory.Type.ShieldColumn, GameObject.Name.ShieldColumn, j++);
            this.setParent(pColumn);

            gap += brickSpriteWidth;
            this.Create(ShieldCategory.Type.ShieldBrick, GameObject.Name.ShieldBrick, i++, startPos_x + gap, startPos_y);
            this.Create(ShieldCategory.Type.ShieldBrick, GameObject.Name.ShieldBrick, i++, startPos_x + gap, startPos_y + brickSpriteHeight);
            this.Create(ShieldCategory.Type.ShieldBrick, GameObject.Name.ShieldBrick, i++, startPos_x + gap, startPos_y + 2 * brickSpriteHeight);
            this.Create(ShieldCategory.Type.ShieldBrick, GameObject.Name.ShieldBrick, i++, startPos_x + gap, startPos_y + 3 * brickSpriteHeight);
            this.Create(ShieldCategory.Type.ShieldBrick, GameObject.Name.ShieldBrick, i++, startPos_x + gap, startPos_y + 4 * brickSpriteHeight);


            this.setParent(pShieldGrid);

            //-----------------------------------------------------------------------------------
            //Column 3: 1-3 normal, 4 = bottomleft, 5 = no brick!

            pColumn = this.Create(ShieldCategory.Type.ShieldColumn, GameObject.Name.ShieldColumn, j++);
            this.setParent(pColumn);

            gap += brickSpriteWidth;
            this.Create(ShieldCategory.Type.ShieldBrickMidLeft_Bottom, GameObject.Name.ShieldBrick, i++, startPos_x + gap, startPos_y + 1 * brickSpriteHeight);
            this.Create(ShieldCategory.Type.ShieldBrick, GameObject.Name.ShieldBrick, i++, startPos_x + gap, startPos_y + 2 * brickSpriteHeight);
            this.Create(ShieldCategory.Type.ShieldBrick, GameObject.Name.ShieldBrick, i++, startPos_x + gap, startPos_y + 3 * brickSpriteHeight);
            this.Create(ShieldCategory.Type.ShieldBrick, GameObject.Name.ShieldBrick, i++, startPos_x + gap, startPos_y + 4 * brickSpriteHeight);


            this.setParent(pShieldGrid);


            //-----------------------------------------------------------------------------------
            //Column 4: 1-3 normal, 4 = bottom mid, 5 = no brick!


            pColumn = this.Create(ShieldCategory.Type.ShieldColumn, GameObject.Name.ShieldColumn, j++);
            this.setParent(pColumn);

            gap += brickSpriteWidth;
            this.Create(ShieldCategory.Type.ShieldBrickMid_Bottom, GameObject.Name.ShieldBrick, i++, startPos_x + gap, startPos_y + 1 * brickSpriteHeight);
            this.Create(ShieldCategory.Type.ShieldBrick, GameObject.Name.ShieldBrick, i++, startPos_x + gap, startPos_y + 2 * brickSpriteHeight);
            this.Create(ShieldCategory.Type.ShieldBrick, GameObject.Name.ShieldBrick, i++, startPos_x + gap, startPos_y + 3 * brickSpriteHeight);
            this.Create(ShieldCategory.Type.ShieldBrick, GameObject.Name.ShieldBrick, i++, startPos_x + gap, startPos_y + 4 * brickSpriteHeight);

            this.setParent(pShieldGrid);


            //-----------------------------------------------------------------------------------
            //Column 5: 1-3 normal, 4 = bottom right, 5 = no brick!


            pColumn = this.Create(ShieldCategory.Type.ShieldColumn, GameObject.Name.ShieldColumn, j++);
            this.setParent(pColumn);

            gap += brickSpriteWidth;
            this.Create(ShieldCategory.Type.ShieldBrickMidRight_Bottom, GameObject.Name.ShieldBrick, i++, startPos_x + gap, startPos_y + 1 * brickSpriteHeight);
            this.Create(ShieldCategory.Type.ShieldBrick, GameObject.Name.ShieldBrick, i++, startPos_x + gap, startPos_y + 2 * brickSpriteHeight);
            this.Create(ShieldCategory.Type.ShieldBrick, GameObject.Name.ShieldBrick, i++, startPos_x + gap, startPos_y + 3 * brickSpriteHeight);
            this.Create(ShieldCategory.Type.ShieldBrick, GameObject.Name.ShieldBrick, i++, startPos_x + gap, startPos_y + 4 * brickSpriteHeight);

            this.setParent(pShieldGrid);


            //-----------------------------------------------------------------------------------
            //Column 6: 1-5 normal 

            pColumn = this.Create(ShieldCategory.Type.ShieldColumn, GameObject.Name.ShieldColumn, j++);
            this.setParent(pColumn);

            gap += brickSpriteWidth;
            this.Create(ShieldCategory.Type.ShieldBrick, GameObject.Name.ShieldBrick, i++, startPos_x + gap, startPos_y);
            this.Create(ShieldCategory.Type.ShieldBrick, GameObject.Name.ShieldBrick, i++, startPos_x + gap, startPos_y + brickSpriteHeight);
            this.Create(ShieldCategory.Type.ShieldBrick, GameObject.Name.ShieldBrick, i++, startPos_x + gap, startPos_y + 2 * brickSpriteHeight);
            this.Create(ShieldCategory.Type.ShieldBrick, GameObject.Name.ShieldBrick, i++, startPos_x + gap, startPos_y + 3 * brickSpriteHeight);
            this.Create(ShieldCategory.Type.ShieldBrick, GameObject.Name.ShieldBrick, i++, startPos_x + gap, startPos_y + 4 * brickSpriteHeight);

            this.setParent(pShieldGrid);


            //-----------------------------------------------------------------------------------
            //Column 7: 1 top right, 2-5 normal


            pColumn = this.Create(ShieldCategory.Type.ShieldColumn, GameObject.Name.ShieldColumn, j++);
            this.setParent(pColumn);

            gap += brickSpriteWidth;

            this.Create(ShieldCategory.Type.ShieldBrick, GameObject.Name.ShieldBrick, i++, startPos_x + gap, startPos_y);
            this.Create(ShieldCategory.Type.ShieldBrick, GameObject.Name.ShieldBrick, i++, startPos_x + gap, startPos_y + brickSpriteHeight);
            this.Create(ShieldCategory.Type.ShieldBrick, GameObject.Name.ShieldBrick, i++, startPos_x + gap, startPos_y + 2 * brickSpriteHeight);
            this.Create(ShieldCategory.Type.ShieldBrick, GameObject.Name.ShieldBrick, i++, startPos_x + gap, startPos_y + 3 * brickSpriteHeight);
            this.Create(ShieldCategory.Type.ShieldBrickRight_Top, GameObject.Name.ShieldBrick, i++, startPos_x + gap, startPos_y + 4 * brickSpriteHeight);


        }



    }
}