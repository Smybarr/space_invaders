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


                //Attaching to the SpriteBatch is now handled inside of 

                //Attach to appropriate Sprite Batch
                //pSB_Aliens.Attach(GameSprite.Name.Squid);
                //pSB_Aliens.Attach(GameSprite.Name.Crab);
                //pSB_Aliens.Attach(GameSprite.Name.Octopus);

                //----------------------
                //BoxSprite (Alien)

                //add box to sx-sy, with a preset width/height
                BoxSpriteManager.Add(BoxSprite.Name.AlienBox, squid_sX, squid_sY, boxSize, boxSize);
                //Attach to appropriate Sprite Batch 
                //pSB_Boxes.Attach(BoxSprite.Name.AlienBox);

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
                Squid pSquid = new Squid(GameObject.Name.Squid, GameSprite.Name.Squid, 1, 50.0f, 200.0f);
                Crab pCrab = new Crab(GameObject.Name.Crab, GameSprite.Name.Crab, 2, 50.0f, 150.0f);
                Octopus pOcto = new Octopus(GameObject.Name.Octopus, GameSprite.Name.Octopus, 3, 50.0f, 100.0f);

                // add to the gameObjectManager
                //GameObjectManager.Attach(pSquid);
                //GameObjectManager.Attach(pCrab);
                //GameObjectManager.Attach(pOcto);


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






        class TestGame : Azul.Game
        {
            //Static Alien Sprites (Practice PA2) - need to work this into a factory pattern
            Azul.Sprite pStaticAlien1;
            Azul.Sprite pStaticAlien2;
            Azul.Sprite pStaticAlien3;
            Azul.Sprite pStaticAlien4;
            Azul.Sprite pStaticAlien5;

            //Dynamic Alien Sprites (Practice PA2) - need to work this into a factory pattern
            Azul.Sprite pDynamicAlien1;
            Azul.Sprite pDynamicAlien2;
            Azul.Sprite pDynamicAlien3;
            Azul.Sprite pDynamicAlien4;

            //FIRE!!!!!!!! 
            Azul.Sprite pDynamicFire1;
            Azul.Sprite pDynamicFire2;
            Azul.Sprite pDynamicFire3;
            Azul.Sprite pDynamicFire4;

            //Sprite Boxes
            Azul.SpriteBox pSpriteBox;
            Azul.SpriteBox pSpriteBox2;

            float dynamicFire3Speed = 2.0f;

            float fireSpeedX1 = 2.0f;
            float fireSpeedY1 = 2.0f;

            float fireSpeedX2 = 4.0f;
            float fireSpeedY2 = 4.0f;

            float dynamicAlien4Speed = 0.02f;


            // float practiceDynamicAlienSpeedX;
            // float practiceDynamicAlienSpeedY;

            Azul.Texture pPracticeAliensTex;

            //ScreenWidth & ScreenHeight not 800/600?
            float ScreenX = 0.0f;
            float ScreenWidth = 300.0f;
            float ScreenHeight = 100.0f;

            float blue = 0.0f;
            float red = 1.0f;
            float green = 0.5f;

            float AlienPosX = 0.0f;
            float AlienPosY = 0.0f;
            float AlienAngle = 0.0f;

            float AlienPosX2 = 0.0f;

            float FirePosX = 0.0f;
            float FirePosY = 0.0f;
            float FireAngle = 0.0f;

            int count = 0;
            int fireCount = 0;

            //-----------------------------------------------------------------------------
            // Game::Initialize()
            //		Allows the engine to perform any initialization it needs to before 
            //      starting to run.  This is where it can query for any required services 
            //      and load any non-graphic related content. 
            //-----------------------------------------------------------------------------
            public override void Initialize()
            {
                Debug.WriteLine("----------------------------------");
                Debug.WriteLine("TEST - Initialize Game");
                Debug.WriteLine("----------------------------------");

                // Game Window Device setup
                this.SetWindowName("TEST RENDER: Space Invaders SMYBARR 2018");

                int windowWidth = 224 * 3;
                int windowHeight = 256 * 3;


                this.SetWidthHeight(windowWidth, windowHeight);
                this.SetClearColor(0.1f, 0.1f, 0.1f, 1.0f);

                Debug.WriteLine("\n------Window Loaded-------\nWidth:\t{0}\nHeight:\t{1}\n\n", this.GetScreenWidth(), this.GetScreenHeight());

                Debug.WriteLine("\n\nTest Initialization Complete\n----------------------------------");
            }

            //-----------------------------------------------------------------------------
            // Game::LoadContent()
            //		Allows you to load all content needed for your engine,
            //	    such as objects, graphics, etc.
            //-----------------------------------------------------------------------------

            public override void LoadContent()
            {

                Debug.WriteLine("----------------------------------");
                Debug.WriteLine("TEST Load Content");
                Debug.WriteLine("----------------------------------\n");

                //---------------------------------------------------------------------------------------------------------
                // Load the Textures
                //---------------------------------------------------------------------------------------------------------

                //create new texture object using specified file as the source, then run a null check;
                pPracticeAliensTex = new Azul.Texture("aliens14x14.tga");
                Debug.Assert(pPracticeAliensTex != null);


                //---------------------------------------------------------------------------------------------------------
                // Create Sprites
                //---------------------------------------------------------------------------------------------------------

                //This is the static short box that is ALWAYS yellow; Location (550.0f width on screen x 500.0f hight on screen, with TOP LEFT corner of screen as origin)
                pSpriteBox = new Azul.SpriteBox(new Azul.Rect(550.0f, 500.0f, 50.0f, 150.0f), new Azul.Color(1.0f, 1.0f, 1.0f, 1.0f));
                Debug.Assert(pSpriteBox != null);

                pSpriteBox2 = new Azul.SpriteBox(new Azul.Rect(550.0f, 100.0f, 50.0f, 100.0f), new Azul.Color(1.0f, 1.0f, 0.0f));
                Debug.Assert(pSpriteBox2 != null);

                //x = 200, y = 400, w x h on screen) new Azul.Rect(253.0f, 63.0f, 85.0f, 64.0f), new Azul.Rect(500.0f, 300.0f, 150.0f, 150.0f)
                //see stitch explanation & stitch .tga file in gimp for reference!
                //pStaticAlien1 = new Azul.Sprite(pPracticeAliensTex, new Azul.Rect(15.0f, 15.0f, 250.0f, 140.0f), new Azul.Rect(550.0f, 500.0f, 100.0f, 100.0f));

                /* Observations on making sprites:
                 *      - Update() method MUST be used (below) to change the position of the sprite on the screen as specified by args 1,2 in Rect 2!!!!! args 3, 4 used for sizing
                 *      - first Rect() is used to do the cookie cutting of the texture from the TGA file; include origins and DOUBLE the height & width of the texture box since origin is at the center!
                 *      - second Rect() box is used to dictate the size of the sprite on the screen, especially the last 2 args - higher the numbers, the bigger the size!
                 */

                //Practice STATIC alien sprites 

                pStaticAlien1 = new Azul.Sprite(pPracticeAliensTex, new Azul.Rect(15.0f, 18.0f, 248.0f, 135.0f), new Azul.Rect(100.0f, 500.0f, 100.0f, 100.0f));
                Debug.Assert(pStaticAlien1 != null);

                pStaticAlien2 = new Azul.Sprite(pPracticeAliensTex, new Azul.Rect(283.0f, 18.0f, 248.0f, 135.0f), new Azul.Rect(200.0f, 200.0f, 150.0f, 150.0f));
                Debug.Assert(pStaticAlien2 != null);

                pStaticAlien3 = new Azul.Sprite(pPracticeAliensTex, new Azul.Rect(548.0f, 18.0f, 248.0f, 135.0f), new Azul.Rect(700.0f, 300.0f, 200.0f, 200.0f));
                Debug.Assert(pStaticAlien3 != null);

                pStaticAlien4 = new Azul.Sprite(pPracticeAliensTex, new Azul.Rect(15.0f, 170.0f, 248.0f, 135.0f), new Azul.Rect(400.0f, 100.0f, 50.0f, 50.0f));
                Debug.Assert(pStaticAlien4 != null);

                pStaticAlien5 = new Azul.Sprite(pPracticeAliensTex, new Azul.Rect(283.0f, 172.0f, 248.0f, 135.0f), new Azul.Rect(500.0f, 400.0f, 70.0f, 70.0f));
                Debug.Assert(pStaticAlien5 != null);


                //Practice DYNAMIC alien sprites

                pDynamicAlien1 = new Azul.Sprite(pPracticeAliensTex, new Azul.Rect(548.0f, 170.0f, 248.0f, 135.0f), new Azul.Rect(350.0f, 225.0f, 100.0f, 100.0f));
                Debug.Assert(pDynamicAlien1 != null);

                pDynamicAlien2 = new Azul.Sprite(pPracticeAliensTex, new Azul.Rect(18.0f, 340.0f, 248.0f, 105.0f), new Azul.Rect(200.0f, 200.0f, 100.0f, 100.0f));
                Debug.Assert(pDynamicAlien2 != null);

                pDynamicAlien3 = new Azul.Sprite(pPracticeAliensTex, new Azul.Rect(548.0f, 18.0f, 248.0f, 135.0f), new Azul.Rect(300.0f, 300.0f, 100.0f, 100.0f));
                Debug.Assert(pDynamicAlien3 != null);

                pDynamicAlien4 = new Azul.Sprite(pPracticeAliensTex, new Azul.Rect(395.0f, 480.0f, 135.0f, 131.0f), new Azul.Rect(100.0f, 100.0f, 100.0f, 100.0f));
                Debug.Assert(pDynamicAlien4 != null);

                //FIRE!!!!

                pDynamicFire1 = new Azul.Sprite(pPracticeAliensTex, new Azul.Rect(480.0f, 633.0f, 60.0f, 110.0f), new Azul.Rect(475.0f, 175.0f, 75.0f, 75.0f));
                Debug.Assert(pDynamicFire1 != null);

                pDynamicFire2 = new Azul.Sprite(pPracticeAliensTex, new Azul.Rect(563.0f, 633.0f, 60.0f, 110.0f), new Azul.Rect(475.0f, 275.0f, 75.0f, 75.0f));
                Debug.Assert(pDynamicFire2 != null);

                pDynamicFire3 = new Azul.Sprite(pPracticeAliensTex, new Azul.Rect(645.0f, 633.0f, 60.0f, 110.0f), new Azul.Rect(475.0f, 375.0f, 75.0f, 75.0f));
                Debug.Assert(pDynamicFire3 != null);

                pDynamicFire4 = new Azul.Sprite(pPracticeAliensTex, new Azul.Rect(730.0f, 633.0f, 60.0f, 110.0f), new Azul.Rect(475.0f, 475.0f, 75.0f, 75.0f));
                Debug.Assert(pDynamicFire4 != null);


                //---------------------------------------------------------------------------------------------------------
                // Demo variables
                //---------------------------------------------------------------------------------------------------------

                Debug.WriteLine("(Width,Height): {0}, {1}", this.GetScreenWidth(), this.GetScreenHeight());

                //AlienPosX = pPracticeStaticAlien.x;
                //AlienPosY = pPracticeStaticAlien.y;


                Debug.WriteLine("\n\nTEST Load Content Complete\n----------------------------------");

            }

            ////-----------------------------------------------------------------------------
            //// Game::Update()
            ////      Called once per frame, update data, tranformations, etc
            ////      Use this function to control process order
            ////      Input, AI, Physics, Animation, and Graphics
            ////-----------------------------------------------------------------------------
            public override void Update()
            {
                //Add your update below this line: ----------------------------

                //KeyboardTest();
                //MouseTest();

                //--------------------------------------------------------
                // Update Static Aliens
                //--------------------------------------------------------

                pStaticAlien1.Update();
                pStaticAlien2.Update();
                pStaticAlien3.Update();
                pStaticAlien4.Update();
                pStaticAlien5.Update();

                pDynamicAlien1.Update();
                pDynamicAlien2.Update();
                pDynamicAlien3.Update();
                pDynamicAlien4.Update();


                pDynamicFire1.Update();
                pDynamicFire2.Update();
                pDynamicFire3.Update();
                pDynamicFire4.Update();

                //--------------------------------------------------------
                // Change Texture, TextureRect, Color
                //--------------------------------------------------------

                count++;

                if (count == 100)
                {
                    pDynamicAlien1.SwapTexture(pPracticeAliensTex);
                    pDynamicAlien1.SwapTextureRect(new Azul.Rect(15.0f, 18.0f, 248.0f, 135.0f));

                    pDynamicAlien1.SwapColor(new Azul.Color(1.0f, 0.0f, 0.0f, 1.0f));

                    pStaticAlien1.SwapColor(new Azul.Color(0.0f, 1.0f, 0.0f, 1.0f));

                    pDynamicAlien2.SwapColor(new Azul.Color(1.0f, 0.0f, 0.0f, 1.0f));

                    pStaticAlien2.SwapColor(new Azul.Color(0.0f, 1.0f, 0.0f, 1.0f));


                }
                else if (count == 200)
                {
                    pDynamicAlien1.SwapTexture(pPracticeAliensTex);
                    pDynamicAlien1.SwapTextureRect(new Azul.Rect(548.0f, 170.0f, 248.0f, 135.0f));

                    pDynamicAlien1.SwapColor(new Azul.Color(0.0f, 1.0f, 0.0f, 1.0f));

                    pStaticAlien1.SwapColor(new Azul.Color(1.0f, 0.0f, 0.0f, 1.0f));

                    pDynamicAlien2.SwapColor(new Azul.Color(0.0f, 1.0f, 0.0f, 1.0f));

                    pStaticAlien2.SwapColor(new Azul.Color(1.0f, 0.0f, 0.0f, 1.0f));


                    count = 0;
                }

                //separate counter for dancing fire (this totally needs to be refactored more efficiently)
                fireCount++;

                if (fireCount == 10)
                {
                    pDynamicFire1.SwapTexture(pPracticeAliensTex);
                    pDynamicFire1.SwapTextureRect(new Azul.Rect(563.0f, 633.0f, 60.0f, 110.0f));

                    pDynamicFire1.SwapColor(new Azul.Color(1.0f, 0.0f, 0.0f, 1.0f));

                }
                else if (fireCount == 20)
                {
                    pDynamicFire1.SwapTexture(pPracticeAliensTex);
                    pDynamicFire1.SwapTextureRect(new Azul.Rect(645.0f, 633.0f, 60.0f, 110.0f));

                    pDynamicFire1.SwapColor(new Azul.Color(1.0f, 1.0f, 0.0f, 1.0f));

                }
                else if (fireCount == 30)
                {
                    pDynamicFire1.SwapTexture(pPracticeAliensTex);
                    pDynamicFire1.SwapTextureRect(new Azul.Rect(730.0f, 633.0f, 60.0f, 110.0f));

                    pDynamicFire1.SwapColor(new Azul.Color(0.0f, 1.0f, 1.0f, 1.0f));

                    fireCount = 0;
                }


                //--------------------------------------------------------
                // Alien - Angles,position - causes 
                //--------------------------------------------------------

                AlienAngle += 0.5f;

                //directs sprite from left to right of screen
                //AlienPosX += 10.0f;
                //if (AlienPosX > 800.0f)
                //    AlienPosX = 0.0f;

                //directs sprite from right to left of screen
                AlienPosX -= 10.0f;
                if (AlienPosX == 0.0f)
                    AlienPosX = 800.0f;

                //directs sprite from bottom to top of screen
                AlienPosY += 0.0f;
                if (AlienPosY > 600.0f)
                    AlienPosY = 0.0f;

                //directs sprite from top to bottom of screen
                //AlienPosY += 0.0f;
                //if (AlienPosY == 0.0f)
                //    AlienPosY = 600.0f;


                pDynamicAlien2.x = AlienPosX;
                //pDynamicAlien2.y = AlienPosY;
                pDynamicAlien2.angle = AlienAngle;
                pDynamicAlien2.Update();


                //--------------------------------------------------------
                //Make some notes about how exactly this works... what effect does each variable have on the action of the sprite?
                //

                FireAngle += 100.5f;
                //Causes the sprite to reset/respawn at the SIDE of the screen after leaving the screen from the other side, since screen width = 800

                FirePosX += 0.0f;
                if (FirePosX > 800.0f)
                    FirePosX = 200.0f;
                //Causes the sprite to reset/respawn at the bottom of the screen after leaving the screen from the top, since screen height = 600
                FirePosY += 15.0f;
                if (FirePosY > 600.0f)
                    FirePosY = 0.0f;

                //by NOT assigning x/y attributes of the sprite to the updated FirePosX/FirePosY values, you can
                //control what part of the 

                //pDynamicFire1.x = FirePosX;
                pDynamicFire1.y = FirePosY;
                pDynamicFire1.angle = FireAngle;
                pDynamicFire1.Update();


                //--------------------------------------------------------
                // Sprite - Scale (flips the sprite upside down?)
                //--------------------------------------------------------

                //pStaticAlien5.sy = 3.0f;
                //pStaticAlien5.Update();

                pDynamicAlien1.sy = -1.0f;
                pDynamicAlien1.Update();

                //--------------------------------------------------------
                // SpriteBox - creathes the boxes that will be necessary for detecting collisions and registering damage to aliens!
                //--------------------------------------------------------

                pSpriteBox.Update();
                pSpriteBox2.Update();

                //--------------------------------------------------------
                // Swap Color - self explanatory
                //--------------------------------------------------------

                //NOTE: this code structure GRADUALLY shifts the color tone rather than just alternate between 2 or more colors!
                blue += 0.001f;
                red -= 0.002f;
                green += 0.005f;

                if (red <= 0.0f)
                    red = 1.0f;
                pStaticAlien3.SwapColor(new Azul.Color(red, 0.0f, blue));
                pStaticAlien3.Update();


                if (green >= 1.0f)
                    green = 0.0f;
                pDynamicAlien4.SwapColor(new Azul.Color(red, green, blue));
                pDynamicAlien4.Update();

                pStaticAlien5.SwapColor(new Azul.Color(0.0f, 0.0f, blue));
                pStaticAlien5.Update();


                //--------------------------------------------------------
                // Swap Screen Rect - still working on what this does exactly....Seeing as how the respawn code from "Aliens angles postion causes" section doesn't work
                // and judging by the name, I'd say the SwapScreenRect() function removes the sprite off screen completely?
                //--------------------------------------------------------

                ScreenX += 6;
                ScreenWidth -= 3;
                ScreenHeight += 1;
                pStaticAlien2.SwapScreenRect(new Azul.Rect(ScreenX, 500, ScreenWidth, ScreenHeight));

                AlienPosX2 -= 10.0f;
                if (AlienPosX2 > 800.0f)
                    AlienPosX2 = 0.0f;

                pStaticAlien2.x = AlienPosX2;
                pStaticAlien2.Update();

                //pAlienSwap.SwapScreenRect(new Azul.Rect(ScreenX, 100, ScreenWidth, ScreenHeight));
                //pAlienSwap.Update();


                //--------------------------------------------------------
                // The Pacer - this code block is what alternates the sprites back and forth!
                //--------------------------------------------------------

                if (pDynamicFire1.x > this.GetScreenWidth() || pDynamicFire1.x < 0.0f)
                {
                    dynamicFire3Speed *= -1.0f;
                }
                pDynamicFire1.x += dynamicFire3Speed;
                pDynamicFire1.Update();

                //--------------------------------------------------------
                // Rotating fire sprite - purpose of the if statements is to keep sprite within boundries of screen
                //--------------------------------------------------------

                if (pDynamicFire2.x > this.GetScreenWidth() || pDynamicFire2.x < 0.0f)
                {
                    fireSpeedX2 *= -1.0f;

                    //pYellowBird.SwapTextureRect(new Azul.Rect(246, 135, 99, 72));

                    //bumpSnd = Azul.Audio.playSound("A.wav", false, true, true);
                    //bumpSnd.Pause(false);
                }

                if (pDynamicFire2.y > this.GetScreenHeight() || pDynamicFire2.y < 0.0f)
                {

                    //bumpSnd = Azul.Audio.playSound("A.wav", false, true, true);
                    //bumpSnd.Pause(false);

                    //pYellowBird.SwapTextureRect(new Azul.Rect(124, 34, 60, 56));

                    fireSpeedY2 *= -1.0f;
                }
                pDynamicFire2.x += fireSpeedX2;
                pDynamicFire2.y += fireSpeedY2;

                pDynamicFire2.Update();

                //--------------------------------------------------------
                // Move Around Screen @ angle, bounce off sides of the screen!
                //--------------------------------------------------------
                if (pDynamicFire3.x > this.GetScreenWidth() || pDynamicFire3.x < 0.0f)
                {
                    fireSpeedX1 *= -1.0f;
                }
                if (pDynamicFire3.y > this.GetScreenHeight() || pDynamicFire3.y < 0.0f)
                {
                    fireSpeedY1 *= -1.0f;
                }
                pDynamicFire3.x += fireSpeedX1;
                pDynamicFire3.y += fireSpeedY1;
                pDynamicFire3.angle += 0.05f;

                pDynamicFire3.Update();

                //--------------------------------------------------------
                // THE PUFFER!! (causes the sprite to inflate/deflate)
                //--------------------------------------------------------

                if (pDynamicAlien4.sx > 5.0f || pDynamicAlien4.sy < 1.0f)
                {
                    dynamicAlien4Speed *= -1.0f;
                }
                pDynamicAlien4.sx += dynamicAlien4Speed;
                pDynamicAlien4.sy += dynamicAlien4Speed;

                pDynamicAlien4.Update();


            }

            //-----------------------------------------------------------------------------
            // Game::Draw()
            //		This function is called once per frame
            //	    Use this for draw graphics to the screen.
            //      Only do rendering here
            //-----------------------------------------------------------------------------
            public override void Draw()
            {
                // draw all objects using the .Render() method!! NEEDED FOR OBJECTS TO SHOW UP ON SCREEN!

                //pSpriteBox.Render();
                //pSpriteBox2.Render();

                pStaticAlien1.Render();
                pStaticAlien2.Render();
                pStaticAlien3.Render();
                pStaticAlien4.Render();
                pStaticAlien5.Render();

                pDynamicAlien1.Render();
                pDynamicAlien2.Render();
                pDynamicAlien3.Render();
                pDynamicAlien4.Render();

                pDynamicFire1.Render();
                pDynamicFire2.Render();
                pDynamicFire3.Render();
                pDynamicFire4.Render();

            }

            //-----------------------------------------------------------------------------
            // Game::UnLoadContent()
            //       unload content (resources loaded above)
            //       unload all content that was loaded before the Engine Loop started
            //-----------------------------------------------------------------------------
            public override void UnLoadContent()
            {

            }

        }
    }


}