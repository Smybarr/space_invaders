using System;
using System.Diagnostics;
using Azul;

namespace SpaceInvaders
{
    public class Image : MLink
    {
        /* All nodes inheriting from DLink should contain AT
         * LEAST the following components:
         *      - enum Name (to identify subtype of node - Name.ShipSprite for example)
         *      - constructor(s)
         *      - Setter (to set unique fields of the node)
         *      - Wash() (to clear the data fields of the node for recycling)
         *      - Dump/Print methods for debugging
         */

        public enum Name
        {
            SquidOpen,
            SquidClosed,

            CrabOpen,
            CrabClosed,

            OctopusOpen,
            OctopusClosed,

            AlienExplosionBoom,
            AlienExplosionPop,

            AlienUFO,

            Ship,

            ShipExplosionOne,
            ShipExplosionTwo,

            Wall,

            Missile,

            AlienBombZigZag_One,
            AlienBombZigZag_Two,
            AlienBombZigZag_Three,
            AlienBombZigZag_Four,

            AlienBombCross_One,
            AlienBombCross_Two,
            AlienBombCross_Three,
            AlienBombCross_Four,

            AlienBombRolling_One,
            AlienBombRolling_Two,
            AlienBombRolling_Three,



            ShieldBrick,

            ShieldBrickLeft_Top,
            ShieldBrickRight_Top,
            ShieldBrickMidLeft_Bottom,
            ShieldBrickMid_Bottom,
            ShieldBrickMidRight_Bottom,



            AlienSplashScreen,


            NullObject,
            Blank,



        }

        protected Name name;
        private Azul.Rect poRect;
        public Texture pTexture;

        public Image()
        {
            this.name = Name.Blank;
            this.pTexture = null;
            this.poRect = new Azul.Rect();

            Debug.Assert(this.poRect != null);
        }
        public Image(Name name, Texture pSrcTexture, float x, float y, float width, float height)
        {
            this.Set(name, pSrcTexture, x, y, width, height);
        }
        ~Image()
        {
            #if (TRACK_DESTRUCTOR)
            Debug.WriteLine("~Image():{0} ", this.GetHashCode());
            #endif

            this.name = Name.Blank;
            this.pTexture = null;
            this.poRect = null;
        }

        public void Set(Name imageName, Texture pSrcTexture, float x, float y, float width, float height)
        {
            Debug.Assert(pSrcTexture != null);
           
            this.name = imageName;
            this.pTexture = pSrcTexture;
            this.poRect.Set(x, y, width, height);
        }
        public void WashNodeData()
        {
            //wash name and data;
            this.name = Name.Blank;
            this.pTexture = null;
            this.poRect.Clear();
        }

        public void DumpNodeData()
        {
            // we are using HASH code as its unique identifier 
            //this.MLinkDump();

            Debug.WriteLine("Image: {0}, hashcode: ({1})", this.name, this.GetHashCode());
            if (this.pMNext == null)
            {
                Debug.WriteLine("      next: null");
            }
            else
            {
                Image pTmp = (Image) this.pMNext;
                Debug.WriteLine("      next: {0}, hashcode: ({1})", pTmp.name, pTmp.GetHashCode());
            }

            if (this.pMPrev == null)
            {
                Debug.WriteLine("      prev: null");
            }
            else
            {
                Image pTmp = (Image) this.pMPrev;
                Debug.WriteLine("      prev: {0}, hashcode: ({1})", pTmp.name, pTmp.GetHashCode());
            }

            // Print Unique Node Data:         
            DebugPrinter.PrintAzulRect(this.poRect);

            if (this.pTexture == null)
            {
                Debug.WriteLine("   Texture: null");
            }
            else
            {
                Debug.WriteLine("   Texture: {0}", this.pTexture.GetName());
            }
            
            Debug.WriteLine("");
            Debug.WriteLine("------------------------");
        }

        public Azul.Texture GetAzulTexture()
        {
            return this.pTexture.GetAzulTexture();
        }
        public Azul.Rect GetAzulRect()
        {
            Debug.Assert(this.poRect != null);
            return this.poRect;
        }

        public Image.Name GetName()
        {
            return this.name;
        }
        public void SetName(Image.Name inName)
        {
            this.name = inName;
        }
    }



    public class ImageManager : Manager
    {
        /* All NodeManagers inheriting from Manager should contain AT
         * LEAST the following components:
         *      
         *      - Constructor
         *          !!! IF SINGLETON:
         *               - private constructor
         *               - public static void Create(int resNum, int resGrow)
         *               - private getInstance()
         *               - private static NodeManager pInstance = null; (Field)
         *          ------------------------------
         *      - 4 Base Manager Function Wrappers
         *          - Add (wrap baseAdd)
         *          - Find (wrap baseFind)
         *          - Remove (wrap baseRemove)
         *          - Dump (wrap baseDump)
         *          ------------------------------
         *      - Override of Manager Abstract Methods
         *          - override protected DLink derivedCreateNewNode()
         *          - override protected Boolean derivedCompareNodes(DLink a, DLink b)
         *          - override protected void derivedWashNode(DLink pLink)
         *          - override protected void derivedDumpNode(DLink pLink)
         *         ------------------------------
         *
         * Node Manager can also have its own unique data and functions if necessary
         *
         */

        //----------------------------------------------------------------------
        // Data: unique data for this manager here
        //----------------------------------------------------------------------

        //TODO Remove New Call?
        private static Image pImageRef = new Image();
        //singleton reference ensures only one manager is created;
        private static ImageManager pInstance = null;

        //----------------------------------------------------------------------
        // Constructor - Singleton Instantiation
        //----------------------------------------------------------------------
        private ImageManager(int startReserveSize = 3, int refillSize = 1)
            : base(startReserveSize, refillSize)
        {
            /*delegate to parent manager*/
        }
        //public facing constructor for instantiation of the singleton instance
        public static void Create(int startReserveSize = 3, int refillSize = 1)
        {
            // make sure values are reasonable 
            Debug.Assert(startReserveSize > 0);
            Debug.Assert(refillSize > 0);

            // initialize the singleton here:
            Debug.Assert(pInstance == null);

            // Do the initialization
            if (pInstance == null)
            {
                //constructor can only be called here since private access
                pInstance = new ImageManager(startReserveSize, refillSize);
            }

            //Add a NullObject into the image manager as default - allows find to work correctly;
            Image pImage = Add(Image.Name.NullObject, Texture.Name.NullObject, 0.0f, 0.0f, 128.0f, 128.0f);
            Debug.Assert(pImage != null);

            Debug.WriteLine("------Image Manager Initialized-------");
        }
        private static ImageManager privGetInstance()
        {
            // Safety - this forces users to call Create() first before using class
            Debug.Assert(pInstance != null);

            return pInstance;
        }

        ~ImageManager()
        {
#if(TRACK_DESTRUCTOR)
            Debug.WriteLine("~ImageMan():{0}", this.GetHashCode());
#endif
            ImageManager.pImageRef = null;
            ImageManager.pInstance = null;
        }
        public static void Destroy()
        {
            // Get the instance
            ImageManager pMan = ImageManager.privGetInstance();
            Debug.WriteLine("--->ImageMan.Destroy()");
            pMan.baseDestroy();

#if (TRACK_DESTRUCTOR)
            Debug.WriteLine("     {0} ({1})", ImageManager.pImageRef, ImageManager.pImageRef.GetHashCode());
            Debug.WriteLine("     {0} ({1})", ImageManager.pInstance, ImageManager.pInstance.GetHashCode());
#endif
            ImageManager.pImageRef = null;
            ImageManager.pInstance = null;
        }


        //load all images that will be used in the game
        public static void LoadImages()
        {
            ImageManager imageManager = ImageManager.privGetInstance();
            Debug.Assert(imageManager != null);

            //-----------------------------------------------
            //Image Load

            //load images from texture sheets above. input = coordinates on tga sheets


            //---------------------------------

            //float Template_tlX = 0.0f;
            //float Template_tlY = 0.0f;
            //float TemplateWidth = 0.0f;
            //float TemplateHeight = 0.0f;
            float normalBrick_tlX = 10.0f;
            float normalBrick_tlY = 0.0f;

            float topLeftBrick_tlX = 0.0f;
            float topLeftBrick_tly = 0.0f;

            float topRightBrick_tlX = 60.0f;
            float topRightBrick_tlY = 0.0f;

            float bottomMidLeftBrick_tlX = 20.0f;
            float bottomMidLeftBrick_tlY = 30.0f;

            float bottomMidBrick_tlX = 30.0f;
            float bottomMidBrick_tlY = 30.0f;

            float bottomMidRightBrick_tlX = 40.0f;
            float bottomMidRightBrick_tlY = 30.0f;

            //all bricks are 10x10 segments from the Shields.tga texture;
            float brickSizeWxH = 10.0f;


            //Shield Bricks - 6 types

            //normal brick
            ImageManager.Add(Image.Name.ShieldBrick, Texture.Name.Shields, normalBrick_tlX, normalBrick_tlY, brickSizeWxH, brickSizeWxH);

            //top left
            ImageManager.Add(Image.Name.ShieldBrickLeft_Top, Texture.Name.Shields, topLeftBrick_tlX, topLeftBrick_tly, brickSizeWxH, brickSizeWxH);

            //top right
            ImageManager.Add(Image.Name.ShieldBrickRight_Top, Texture.Name.Shields, topRightBrick_tlX, topRightBrick_tlY, brickSizeWxH, brickSizeWxH);

            //bottom left
            ImageManager.Add(Image.Name.ShieldBrickMidLeft_Bottom, Texture.Name.Shields, bottomMidLeftBrick_tlX, bottomMidLeftBrick_tlY, brickSizeWxH, brickSizeWxH);

            //bottom middle
            ImageManager.Add(Image.Name.ShieldBrickMid_Bottom, Texture.Name.Shields, bottomMidBrick_tlX, bottomMidBrick_tlY, brickSizeWxH, brickSizeWxH);

            //bottom right
            ImageManager.Add(Image.Name.ShieldBrickMidRight_Bottom, Texture.Name.Shields, bottomMidRightBrick_tlX, bottomMidRightBrick_tlY, brickSizeWxH, brickSizeWxH);




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

            //float Template_tlX = 0.0f;
            //float Template_tlY = 0.0f;
            //float TemplateWidth = 0.0f;
            //float TemplateHeight = 0.0f;

            //---------------------------------

            float heroShip_tlX = 56.0f;
            float heroShip_tlY = 336.0f;
            float heroShipWidth = 182.0f;
            float heroShipHeight = 112.0f;

            float heroMissile_tlX = 420.0f;
            float heroMissile_tlY = 700.0f;
            float heroMissileWidth = 14.0f;
            float heroMissileHeight = 56.0f;


            float heroExplodeONE_tlX = 308.0f;
            float heroExplodeONEWidth = 211.0f;

            float heroExplodeTWO_tlX = 560.0f;
            float heroExplodeTWOWidth = 224.0f;

            float heroExplode_tlY = 336.0f;
            float heroExplodeHeight = 112.0f;

            //---------------------------------

            float alienUFO_tlX = 84.0f;
            float alienUFO_tlY = 504.0f;
            float alienUFOWidth = 224.0f;
            float alienUFOHeight = 98.0f;

            //---------------------------------

            float alienExplodeONE_tlX = 406.0f;
            float alienExplodeONEWidth = 112.0f;

            float alienExplodeTWO_tlX = 574.0f;
            float alienExplodeTWOWidth = 182.0f;

            float alienExplode_tlY = 490.0f;
            float alienExplodeHeight = 112.0f;

            //---------------------------------
            float AlienBombZigOne_tlX = 490.0f;
            float AlienBombZigTwo_tlX = 574.0f;
            float AlienBombZigThree_tlX = 658.0f;
            float AlienBombZigFour_tlX = 742.0f;

            float AlienBombZig_tlY = 644.0f;

            float AlienBombZigWidth = 42.0f;
            float AlienBombZigHeight = 98.0f;


            //---------------------------------
            float AlienBombCrossOne_tlX = 28.0f;
            float AlienBombCrossTwo_tlX = 112.0f;
            float AlienBombCrossThree_tlX = 196.0f;
            float AlienBombCrossFour_tlX = 280.0f;

            float AlienBombCross_tlY = 798.0f;

            float AlienBombCrossWidth = 42.0f;
            float AlienBombCrossHeight = 84.0f;


            //---------------------------------
            float AlienBombRollingOne_tlX = 378.0f;
            float AlienBombRollingTwo_tlX = 448.0f;
            float AlienBombRollingThree_tlX = 532.0f;

            float AlienBombRolling_tlY = 798.0f;

            float AlienBombRollingOneWidth = 14.0f;
            float AlienBombRollingTwoThreeWidth = 42.0f;

            float AlienBombRollingHeight = 98.0f;


            float AlienSplashScreen_tlX = 42.0f;
            float AlienSplashScreen_tlY = 644.0f;
            float AlienSplashScreenWidth = 294.0f;
            float AlienSplashScreenHeight = 112.0f;



            //---------------------------------
            //Load the images

            //----------------------

            //Squid Open
            ImageManager.Add(Image.Name.SquidOpen, Texture.Name.GameSprites, squidOpen_tlX, constAlienOpen_tlY, squid_ImageWidth, constAlien_ImageHeight);
            //Squid Closed
            ImageManager.Add(Image.Name.SquidClosed, Texture.Name.GameSprites, squidClosed_tlX, constAlienClosed_tlY, squid_ImageWidth, constAlien_ImageHeight);

            //----------------------

            //Crab Open
            ImageManager.Add(Image.Name.CrabOpen, Texture.Name.GameSprites, crabOpen_tlX, constAlienOpen_tlY, crab_ImageWidth, constAlien_ImageHeight);
            //Crab Closed
            ImageManager.Add(Image.Name.CrabClosed, Texture.Name.GameSprites, crabClosed_tlX, constAlienClosed_tlY, crab_ImageWidth, constAlien_ImageHeight);


            //----------------------

            //Octopus Open
            ImageManager.Add(Image.Name.OctopusOpen, Texture.Name.GameSprites, octoOpen_tlX, constAlienOpen_tlY, octo_ImageWidth, constAlien_ImageHeight);
            //Octopus Closed
            ImageManager.Add(Image.Name.OctopusClosed, Texture.Name.GameSprites, octoClosed_tlX, constAlienClosed_tlY, octo_ImageWidth, constAlien_ImageHeight);

            //----------------------
            //AlienBombZigZag (4 animation images)
            ImageManager.Add(Image.Name.AlienBombZigZag_One, Texture.Name.GameSprites, AlienBombZigOne_tlX, AlienBombZig_tlY, AlienBombZigWidth, AlienBombZigHeight);
            ImageManager.Add(Image.Name.AlienBombZigZag_Two, Texture.Name.GameSprites, AlienBombZigTwo_tlX, AlienBombZig_tlY, AlienBombZigWidth, AlienBombZigHeight);
            ImageManager.Add(Image.Name.AlienBombZigZag_Three, Texture.Name.GameSprites, AlienBombZigThree_tlX, AlienBombZig_tlY, AlienBombZigWidth, AlienBombZigHeight);
            ImageManager.Add(Image.Name.AlienBombZigZag_Four, Texture.Name.GameSprites, AlienBombZigFour_tlX, AlienBombZig_tlY, AlienBombZigWidth, AlienBombZigHeight);


            //AlienBombCross (4 animtation images)
            ImageManager.Add(Image.Name.AlienBombCross_One, Texture.Name.GameSprites, AlienBombCrossOne_tlX, AlienBombCross_tlY, AlienBombCrossWidth, AlienBombCrossHeight);
            ImageManager.Add(Image.Name.AlienBombCross_Two, Texture.Name.GameSprites, AlienBombCrossTwo_tlX, AlienBombCross_tlY, AlienBombCrossWidth, AlienBombCrossHeight);
            ImageManager.Add(Image.Name.AlienBombCross_Three, Texture.Name.GameSprites, AlienBombCrossThree_tlX, AlienBombCross_tlY, AlienBombCrossWidth, AlienBombCrossHeight);
            ImageManager.Add(Image.Name.AlienBombCross_Four, Texture.Name.GameSprites, AlienBombCrossFour_tlX, AlienBombCross_tlY, AlienBombCrossWidth, AlienBombCrossHeight);


            //AlienBombRolling (3 animation images)
            ImageManager.Add(Image.Name.AlienBombRolling_One, Texture.Name.GameSprites, AlienBombRollingOne_tlX,
                AlienBombRolling_tlY, AlienBombRollingOneWidth, AlienBombRollingHeight);

            ImageManager.Add(Image.Name.AlienBombRolling_Two, Texture.Name.GameSprites, AlienBombRollingTwo_tlX,
                AlienBombRolling_tlY, AlienBombRollingTwoThreeWidth, AlienBombRollingHeight);

            ImageManager.Add(Image.Name.AlienBombRolling_Three, Texture.Name.GameSprites, AlienBombRollingThree_tlX,
                AlienBombRolling_tlY, AlienBombRollingTwoThreeWidth, AlienBombRollingHeight);

            //hero ship
            ImageManager.Add(Image.Name.Ship, Texture.Name.GameSprites, heroShip_tlX, heroShip_tlY, heroShipWidth, heroShipHeight);

            //alien ufo ship
            ImageManager.Add(Image.Name.AlienUFO, Texture.Name.GameSprites, alienUFO_tlX, alienUFO_tlY,
                alienUFOWidth, alienUFOHeight);

            //Missile
            ImageManager.Add(Image.Name.Missile, Texture.Name.GameSprites, heroMissile_tlX, heroMissile_tlY, heroMissileWidth, heroMissileHeight);

            //Wall (invisible) 
            ImageManager.Add(Image.Name.Wall, Texture.Name.GameSprites, 40, 185, 20, 10);


                    





            //Alien Explosions (2)
            ImageManager.Add(Image.Name.AlienExplosionBoom, Texture.Name.GameSprites, alienExplodeONE_tlX, alienExplode_tlY,
                alienExplodeONEWidth, alienExplodeHeight);
            ImageManager.Add(Image.Name.AlienExplosionPop, Texture.Name.GameSprites, alienExplodeTWO_tlX, alienExplode_tlY,
                alienExplodeTWOWidth, alienExplodeHeight);


            //Hero Explosions (2)
            ImageManager.Add(Image.Name.ShipExplosionOne, Texture.Name.GameSprites, heroExplodeONE_tlX, heroExplode_tlY,
                heroExplodeONEWidth, heroExplodeHeight);
            ImageManager.Add(Image.Name.ShipExplosionTwo, Texture.Name.GameSprites, heroExplodeTWO_tlX, heroExplode_tlY,
                heroExplodeTWOWidth, heroExplodeHeight);


            //Alien Splash Screen
            ImageManager.Add(Image.Name.AlienSplashScreen, Texture.Name.GameSprites, AlienSplashScreen_tlX, AlienSplashScreen_tlY,
                AlienSplashScreenWidth, AlienSplashScreenHeight);



        }


        //----------------------------------------------------------------------
        // 4 Wrapper methods: baseAdd, baseFind, baseRemove, baseDump
        //----------------------------------------------------------------------

        public static Image Add(Image.Name imageName, Texture.Name textName, float x, float y, float width, float height)
        {
            ImageManager pMan = privGetInstance();
            Debug.Assert(pMan != null);

            Texture pTexture = TextureManager.Find(textName);
            Debug.Assert(pTexture != null);

            Image pNode = (Image)pMan.baseAddToFront();
            Debug.Assert(pNode != null);

            //set the data
            pNode.Set(imageName, pTexture, x, y, width, height);
            return pNode;
        }
        public static Image Find(Image.Name name)
        {
            //get the singleton
            ImageManager pMan = privGetInstance();

            Debug.Assert(pMan != null);
            // Compare functions only compares two Nodes

            // So:  Use a reference node
            //      fill in the needed data
            //      use in the Compare() function
            Debug.Assert(pImageRef != null);
            pImageRef.WashNodeData();

            //find the node by name
            pImageRef.SetName(name);

            Image pData = (Image) pMan.baseFindNode(pImageRef);

            return pData;
        }
        public static void Remove(Image pNode)
        {
            //get the singleton
            ImageManager pMan = privGetInstance();
            Debug.Assert(pMan != null);

            Debug.Assert(pNode != null);
            pMan.baseRemoveNode(pNode);
        }
        public static void DumpAll()
        {
            ImageManager pMan = privGetInstance();
            Debug.Assert(pMan != null);

            Debug.WriteLine("------ Image Manager Dump All ------");
            pMan.baseDumpAll();
        }
        public static void DumpStats()
        {
            ImageManager pMan = privGetInstance();
            Debug.Assert(pMan != null);

            Debug.WriteLine("------ Image Manager Stats ------");
            pMan.baseDumpStats();
        }
        public static void DumpLists()
        {
            ImageManager pMan = privGetInstance();
            Debug.Assert(pMan != null);

            Debug.WriteLine("------ Image Manager Lists ------");
            pMan.baseDumpLists();
        }
        //----------------------------------------------------------------------
        // Override Abstract methods
        //----------------------------------------------------------------------

        protected override Boolean derivedCompareNodes(MLink pLinkA, MLink pLinkB)
        {
            // This is used in baseFindNode() 
            Debug.Assert(pLinkA != null);
            Debug.Assert(pLinkB != null);

            Image pDataA = (Image) pLinkA;
            Image pDataB = (Image) pLinkB;

            Boolean status = false;

            if (pDataA.GetName() == pDataB.GetName())
            {
                status = true;
            }

            return status;
        }
        protected override MLink derivedCreateNode()
        {
            MLink pNode = new Image();
            Debug.Assert(pNode != null);

            return pNode;
        }
        protected override void derivedDumpNode(MLink pLink)
        {
            Debug.Assert(pLink != null);
            Image pNode = (Image) pLink;
            pNode.DumpNodeData();
        }
        protected override void derivedWashNode(MLink pLink)
        {
            Debug.Assert(pLink != null);
            Image pNode = (Image) pLink;
            pNode.WashNodeData();
        }


    }
}