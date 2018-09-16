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




        public AlienType Create(AlienType.Type type, GameObject.Name gameObjectName, float posX = 0.0f, float posY = 0.0f)
        {
            AlienType pAlien = null;

            switch (type)
            {
                case AlienType.Type.Crab:
                    pAlien = new Crab(gameObjectName, GameSprite.Name.Crab, posX, posY);
                    break;

                case AlienType.Type.Squid:
                    pAlien = new Squid(gameObjectName, GameSprite.Name.Squid, posX, posY);
                    break;

                case AlienType.Type.Octopus:
                    pAlien = new Octopus(gameObjectName, GameSprite.Name.Octopus, posX, posY);
                    break;


                case AlienType.Type.AlienGrid:
                    //the grid doesn't have a single sprite - enter game sprite null object!
                    pAlien = new Grid(gameObjectName, GameSprite.Name.NullObject, posX, posY);
                    // --> Add alien grid to the gameObjectManager ONLY once - the root
                    //Debug.Assert(pAlien != null);
                    GameObjectManager.Attach(pAlien);
                    break;

                default:
                    // something is wrong
                    Debug.Assert(false);
                    break;
            }


            //insert alien into the PCSTree Hierarchy;
            this.pTree.Insert(pAlien, this.pParent);

            // Attached to Group
            this.pSpriteBatch.Attach(pAlien.pProxySprite);

            return pAlien;
        }


    }
}