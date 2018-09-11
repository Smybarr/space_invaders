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


            TimerEventManager.Create();

            DeathManager.Create(1, 1);

            //load all new objects here and attach to death manager;

            //SquidOpen initial subrect;
            Azul.Rect imageSubRect = new Azul.Rect(548.0f, 18.0f, 248.0f, 135.0f);
            DeathManager.Attach(imageSubRect);

            //render dimensions/coordinates
            float spriteWidth = 65.0f;
            float spriteHeight = 35.0f;

            float screenX = 300.0f;
            float screenY = 400.0f;

            float boxWidth = 33.0f;
            float boxHeight = 33.0f;

            Azul.Rect box_pos_size = new Azul.Rect(screenX, screenY, boxWidth, boxHeight);
            DeathManager.Attach(box_pos_size);
            //-----------------------------------------------
            //textures

            //load texture sheets: images will be cut from these sheets
            //TextureManager.Add(Texture.Name.SpaceInvadersMono4, "SpaceInvadersMono4.tga");
            TextureManager.Add(Texture.Name.GameSprites, "SpaceInvaderSprites_14x14.tga");

            //-----------------------------------------------
            //images

            //load images from texture sheets above. input = coordinates on tga sheets
            //SquidOpen

            ImageManager.Add(Image.Name.SquidOpen, Texture.Name.GameSprites, imageSubRect.x, imageSubRect.y, imageSubRect.width, imageSubRect.height);

            //SquidClosed
            imageSubRect.Set(548.0f, 170.0f, 248.0f, 135.0f);
            ImageManager.Add(Image.Name.SquidClosed, Texture.Name.GameSprites, imageSubRect.x, imageSubRect.y, imageSubRect.width, imageSubRect.height);


            //-----------------------------------------------
            //Create sprite batch
            SpriteBatch pSB_Aliens = SpriteBatchManager.Add(SpriteBatch.Name.GameSprites);
            SpriteBatch pSB_Boxes = SpriteBatchManager.Add(SpriteBatch.Name.SpriteBoxes);



            //-----------------------------------------------
            //Create sprites/boxes

            //render dimensions/coordinates



            //this rect dictates where to render the sprites in the game window
            //Azul.Rect position_size = new Azul.Rect(screenX, screenY, spriteWidth, spriteHeight);

            //----------------------
            //alien sprite

            //squid game sprite
            GameSpriteManager.Add(GameSprite.Name.Squid, Image.Name.SquidOpen, screenX, screenY, spriteWidth, spriteHeight);
            //GameSpriteManager.Add(GameSprite.Name.Squid, Image.Name.SquidOpen, position_size);

            //Attach to Sprite Batch
            pSB_Aliens.Attach(GameSprite.Name.Squid);


            //----------------------
            //alien box
           
            BoxSpriteManager.Add(BoxSprite.Name.AlienBox, box_pos_size);

            //Attach to Sprite Batch
            pSB_Boxes.Attach(BoxSprite.Name.AlienBox);


            //-----------------------------------------------
            //Animated Sprite

            // Create an animation sprite
            AnimationSprite pAnimSprite = new AnimationSprite(GameSprite.Name.Squid);
            //attach to death manager for garbage collection override
            DeathManager.Attach(pAnimSprite);


            // attach alternating images to animation cycle
            pAnimSprite.Attach(Image.Name.SquidOpen);
            pAnimSprite.Attach(Image.Name.SquidClosed);

            // add AnimationSprite to timer
            TimerEventManager.Add(TimerEvent.Name.SpriteAnimation, pAnimSprite, 1.0f);




            // create 10 proxies of the squid sprite
            //note that squid and the timer animation are independent
            for (int i = 0; i < 10; i++)
            {
                ProxySprite pProxy = ProxySpriteManager.Add(GameSprite.Name.Squid);
                pProxy.x = 50.0f + 40.0f * i;
                pProxy.y = 700.0f;
                pSB_Aliens.Attach(pProxy);
            }









            Debug.WriteLine("\n\nLoad Content Complete\n----------------------------------\n");
                       
            //-----------------------------------------------
            //Data Dump;

            //TextureManager.Dump();
            //ImageManager.Dump();
            //SpriteManager.Dump();
            //SpriteBoxManager.Dump();
            //SpriteBatchManager.Dump();
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
            //sprites/spriteboxes
            GameSprite pSquid = GameSpriteManager.Find(GameSprite.Name.Squid);
            Debug.Assert(pSquid != null);

            pSquid.Update();

            //sprite boxes
            BoxSprite pAlienBox = BoxSpriteManager.Find(BoxSprite.Name.AlienBox);
            pAlienBox.Update();

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
            SpriteBatchManager.renderBoxes = false;

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

