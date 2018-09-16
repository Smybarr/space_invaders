using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    class SpaceInvaders : Azul.Game
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

            int windowWidth = 224 * 3; 
            int windowHeight = 256 * 3; 

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
            //Manager Load
            TextureManager.Create();
            ImageManager.Create();

            GameSpriteManager.Create(5, 2);
            BoxSpriteManager.Create();

            SpriteBatchManager.Create();

            ProxySpriteManager.Create(10, 1);
            GameObjectManager.Create();


            TimerEventManager.Create();
            DeathManager.Create(1, 1);
            GhostManager.Create(1, 1);
            //load all new objects here and attach to death manager;

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


            //-----------------------------------------------
            //Texture Load

            //load texture sheets: images will be cut from these sheets
            TextureManager.Add(Texture.Name.SpaceInvadersMono4, "SpaceInvadersMono4.tga");
            TextureManager.Add(Texture.Name.GameSprites, "SpaceInvaderSprites_14x14.tga");









            //-----------------------------------------------
            //Image Load

            //load images from texture sheets above. input = coordinates on tga sheets

            //constant image rect values
            // topLeft x, y & image width/height values
            // from "SpaceInvaderSprites_14x14.tga" texture sheet
            float constAlienOpen_tlY = 28.0f;
            float constAlienClosed_tlY = 182.0f;
            float constAlien_ImageHeight = 112.0f;

            //---------------------------------
            float squidOpen_tlX = 616.0f;
            float squidClosed_tlX = 616.0f;

            float squid_ImageWidth = 112.0f;

            //---------------------------------
            float crabOpen_tlX = 322.0f;
            float crabClosed_tlX = 322.0f;
           
            float crab_ImageWidth = 154.0f;

            //---------------------------------
            float octoOpen_tlX = 56.0f;
            float octoClosed_tlX = 56.0f;

            float octo_ImageWidth = 168.0f;

            //---------------------------------
            //Load the images


            //Squid Open
            ImageManager.Add(Image.Name.SquidOpen, Texture.Name.GameSprites, squidOpen_tlX, constAlienOpen_tlY, squid_ImageWidth, constAlien_ImageHeight);
            //Squid Closed
            ImageManager.Add(Image.Name.SquidClosed, Texture.Name.GameSprites, squidClosed_tlX, constAlienClosed_tlY, squid_ImageWidth, constAlien_ImageHeight);

            //Crab Open
            ImageManager.Add(Image.Name.CrabOpen, Texture.Name.GameSprites, crabOpen_tlX, constAlienOpen_tlY, crab_ImageWidth, constAlien_ImageHeight);
            //Crab Closed
            ImageManager.Add(Image.Name.CrabClosed, Texture.Name.GameSprites, crabClosed_tlX, constAlienClosed_tlY, crab_ImageWidth, constAlien_ImageHeight);

            //Octopus Open
            ImageManager.Add(Image.Name.OctopusOpen, Texture.Name.GameSprites, octoOpen_tlX, constAlienOpen_tlY, octo_ImageWidth, constAlien_ImageHeight);
            //Octopus Closed
            ImageManager.Add(Image.Name.OctopusClosed, Texture.Name.GameSprites, octoClosed_tlX, constAlienClosed_tlY, octo_ImageWidth, constAlien_ImageHeight);









            //-----------------------------------------------
            //Sprite Batch Creation
            SpriteBatch pSB_Aliens = SpriteBatchManager.Add(SpriteBatch.Name.GameSprites);
            SpriteBatch pSB_Boxes = SpriteBatchManager.Add(SpriteBatch.Name.SpriteBoxes);









            //-----------------------------------------------
            //Sprites/BoxSprites Load
            //NOTE that the following coordinates will not matter
            //  once proxy sprites are used!

            //35w x 35h for all sprites;
            float const_SpriteSize = 35.0f;

            //render dimensions/coordinates
            float squid_sX = 400.0f;
            float squid_sY = 400.0f;

            float crab_sX = 400.0f;
            float crab_sY = 350.0f;

            float octo_sX = 400.0f;
            float octo_sY = 300.0f;

            float boxSize = 36.0f;

            //----------------------
            //GameSprite (Alien)

            //squid game sprite
            GameSpriteManager.Add(GameSprite.Name.Squid, Image.Name.SquidOpen, squid_sX, squid_sY, const_SpriteSize, const_SpriteSize);
            //crab game sprite
            GameSpriteManager.Add(GameSprite.Name.Crab, Image.Name.CrabOpen, crab_sX, crab_sY, const_SpriteSize, const_SpriteSize);
            //octopus game sprite
            GameSpriteManager.Add(GameSprite.Name.Octopus, Image.Name.OctopusOpen, octo_sX, octo_sY, const_SpriteSize, const_SpriteSize);

            //Attachment to sprite batch now happens inside of the game object creation for ALIENS ONLY!!!
            ////Attach to appropriate Sprite Batch
            //pSB_Aliens.Attach(GameSprite.Name.Squid);
            //pSB_Aliens.Attach(GameSprite.Name.Crab);
            //pSB_Aliens.Attach(GameSprite.Name.Octopus);

            //----------------------
            //BoxSprite (Alien)

            //todo fix boxsprite rendering issues after gameobject-pcstree refactor
            //add box to sx-sy, with a preset width/height
            BoxSpriteManager.Add(BoxSprite.Name.AlienBox, squid_sX, squid_sY, boxSize, boxSize);
            ////Attach to appropriate Sprite Batch 
            //pSB_Boxes.Attach(BoxSprite.Name.AlienBox);














            //----------------------
            //Animated Sprite (Attached to TimerManager
            //Each animation is a TimerEvent following command pattern;

            // Create a squid animation sprite
            AnimationSprite pAnim_Squid = new AnimationSprite(GameSprite.Name.Squid);
            AnimationSprite pAnim_Crab = new AnimationSprite(GameSprite.Name.Crab);
            AnimationSprite pAnim_Octo = new AnimationSprite(GameSprite.Name.Octopus);
            
            // attach to death manager for garbage collection override
            DeathManager.Attach(pAnim_Squid);
            DeathManager.Attach(pAnim_Crab);
            DeathManager.Attach(pAnim_Octo);

            // attach alternating images to animation cycle
            pAnim_Squid.Attach(Image.Name.SquidOpen);
            pAnim_Squid.Attach(Image.Name.SquidClosed);

            pAnim_Crab.Attach(Image.Name.CrabOpen);
            pAnim_Crab.Attach(Image.Name.CrabClosed);

            pAnim_Octo.Attach(Image.Name.OctopusOpen);
            pAnim_Octo.Attach(Image.Name.OctopusClosed);

            // add AnimationSprite to timer

            //set the interval between events and add sprite animation event objects to timer manager;
            float animInterval = 1.0f;
            TimerEventManager.Add(TimerEvent.Name.SpriteAnimation, pAnim_Squid, animInterval);
            TimerEventManager.Add(TimerEvent.Name.SpriteAnimation, pAnim_Crab, animInterval);
            TimerEventManager.Add(TimerEvent.Name.SpriteAnimation, pAnim_Octo, animInterval);













            //-----------------------------------------------
            //GameObject Load (Factory)

            // Create a AlienTree
            PCSTree pAlienTree = new PCSTree();

            // create the factory 
            AlienFactory AlienFactory = new AlienFactory(SpriteBatch.Name.GameSprites, pAlienTree);
            DeathManager.Attach(AlienFactory);

            // attach grid as a child of game object root; grid will be parent of all alien game objects
            AlienType pGrid = AlienFactory.Create(AlienType.Type.AlienGrid, GameObject.Name.Grid);

            // set the grid as the root parent to attach all aliens to
            AlienFactory.SetParent(pGrid);

            float gap = 50.0f;
            float proxyX = 50.0f;
            // create 30 alien game objects quickly and attach to grid parent
            for (int i = 0; i < 10; i++)
            {
                // load children of grid;
                AlienFactory.Create(AlienType.Type.Crab, GameObject.Name.Crab, i, proxyX + gap * i, 700.0f);
                AlienFactory.Create(AlienType.Type.Squid, GameObject.Name.Squid, i, proxyX + gap * i, 650.0f);
                AlienFactory.Create(AlienType.Type.Octopus, GameObject.Name.Octopus, i, proxyX + gap * i, 600.0f);
            }
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

            //TextureManager.DumpLists();
            //ImageManager.DumpLists();
            //GameSpriteManager.DumpLists();
            //BoxSpriteManager.DumpLists();
            //SpriteBatchManager.DumpLists();
            //ProxySpriteManager.DumpLists();
            //TimerEventManager.DumpLists();
            //GameObjectManager.DumpLists();
            //DeathManager.DumpLists();
            //GhostManager.DumpLists();


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
        }

        //-----------------------------------------------------------------------------
        // Game::Update()
        //      Called once per frame, update data, tranformations, etc
        //      Use this function to control process order
        //      Input, AI, Physics, Animation, and Graphics
        //-----------------------------------------------------------------------------
        public override void Update()
        {
            //KeyboardTest();
            //MouseTest();

            // Fire off the timer events
            TimerEventManager.Update(this.GetTime());

            //-----------------------------------------------
            //sprite box
            BoxSprite pAlienBox = BoxSpriteManager.Find(BoxSprite.Name.AlienBox);
            pAlienBox.Update();

            //GameObjectManager updates ALL game objects and sprite positions
            // remember each game object has a proxy sprite attached
            GameObjectManager.Update();

        }

        //-----------------------------------------------------------------------------
        // Game::Draw()
        //		This function is called once per frame
        //	    Use this for draw graphics to the screen.
        //      Only do rendering here
        //-----------------------------------------------------------------------------
        public override void Draw()
        {
            //-----------------------------------------------
            //sprites
            SpriteBatchManager.renderBoxes = true;

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

