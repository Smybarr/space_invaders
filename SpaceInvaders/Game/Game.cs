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

            TextureManager.Create();
            ImageManager.Create();
            SpriteManager.Create();

            //-----------------------------------------------
            //textures

            //load texture sheets: images will be cut from these sheets
            TextureManager.Add(Texture.Name.SpaceInvadersMono4, "SpaceInvadersMono4.tga");
            TextureManager.Add(Texture.Name.GameSprites, "SpaceInvaderSprites_14x14.tga");

            //-----------------------------------------------
            //images

            //load images from texture sheets above. input = coordinates on tga sheets
            //SquidOpen
            Azul.Rect textCoordinates = new Azul.Rect(548.0f, 18.0f, 248.0f, 135.0f);         
            ImageManager.Add(Image.Name.SquidOpen, Texture.Name.GameSprites, textCoordinates);
            
            //SquidClosed
            //textCoordinates.Set(548.0f, 170.0f, 248.0f, 135.0f);
            //ImageManager.Add(Image.Name.SquidClosed, Texture.Name.GameSprites, textCoordinates);


            //-----------------------------------------------
            //sprites
            
            float spriteWidth = 65.0f;
            float spriteHeight = 35.0f;

            //this rect dictates where to render the sprites in the game window
            //Azul.Rect position_size = new Azul.Rect(500.0f, 900.0f, spriteWidth, spriteHeight);
            Azul.Rect position_size = new Azul.Rect(300.0f, 400.0f, spriteWidth, spriteHeight);
            //squidOpen
            SpriteManager.Add(Sprite.Name.Squid, Image.Name.SquidOpen, position_size);
            
            //squidClosed
            //SpriteManager.Add(Sprite.Name.Squid, Image.Name.SquidClosed, position_size);


            Debug.WriteLine("\n\nLoad Content Complete\n----------------------------------\n");
                       
            //-----------------------------------------------
            //Data Dump;

            //TextureManager.Dump();
            //ImageManager.Dump();
            //SpriteManager.Dump();
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

            //-----------------------------------------------
            //sprites

            //find the sprite created above and call update on it;
            //eventually SpriteManager will automatically update all active sprites
            Sprite pSquid = SpriteManager.Find(Sprite.Name.Squid);
            pSquid.Update();


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

            //find the sprite created above and call draw on it to reflect any changes
            //      to coordinate data;
            //eventually SpriteManager will automatically draw all active sprites
            Sprite pSquid = SpriteManager.Find(Sprite.Name.Squid);
            pSquid.Draw();

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

