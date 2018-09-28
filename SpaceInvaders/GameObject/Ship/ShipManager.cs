using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public class ShipManager
    {
        public enum State
        {
            Ready,
            MissileFlying,
            End
        }

        // Data: ----------------------------------------------
        private static ShipManager pInstance = null;

        // Active Objects
        private Ship pShip;
        private Missile pMissile;

        // Reference Objects (State Objects)
        private ShipStateReady pStateReady;
        private ShipStateMissileFlying pStateMissileFlying;
        private ShipStateEnd pStateEnd;


        private ShipManager()
        {
            // Store the states
            this.pStateReady = new ShipStateReady();
            this.pStateMissileFlying = new ShipStateMissileFlying();
            this.pStateEnd = new ShipStateEnd();

            // set active
            this.pShip = null;
            this.pMissile = null;
        }

        public static void CreateShipTree()
        {
            // make sure its the first time
            Debug.Assert(pInstance == null);

            // Do the initialization
            if (pInstance == null)
            {
                pInstance = new ShipManager();
            }

            Debug.Assert(pInstance != null);

            // Stuff to initialize after the instance was created
            pInstance.pShip = CreateShips();
            pInstance.pShip.SetState(ShipManager.State.Ready);

        }

        private static ShipManager privInstance()
        {
            Debug.Assert(pInstance != null);

            return pInstance;
        }

        public static Ship GetShip()
        {
            ShipManager pShipMan = ShipManager.privInstance();

            Debug.Assert(pShipMan != null);
            Debug.Assert(pShipMan.pShip != null);

            return pShipMan.pShip;
        }

        public static ShipState GetState(State state)
        {
            ShipManager pShipMan = ShipManager.privInstance();
            Debug.Assert(pShipMan != null);

            ShipState pShipState = null;

            switch (state)
            {
                case ShipManager.State.Ready:
                    pShipState = pShipMan.pStateReady;
                    break;

                case ShipManager.State.MissileFlying:
                    pShipState = pShipMan.pStateMissileFlying;
                    break;

                case ShipManager.State.End:
                    pShipState = pShipMan.pStateEnd;
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }

            return pShipState;
        }

        public static Missile GetMissile()
        {
            ShipManager pShipMan = ShipManager.privInstance();

            Debug.Assert(pShipMan != null);
            Debug.Assert(pShipMan.pMissile != null);

            return pShipMan.pMissile;
        }

        public static Missile ActivateMissile()
        {
            ShipManager pShipMan = ShipManager.privInstance();
            Debug.Assert(pShipMan != null);

            // get the game object tree
            PCSTree pTree = GameObjectManager.GetRootTree();
            Debug.Assert(pTree != null);


            //create a new missile!!

            // copy over safe copy (create a new one or find in game object tree)
            Missile pMissile = new Missile(GameObject.Name.Missile, GameSprite.Name.Missile, 0, 400, 100);
            pShipMan.pMissile = pMissile;


            // Attach missile to SpriteBatches
            SpriteBatch pSB_Aliens = SpriteBatchManager.Find(SpriteBatch.Name.GameSprites);
            SpriteBatch pSB_Boxes = SpriteBatchManager.Find(SpriteBatch.Name.SpriteBoxes);

            pMissile.ActivateCollisionSprite(pSB_Boxes);
            pMissile.ActivateGameSprite(pSB_Aliens);


            // Attach the missile to the missile root
            GameObject pMissileRoot = GameObjectManager.Find(GameObject.Name.MissileRoot);
            Debug.Assert(pMissileRoot != null);

            // Add to GameObject Tree - {update and collisions}
            pTree.Insert(pShipMan.pMissile, pMissileRoot);

            return pShipMan.pMissile;
        }


        private static Ship CreateShips()
        {
            ShipManager pShipMan = ShipManager.privInstance();
            Debug.Assert(pShipMan != null);

            // get the game object tree
            PCSTree rootGamObjTree = GameObjectManager.GetRootTree();
            Debug.Assert(rootGamObjTree != null);

            //create the root - coordinates are 0 since it's a root
            //ShipRoot pShipRoot = new ShipRoot(GameObject.Name.ShipRoot, GameSprite.Name.NullObject, 0, 0.0f, 0.0f);
            //DeathManager.Attach(shipRoot);

            //todo find the ship object from loading instead of creating a new one
            // copy over safe copy (new or find precreated game object)

            float shipStartPos_X = 75.0f;
            float shipStartPos_Y = 75.0f;

            Ship pShip = new Ship(GameObject.Name.Ship, GameSprite.Name.Ship, 0, shipStartPos_X, shipStartPos_Y);
            pShipMan.pShip = pShip;

            // Attach the sprite to the correct sprite batch
            SpriteBatch pSB_Aliens = SpriteBatchManager.Find(SpriteBatch.Name.GameSprites);
            pSB_Aliens.Attach(pShip.pProxySprite);


            // Attach the ship to the ship root
            GameObject pShipRoot = GameObjectManager.Find(GameObject.Name.ShipRoot);
            Debug.Assert(pShipRoot != null);

            // Add current ship to GameObject Tree - {update and collisions}
            rootGamObjTree.Insert(pShipMan.pShip, pShipRoot);

            //attach the shipRoot tree to the root game object tree
            //GameObjectManager.AttachTree(pShipRoot, rootGamObjTree);
            GameObjectManager.AttachTree(pShipRoot);

            return pShipMan.pShip;
        }



    }
}