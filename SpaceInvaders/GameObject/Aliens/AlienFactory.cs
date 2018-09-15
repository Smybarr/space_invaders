using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public class AlienFactory
    {
        public AlienFactory(SpriteBatch.Name spriteBatchName)
        {
            this.pSpriteBatch = SpriteBatchManager.Find(spriteBatchName);
            Debug.Assert(this.pSpriteBatch != null);
        }

        ~AlienFactory()
        {
            #if (TRACK_DESTRUCTOR)
            Debug.WriteLine("~AlienFactory():{0}", this.GetHashCode());
            #endif
            this.pSpriteBatch = null;
        }

        public void Create(AlienType.Type type, float posX, float posY)
        {
            AlienType pAlien = null;

            switch (type)
            {
                case AlienType.Type.Crab:
                    pAlien = new Crab(GameObject.Name.Crab, GameSprite.Name.Crab, posX, posY);
                    break;

                case AlienType.Type.Squid:
                    pAlien = new Squid(GameObject.Name.Squid, GameSprite.Name.Squid, posX, posY);
                    break;

                case AlienType.Type.Octopus:
                    pAlien = new Octopus(GameObject.Name.Octopus, GameSprite.Name.Octopus, posX, posY);
                    break;

                default:
                    // something is wrong
                    Debug.Assert(false);
                    break;
            }

            // add it to the gameObjectManager
            Debug.Assert(pAlien != null);
            GameObjectManager.Attach(pAlien);

            // Attached to Group
            this.pSpriteBatch.Attach(pAlien.pProxySprite);
        }

        // Data: ---------------------

        SpriteBatch pSpriteBatch;
    }
}