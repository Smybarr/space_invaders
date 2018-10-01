using Azul;

namespace SpaceInvaders.Test
{
    using System;
    using System.Diagnostics;

    namespace SpaceInvaders
    {

        public class TestGameTwo : Azul.Game
        {
            //-----------------------------------------------------------------------------
            // Game::Initialize()
            //		Allows the engine to perform any initialization it needs to before 
            //      starting to run.  This is where it can query for any required services 
            //      and load any non-graphic related content. 
            //-----------------------------------------------------------------------------
            public override void Initialize()
            {
                Debug.WriteLine("----------------------------------");
                Debug.WriteLine("Initialize Game");
                Debug.WriteLine("----------------------------------");

                // Game Window Device setup
                this.SetWindowName("Space Invaders SMYBARR 2018");

                int windowWidth = 224 * 3; //672
                int windowHeight = 256 * 3; //768

                //set the screen size;       
                this.SetWidthHeight(windowWidth, windowHeight);
                //set the screen background color (r, g, b, alpha);
                this.SetClearColor(0.1f, 0.1f, 0.1f, 1.0f);

                Debug.WriteLine("\n------Window Loaded-------");
                Debug.WriteLine("Width:\t{0}\nHeight:\t{1}\n\n", this.GetScreenWidth(), this.GetScreenHeight());


                Debug.WriteLine("\n\nInitialization Complete\n----------------------------------\n\n");
            }

            //-----------------------------------------------------------------------------
            // Game::LoadContent()
            //		Allows you to load all content needed for your engine,
            //	    such as objects, graphics, etc.
            //-----------------------------------------------------------------------------

            public override void LoadContent()
            {
                Debug.WriteLine("----------------------------------");
                Debug.WriteLine("Load Content");
                Debug.WriteLine("----------------------------------\n");


                //-----------------------------------------------
                //Create Game Simulation instance
                Simulation.Create();

                //-----------------------------------------------
                //Manager Load
                //------------------------------
                //Texture Manager Create
                TextureManager.Create();
                //------------------------------
                //Image Manager Create
                ImageManager.Create();
                //------------------------------
                //Game/Box Sprite Manager Create
                GameSpriteManager.Create(5, 2);
                BoxSpriteManager.Create();
                //------------------------------
                //SpriteBatch Manager Create
                SpriteBatchManager.Create();
                //------------------------------
                //ProxySprite Manager Create
                ProxySpriteManager.Create(10, 1);
                //------------------------------
                //GameObject Manager Create
                GameObjectManager.Create();
                //------------------------------
                //CollisionPair Manager Create
                ColPairManager.Create();
                //------------------------------
                //TimerEvent Manager Create
                TimerEventManager.Create();
                //------------------------------
                //Glyph/Font Manager Create/Load
                GlyphManager.Create();
                FontManager.Create();

                //------------------------------
                //Death/Ghost Manager Create
                DeathManager.Create(1, 1);
                GhostManager.Create(1, 1);
                //-----------------------------------------------
                // Input Manager - Key Observer Load
                InputManager.LoadKeyInputObservers();

                //-----------------------------------------------
                //Print some initial stats
                TextureManager.DumpStats();
                ImageManager.DumpStats();
                GameSpriteManager.DumpStats();
                BoxSpriteManager.DumpStats();
                SpriteBatchManager.DumpStats();
                ProxySpriteManager.DumpStats();
                TimerEventManager.DumpStats();
                GameObjectManager.DumpStats();
                DeathManager.DumpStats();
                GhostManager.DumpStats();
                ColPairManager.DumpStats();
                GlyphManager.DumpStats();

                //------------------------------
                //Asset Loading

                //Texture/Image/Font Load
                TextureManager.LoadTextures();
                ImageManager.LoadImages();
                FontManager.LoadFonts();

                //-----------------------------------------------
                //Sprite Batch /Sprite Load
                SpriteBatch pSB_GameSprites = SpriteBatchManager.Add(SpriteBatch.Name.GameSprites);
                SpriteBatch pSB_Boxes = SpriteBatchManager.Add(SpriteBatch.Name.SpriteBoxes);
                SpriteBatch pSB_Texts = SpriteBatchManager.Add(SpriteBatch.Name.TextLetters);


                //-----------------------------------------------
                //Sprites/BoxSprites Load
                //NOTE that the following coordinates will not matter
                //once proxy sprites are used!

                //35w x 35h for all sprites;
                float const_AlienSpriteSize = 30.0f;


                //render dimensions/coordinates
                float squid_sX = 400.0f;
                float squid_sY = 400.0f;

                float crab_sX = 400.0f;
                float crab_sY = 350.0f;

                float octo_sX = 400.0f;
                float octo_sY = 300.0f;

                float missileSpriteWidth = 4.0f;
                float missileSpriteHeight = 8.0f;

                float alienBombSpriteWidth = 6.0f;
                float alienBombSpriteHeight = 12.0f;

                float shipSpriteWidth = 55.0f;
                float shipSpriteHeight = 25.0f;

                float verticalWallWidth = 678.0f;
                float verticalWallHeight = 10.0f;

                float horizontalWallWidth = 678.0f;
                float horizontalWallHeight = 10.0f;

                //----------------------
                //aliens (initial sprites only)

                //note that the alien grid and alien column are game sprites for box sprite rendering,
                //but will pass a null image since they don't have anything to render
                GameSpriteManager.Add(GameSprite.Name.AlienGrid, Image.Name.NullObject, 0, 0, 0, 0);
                GameSpriteManager.Add(GameSprite.Name.AlienColumn, Image.Name.NullObject, 0, 0, 0, 0);

                //squid game sprite
                GameSpriteManager.Add(GameSprite.Name.Squid, Image.Name.SquidOpen, squid_sX, squid_sY, const_AlienSpriteSize, const_AlienSpriteSize);
                //crab game sprite
                GameSpriteManager.Add(GameSprite.Name.Crab, Image.Name.CrabOpen, crab_sX, crab_sY, const_AlienSpriteSize, const_AlienSpriteSize);
                //octopus game sprite
                GameSpriteManager.Add(GameSprite.Name.Octopus, Image.Name.OctopusOpen, octo_sX, octo_sY, const_AlienSpriteSize, const_AlienSpriteSize);

                //----------------------
                //alien bombs (initial sprites only)

                //zigzag bomb 
                GameSpriteManager.Add(GameSprite.Name.ZigZagAlienBomb, Image.Name.AlienBombZigZag_One, 0, 0, alienBombSpriteWidth, alienBombSpriteHeight);
                //cross bomb 
                GameSpriteManager.Add(GameSprite.Name.CrossAlienBomb, Image.Name.AlienBombCross_One, 0, 0, alienBombSpriteWidth, alienBombSpriteHeight);
                //rolling bomb 
                GameSpriteManager.Add(GameSprite.Name.RollingAlienBomb, Image.Name.AlienBombRolling_Two, 0, 0, alienBombSpriteWidth, alienBombSpriteHeight);

                //----------------------
                //hero ship
                GameSpriteManager.Add(GameSprite.Name.Ship, Image.Name.Ship, 640.0f, 440.0f, shipSpriteWidth, shipSpriteHeight);
                //hero missile
                GameSpriteManager.Add(GameSprite.Name.Missile, Image.Name.Missile, 0, 0, missileSpriteWidth, missileSpriteHeight);

                //----------------------
                //walls
                //note screen w = 672px wide by 768px
                GameSpriteManager.Add(GameSprite.Name.VerticalWall, Image.Name.NullObject, 0, 0, verticalWallWidth, verticalWallHeight);
                GameSpriteManager.Add(GameSprite.Name.HorizontalWall, Image.Name.NullObject, 0, 0, horizontalWallWidth, horizontalWallHeight);


                //----------------------
                //BoxSprites are added in the CollisionObject constructor depending on the sprite!







                //-----------------------------------------------
                //Load the Animations (TimerEvents)

                TimerEventManager.LoadAlienAnimations();
                TimerEventManager.LoadBombAnimations();











                //-----------------------------------------------
                //Set the Game Simulation State;


                Simulation.SetState(Simulation.State.Realtime);






                //-----------------------------------------------
                //GameObject Load (Factory)

                //get the PCSRootTree that was created by GameObjectManager
                PCSTree rootGamObjTree = GameObjectManager.GetRootTree();
                //make sure root tree and root have been created;
                Debug.Assert(rootGamObjTree != null);
                Debug.Assert(rootGamObjTree.GetRoot() != null);

                //check the tree
                rootGamObjTree.DumpTree();
                /*     */

                //------------------------------------------------
                // Create Missile Tree

                MissileRoot pMissileRoot = new MissileRoot(GameObject.Name.MissileRoot, GameSprite.Name.NullObject, 0, 0.0f, 0.0f);

                rootGamObjTree.Insert(pMissileRoot, null);
                pMissileRoot.ActivateCollisionSprite(pSB_Boxes);

                //GameObjectManager.AttachTree(pMissileRoot, rootGamObjTree);
                GameObjectManager.AttachTree(pMissileRoot);

                //TEST------------------
                //PCSTree missileTree = new PCSTree();

                //MissileRoot pMissileRoot = new MissileRoot(GameObject.Name.MissileRoot, GameSprite.Name.NullObject, 0, 0.0f, 0.0f);

                //missileTree.Insert(pMissileRoot, null);
                //pMissileRoot.ActivateCollisionSprite(pSB_Boxes);

                //GameObjectManager.AttachTree(pMissileRoot, rootGamObjTree);
                //rootGamObjTree.Insert(pMissileRoot, rootGamObjTree.GetRoot());

                //------------------------------------------------
                // Create Ship Tree

                ShipRoot shipRoot = new ShipRoot(GameObject.Name.ShipRoot, GameSprite.Name.NullObject, 0, 0.0f, 0.0f);
                DeathManager.Attach(shipRoot);
                shipRoot.ActivateCollisionSprite(pSB_Boxes);

                //check the tree
                rootGamObjTree.DumpTree();

                //attach the shipRoot to the rootGameObjTree, with the gamObjRoot as parent
                rootGamObjTree.Insert(shipRoot, null);

                //check the tree
                rootGamObjTree.DumpTree();

                ////attach a ship to the ship root;
                //Ship ship = new Ship(GameObject.Name.Ship, GameSprite.Name.Ship, 0, 0.0f, 0.0f);
                //rootGamObjTree.Insert(ship, shipRoot);
                //ship.ActivateCollisionSprite(pSB_Boxes);
                //ship.ActivateGameSprite(pSB_GameSprites);

                //attach the shipRoot to the root game object tree
                //GameObjectManager.AttachTree(shipRoot, rootGamObjTree);
                GameObjectManager.AttachTree(shipRoot);

                //create the ship manager that handles all the ship's states
                //ShipManager.Create();
                ShipManager.CreateShipManager();

                //check the tree
                rootGamObjTree.DumpTree();



                //------------------------------------------------
                // Create an Alien Tree

                //AlienRoot alienRoot = new AlienRoot(GameObject.Name.AlienRoot, GameSprite.SpriteName.Null_Object, 0, 0.0f, 0.0f);

                //create the PCS tree that will hold all alien game objects
                PCSTree pAlienTree = new PCSTree();
                DeathManager.Attach(pAlienTree);

                //create the alien game object factory with pAlienTree as the factory's tree
                //factory will attach all alien game objects to pAlienTree
                AlienFactory pAlienFactory = new AlienFactory(SpriteBatch.Name.GameSprites, pAlienTree);
                DeathManager.Attach(pAlienFactory);

                //attach grid as a child of game object root; grid will be parent of all alien game objects
                Grid pGrid = (Grid)pAlienFactory.Create(AlienType.Type.AlienGrid, GameObject.Name.Grid);
                
                // set the grid as the root parent to attach all aliens to;
                pAlienFactory.SetParent(pGrid);

                //Build the grid (builder pattern)
                int numberOfColumns = 11;

                //numberOfColumns = 1;
                AlienFactory.BuildAlienGrid(pAlienFactory, pGrid, numberOfColumns);

                //check the tree
                rootGamObjTree.DumpTree();

                //GameObjectManager.AttachTree(pGrid, rootGamObjTree);
                GameObjectManager.AttachTree(pGrid);

                //------------------------------------------------
                // Create Alien Bomb tree

                //create the root
                BombRoot pBombRoot = new BombRoot(GameObject.Name.BombRoot, GameSprite.Name.NullObject, 0, 0.0f, 0.0f);

                //insert root into the bomb tree
                rootGamObjTree.Insert(pBombRoot, null);
                //pBombRoot.ActivateCollisionSprite(pSB_Boxes);

                //add the bombtree to the root tree
                //GameObjectManager.AttachTree(pBombRoot, rootGamObjTree);
                GameObjectManager.AttachTree(pBombRoot);

                //check the tree
                rootGamObjTree.DumpTree();





                //------------------------------------------------
                // Associate and Create Walls
                //note screen = 672px wide by 768px

                WallRoot pWallRoot = new WallRoot(GameObject.Name.WallRoot, GameSprite.Name.NullObject, 0, 0.0f, 0.0f);

                rootGamObjTree.Insert(pWallRoot, null);

                WallTop pWallTop = new WallTop(GameObject.Name.WallTop, GameSprite.Name.HorizontalWall, 0, 336.0f, 728.0f, 612.0f, 10.0f);

                rootGamObjTree.Insert(pWallTop, pWallRoot);
                pWallTop.ActivateCollisionSprite(pSB_Boxes);

                WallBottom pWallBottom = new WallBottom(GameObject.Name.WallBottom, GameSprite.Name.HorizontalWall, 0, 336.0f, 40.0f, 612.0f, 10.0f);

                rootGamObjTree.Insert(pWallBottom, pWallRoot);
                pWallBottom.ActivateCollisionSprite(pSB_Boxes);

                WallRight pWallRight = new WallRight(GameObject.Name.WallRight, GameSprite.Name.HorizontalWall, 0, 652.0f, 384.0f, 10.0f, 693.0f);

                rootGamObjTree.Insert(pWallRight, pWallRoot);
                pWallRight.ActivateCollisionSprite(pSB_Boxes);

                WallLeft pWallLeft = new WallLeft(GameObject.Name.WallLeft, GameSprite.Name.HorizontalWall, 0, 20.0f, 384.0f, 10.0f, 693.0f);

                rootGamObjTree.Insert(pWallLeft, pWallRoot);
                pWallLeft.ActivateCollisionSprite(pSB_Boxes);

                //now that it's fully created, attach the newly created wall tree to the main game object tree
                //GameObjectManager.AttachTree(pWallRoot, rootGamObjTree);
                GameObjectManager.AttachTree(pWallRoot);

                //check the tree
                rootGamObjTree.DumpTree();






                ColPair pColPair = null;

                /*            */
                //------------------------------------------------
                // Associate and Create Collision Pairs

                // associate object roots in a collision pair

                pColPair = ColPairManager.Add(ColPair.Name.Missile_Wall, pMissileRoot, pWallRoot);
                Debug.Assert(pColPair != null);

                // associate object roots in a collision pair
                pColPair = ColPairManager.Add(ColPair.Name.Missile_Wall, pMissileRoot, pWallRoot);
                //associate the observers that will act on the collision
                pColPair.Attach(new ShipReadyObserver());
                pColPair.Attach(new ShipRemoveMissileObserver());

                //// associate object roots in a collision pair
                //ColPairManager.Add(ColPair.Name.Alien_Missile, pMissileRoot, pGrid);
    


                // Bomb vs wall bottom
                pColPair = ColPairManager.Add(ColPair.Name.Bomb_Wall, pBombRoot, pWallRoot);
                pColPair.Attach(new BombObserver());
                //todo debug this test bomb reset observer
                pColPair.Attach(new RemoveBombObserver());






                Debug.WriteLine("\n\n\n\n\n");
                GameObjectManager.DumpAll();







                Debug.WriteLine("\n\nLoad Content Complete\n----------------------------------\n");

                //-----------------------------------------------
                //Data Dump;

                TextureManager.DumpStats();
                ImageManager.DumpStats();
                GameSpriteManager.DumpStats();
                BoxSpriteManager.DumpStats();
                SpriteBatchManager.DumpStats();
                ProxySpriteManager.DumpStats();
                TimerEventManager.DumpStats();
                GameObjectManager.DumpStats();
                DeathManager.DumpStats();
                GhostManager.DumpStats();
                ColPairManager.DumpStats();
                GlyphManager.DumpStats();

                //TextureManager.DumpLists();
                //ImageManager.DumpLists();
                //GameSpriteManager.DumpLists();
                BoxSpriteManager.DumpLists();
                //SpriteBatchManager.DumpLists();
                //ProxySpriteManager.DumpLists();
                //TimerEventManager.DumpLists();
                //GameObjectManager.DumpLists();
                //DeathManager.DumpLists();
                //GhostManager.DumpLists();
                //ColPairManager.DumpLists();
                //GlyphManager.DumpLists();

                //TextureManager.DumpAll();
                //ImageManager.DumpAll();
                //GameSpriteManager.DumpAll();
                //BoxSpriteManager.DumpAll();
                //SpriteBatchManager.DumpAll();
                //ProxySpriteManager.DumpAll();
                //TimerEventManager.DumpAll();
                //GameObjectManager.DumpAll();
                //DeathManager.DumpAll();
                //GhostManager.DumpAll();
                //ColPairManager.DumpAll();
                //GlyphManager.DumpAll();
            }

            //-----------------------------------------------------------------------------
            // Game::Update()
            //      Called once per frame, update data, tranformations, etc
            //      Use this function to control process order
            //      Input, AI, Physics, Animation, and Graphics
            //-----------------------------------------------------------------------------
            public override void Update()
            {
 

                //Update game simulation
                // Single Step, Free running...
                Simulation.Update(this.GetTime());

                //always update input asap
                InputManager.Update();

                // Run based on simulation stepping
                if (Simulation.GetTimeStep() > 0.0f)
                {
                    // Fire off the timer events

                    //with simulator timer
                    TimerEventManager.Update(Simulation.GetTotalTime());
                    ////no simulator timer
                    //TimerEventManager.Update(this.GetTime());


                    // Do the collision checks
                    ColPairManager.Process();

                    //GameObjectManager updates ALL game objects and sprite positions
                    // remember each game object has a proxy sprite attached
                    GameObjectManager.Update();


                    // Delete any objects here...
                    DelayedObjectManager.Process();
                }




                ////always update input asap
                //InputMan.Update();

                //// Fire off the timer events
                //TimerMan.Update(this.GetTime());

                //// Do the collision checks
                //ColPairMan.Process();

                ////GameObjectManager updates ALL game objects and sprite positions
                //// remember each game object has a proxy sprite attached
                //// walk through all objects and push to flyweight
                //GameObjectMan.Update();

                //// Delete any objects here...
                //DelayedObjectMan.Process();
            }

            //-----------------------------------------------------------------------------
            // Game::Draw()
            //		This function is called once per frame
            //	    Use this for draw graphics to the screen.
            //      Only do rendering here
            //-----------------------------------------------------------------------------
            public override void Draw()
            {
                
                //toggle drawing collision boxes by pressing the 'c' key

                //draw all the sprites attached to sprite batches
                SpriteBatchManager.Draw();
            }

            //-----------------------------------------------------------------------------
            // Game::UnLoadContent()
            //       unload content (resources loaded above)
            //       unload all content that was loaded before the Engine Loop started
            //-----------------------------------------------------------------------------
            public override void UnLoadContent()
            {
                TextureManager.Destroy();
                ImageManager.Destroy();

                GameSpriteManager.Destroy();
                BoxSpriteManager.Destroy();

                SpriteBatchManager.Destroy();

                ProxySpriteManager.Destroy();

                TimerEventManager.Destroy();
                DeathManager.Destroy();
            }
        }

    }


}