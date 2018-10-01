using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

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
        private Ship pCurrentShip;
        private Missile pMissile;

        // Reference Objects (State Objects)
        private ShipStateReady pStateReady;
        private ShipStateMissileFlying pStateMissileFlying;
        private ShipStateEnd pStateEnd;

        private int numShipChances = 0;

        private float startShip_PosX = 75.0f;
        private float startShip_PosY = 60.0f;

        private float extraShipOne_X = 400.0f;
        private float extraShipTwo_X = 470.0f;
        private float extraShip_Y = 20.0f;


        private ShipManager()
        {
            // Store the states
            this.pStateReady = new ShipStateReady();
            this.pStateMissileFlying = new ShipStateMissileFlying();
            this.pStateEnd = new ShipStateEnd();

            // set active
            this.pCurrentShip = null;
            this.pMissile = null;        
        }

        public static void CreateShipManager()
        {
            // make sure its the first time
            Debug.Assert(pInstance == null);

            // Do the initialization
            if (pInstance == null)
            {
                pInstance = new ShipManager();

                Debug.Assert(pInstance != null);

                // Stuff to initialize after the instance was created
                
                //Create the Ship Tree
                pInstance.pCurrentShip = CreateShips();
                pInstance.pCurrentShip.SetState(ShipManager.State.Ready);
            }
        }

        private static ShipManager privInstance()
        {
            Debug.Assert(pInstance != null);

            return pInstance;
        }

        public static Ship GetCurrentShip()
        {
            ShipManager pShipMan = ShipManager.privInstance();

            Debug.Assert(pShipMan != null);
            Debug.Assert(pShipMan.pCurrentShip != null);

            return pShipMan.pCurrentShip;
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

            //todo - refactor this using object pooling for ships - also will need more than these 3 ships w/ two player!
            //Create 3 ships total-----------------

            //create the starting ship;
            Ship pShip = new Ship(GameObject.Name.Ship, GameSprite.Name.Ship, 0, pShipMan.startShip_PosX, pShipMan.startShip_PosY);
            //set as the ship managers current ship
            pShipMan.pCurrentShip = pShip;

            // Attach the sprite to the correct sprite batch
            SpriteBatch pSB_GameSprites = SpriteBatchManager.Find(SpriteBatch.Name.GameSprites);
            SpriteBatch pSB_Boxes = SpriteBatchManager.Find(SpriteBatch.Name.SpriteBoxes);
            pSB_GameSprites.Attach(pShip.pProxySprite);
            pShip.ActivateCollisionSprite(pSB_Boxes);


            // Attach the ship to the ship root
            // get the root (created before this function was called)
            GameObject pShipRoot = GameObjectManager.Find(GameObject.Name.ShipRoot);
            Debug.Assert(pShipRoot != null);

            // Add current ship tree to GameObject Tree - {update and collisions}
            rootGamObjTree.Insert(pShipMan.pCurrentShip, pShipRoot);

            //TESTING - CREATING 2 EXTRA SHIPS-------------------------------------------

            //extra ship one
            Ship pShipOne = new Ship(GameObject.Name.Ship, GameSprite.Name.Ship, 1, pShipMan.extraShipOne_X, pShipMan.extraShip_Y);
                       
            //add to the ship tree as well as root game object tree
            rootGamObjTree.Insert(pShipOne, pShipMan.pCurrentShip);

            // Attach the sprite to the correct sprite batch

            pSB_GameSprites.Attach(pShipOne.pProxySprite);
            pShipOne.ActivateGameSprite(pSB_GameSprites);
            pShipOne.ActivateCollisionSprite(pSB_Boxes);

            //update the position of the new ship 
            pShipOne.Update();

            //extra ship two
            Ship pShipTwo = new Ship(GameObject.Name.Ship, GameSprite.Name.Ship, 2, pShipMan.extraShipTwo_X, pShipMan.extraShip_Y);


            //add to the ship tree as well as root game object tree
            rootGamObjTree.Insert(pShipTwo, pShipOne);

            // Attach the sprite to the correct sprite batch
            pSB_GameSprites.Attach(pShipTwo.pProxySprite.pSprite);
            pShipTwo.ActivateGameSprite(pSB_GameSprites);
            pShipTwo.ActivateCollisionSprite(pSB_Boxes);

            //update the position of the new ship 
            pShipTwo.Update();

            //END TESTING OF CREATING 2 EXTRA SHIPS-------------------------------------------


            //attach the shipRoot tree to the root game object tree
            //GameObjectManager.AttachTree(pShipRoot, rootGamObjTree);
            GameObjectManager.AttachTree(pShipRoot);

            return pShipMan.pCurrentShip;
        }

        private bool privHasAnotherShip()
        {
            //check if there's another ship to pull;

            //if the current ship has a sibling, this returns true...
            //if no sibling then returns false;
            bool result = false;

            GameObject pCurrShip = ShipManager.GetCurrentShip();


            if (pCurrShip.pChild != null)
            {
                result = true;
            }
            else
            {
                //todo no more ships - tigger game over state!
                Debug.WriteLine("NO MORE SHIPS - GAME OVER!!!!!!!\n\n\n\n");
                Debug.Assert(false);
            }

            return result;
        }

        public static void GetNextShip()
        {
            //get the instance of the ship manager;
            ShipManager pShipMan = ShipManager.privInstance();
            Debug.Assert(pShipMan != null);

            Ship nextShip = null;

            //if there's another ship, get and prepare it
            if (pShipMan.privHasAnotherShip())
            {
                nextShip = (Ship) pShipMan.pCurrentShip.pChild;
                Debug.Assert(nextShip != null);
                
                //set the coordinates to starting position
                nextShip.x = pShipMan.startShip_PosX;
                nextShip.y = pShipMan.startShip_PosY;
                nextShip.SetState(State.Ready);
                //update the positioning;
                nextShip.pProxySprite.Update();
                nextShip.Update();

                //remove rendering of current ship (off screen);
                //todo FIX THIS - Cheap Hack to just render the old ships WAYY off screen!
                pShipMan.pCurrentShip.x = -1000.0f;
                pShipMan.pCurrentShip.y = -1000.0f;
                pShipMan.pCurrentShip.SetState(State.End);
                pShipMan.pCurrentShip.pProxySprite.Update();

                ////define an override remove inside of Ship class
                //pShipMan.pCurrentShip.Remove();

                pShipMan.pCurrentShip = nextShip;
                pShipMan.pCurrentShip.Update();

                Debug.Assert(pShipMan.pCurrentShip != null);

                //remove the current ship;
            }

            //return nextShip;
        }

        public void PrepareNewShip(Ship pShip)
        {
            Debug.Assert(pShip != null);

            pShip.SetState(State.Ready);
        }

    }
}