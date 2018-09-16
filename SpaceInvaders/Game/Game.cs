using System;
using System.Diagnostics;
using SpaceInvaders.DestructorManagement;

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
            //load all new objects here and attach to death manager;

            //-----------------------------------------------
            //Texture Load

            //load texture sheets: images will be cut from these sheets
            TextureManager.Add(Texture.Name.SpaceInvadersMono4, "SpaceInvadersMono4.tga");
            TextureManager.Add(Texture.Name.GameSprites, "SpaceInvaderSprites_14x14.tga");

            //-----------------------------------------------
            //Image Load

            //load images from texture sheets above. input = coordinates on tga sheets

            //constants
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

            //Attach to appropriate Sprite Batch
            pSB_Aliens.Attach(GameSprite.Name.Squid);
            pSB_Aliens.Attach(GameSprite.Name.Crab);
            pSB_Aliens.Attach(GameSprite.Name.Octopus);

            //----------------------
            //BoxSprite (Alien)

            //add box to sx-sy, with a preset width/height
            BoxSpriteManager.Add(BoxSprite.Name.AlienBox, squid_sX, squid_sY, boxSize, boxSize);
            //Attach to appropriate Sprite Batch 
            pSB_Boxes.Attach(BoxSprite.Name.AlienBox);

            //----------------------
            //Animated Sprite 

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

            //----------------------
            //Proxy Sprite

            // create 10 proxies of each alien sprite
            //note that squid and the timer animation are independent
            float gap = 50.0f;
            float proxyX = 50.0f;
            for (int i = 0; i < 10; i++)
            {
                
                //create the proxy sprites
                ProxySprite squidProxy = ProxySpriteManager.Add(GameSprite.Name.Squid);
                ProxySprite crabProxy = ProxySpriteManager.Add(GameSprite.Name.Crab);
                ProxySprite octoProxy = ProxySpriteManager.Add(GameSprite.Name.Octopus);

                //set the coordinates
                squidProxy.x = proxyX + gap * i;
                squidProxy.y = 700.0f;

                crabProxy.x = proxyX + gap * i;
                crabProxy.y = 650.0f;

                octoProxy.x = proxyX + gap * i;
                octoProxy.y = 600.0f;

                //attach proxy sprite clones to the sprite batch for rendering
                pSB_Aliens.Attach(squidProxy);
                pSB_Aliens.Attach(crabProxy);
                pSB_Aliens.Attach(octoProxy);

            }



            //-----------------------------------------------
            //GameObject Load (Individual or Factory)

            //Method 1) Individual
            // Create a game object
            Squid pSquid = new Squid(GameObject.Name.Squid, GameSprite.Name.Squid, 50.0f, 200.0f);
            Crab pCrab = new Crab(GameObject.Name.Crab, GameSprite.Name.Crab, 50.0f, 150.0f);
            Octopus pOcto = new Octopus(GameObject.Name.Octopus, GameSprite.Name.Octopus, 50.0f, 100.0f);

            // add to the gameObjectManager
            GameObjectManager.Attach(pSquid);
            GameObjectManager.Attach(pCrab);
            GameObjectManager.Attach(pOcto);


            //change to color to differentiate from proxies
            pSquid.pProxySprite.pSprite.ChangeColor(1.0f, 0.0f, 0.0f);
            pCrab.pProxySprite.pSprite.ChangeColor(0.0f, 1.0f, 0.0f);
            pOcto.pProxySprite.pSprite.ChangeColor(0.0f, 0.0f, 1.0f);


            // Attach the GameObject's associated proxy sprites to sprite batch for rendering
            pSB_Aliens.Attach(pSquid.pProxySprite);
            pSB_Aliens.Attach(pCrab.pProxySprite);
            pSB_Aliens.Attach(pOcto.pProxySprite);


            //----------------------
            //Method 2) Factory

            // create the Alien Factory 
            //AlienFactory AlienFactory = new AlienFactory(SpriteBatch.Name.GameSprites);

            // eventually we'll make one game object per column
            // then use proxies to cookie-cut copy each game object
            // to make more sprites in the grid;

            // create 15 game objects quickly
            //for (int i = 0; i < 5; i++)
            //{
            //    AlienFactory.Create(AlienType.Type.Crab, 100.0f + i * 150.0f, 700.0f);
            //    AlienFactory.Create(AlienType.Type.Squid, 50.0f + 40.0f * i, 600.0f);
            //    AlienFactory.Create(AlienType.Type.Octopus, 100.0f + i * 150.0f, 450.0f);
            //}



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


            //TextureManager.DumpLists();
            //ImageManager.DumpLists();
            //GameSpriteManager.DumpLists();
            //BoxSpriteManager.DumpLists();
            //SpriteBatchManager.DumpLists();
            //ProxySpriteManager.DumpLists();
            //TimerEventManager.DumpLists();
            //GameObjectManager.DumpLists();
            //DeathManager.DumpLists();



            //TextureManager.DumpAll();
            //ImageManager.DumpAll();
            //GameSpriteManager.DumpAll();
            //BoxSpriteManager.DumpAll();
            //SpriteBatchManager.DumpAll();
            //ProxySpriteManager.DumpAll();
            //TimerEventManager.DumpAll();
            //GameObjectManager.DumpAll();
            //DeathManager.DumpAll();
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
            //Sprites

            //GameSprite pSquid = GameSpriteManager.Find(GameSprite.Name.Squid);
            //Debug.Assert(pSquid != null);
            //pSquid.Update();

            //GameSprite pCrab = GameSpriteManager.Find(GameSprite.Name.Crab);
            //Debug.Assert(pSquid != null);
            //pCrab.Update();

            //GameSprite pOctopus = GameSpriteManager.Find(GameSprite.Name.Octopus);
            //Debug.Assert(pSquid != null);
            //pOctopus.Update();

            //-----------------------------------------------
            //GameObjects

            //GameObject pAlienSquid = GameObjectManager.Find(GameObject.Name.Squid);
            //Debug.Assert(pAlienSquid != null);
            //pAlienSquid.Update();

            //GameObject pAlienCrab = GameObjectManager.Find(GameObject.Name.Crab);
            //Debug.Assert(pAlienCrab != null);
            //pAlienCrab.Update();

            //GameObject pAlienOctopus = GameObjectManager.Find(GameObject.Name.Octopus);
            //Debug.Assert(pAlienOctopus != null);
            //pAlienOctopus.Update();

            //-----------------------------------------------
            //sprite box
            BoxSprite pAlienBox = BoxSpriteManager.Find(BoxSprite.Name.AlienBox);
            pAlienBox.Update();

            //GameObjectManager updates ALL game objects and sprite positions
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

