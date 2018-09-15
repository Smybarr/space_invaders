using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public class GameSprite : SpriteBase
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
            Squid,
            Crab,
            Octopus,

            AlienUFO,

            AlienExplosion,

            Ship,

            Wall,

            Missile,

            RollingAlienBomb,
            ZigZagAlienBomb,
            CrossAlienBomb,

            ShieldBrick,

            ShieldBrickLeft_Top0,
            ShieldBrickLeft_Top1,
            ShieldBrickLeft_Bottom,

            ShieldBrickRight_Top0,
            ShieldBrickRight_Top1,
            ShieldBrickRight_Bottom,


            NullObject,
            Blank
        }

        // Static Data: ------------------------------------
        private static Azul.Rect pPrivScreenRect = new Azul.Rect();
        private static Azul.Color defaultSpriteColor_White = new Azul.Color(1, 1, 1);

        // Dynamic Sprite Data: -------------------------------------------
        private Name name;

        //pulled from sprite base due to proxy pattern;
        public float x;
        public float y;
        public float sx;
        public float sy;
        public float angle;

        private Image pImage;
        private Azul.Color poColor;
        private Azul.Rect poScreenRect;
        private Azul.Sprite poAzulSprite;


        // Constructors/Destructors: -------------------------------------------
        public GameSprite()
        {
            //SPRITE COLOR!
            //set the default color to white
            this.poColor = new Azul.Color(defaultSpriteColor_White);
            Debug.Assert(this.poColor != null);

            //default field values
            this.name = Name.Blank;

            this.pImage = ImageManager.Find(Image.Name.Blank);
            Debug.Assert(this.pImage != null);

            Debug.Assert(pPrivScreenRect != null);
            Debug.Assert(defaultSpriteColor_White != null);

            this.poScreenRect = new Azul.Rect(pPrivScreenRect);
            Debug.Assert(poScreenRect != null);

            this.poAzulSprite = new Azul.Sprite(pImage.GetAzulTexture(), pImage.GetAzulRect(), pPrivScreenRect, this.poColor);
            Debug.Assert(this.poAzulSprite != null);

            this.x = poAzulSprite.x;
            this.y = poAzulSprite.y;
            this.sx = poAzulSprite.sx;
            this.sy = poAzulSprite.sy;
            this.angle = poAzulSprite.angle;

        }
        ~GameSprite()
        {
#if (TRACK_DESTRUCTOR)
            Debug.WriteLine("~GameSprite():{0} ", this.GetHashCode());
#endif
            this.name = Name.Blank;
            this.pImage = null;
            this.poColor = null;
            this.poScreenRect = null;
            this.poAzulSprite = null;

            GameSprite.pPrivScreenRect = null;
            GameSprite.defaultSpriteColor_White = null;
        }

        public void Set(Name spriteName, Image pImage, Azul.Rect pScreenRect, Azul.Color pColor)
        {
            //set the name
            this.name = spriteName;

            //set the image
            Debug.Assert(pImage != null);
            this.pImage = pImage;

            Debug.Assert(this.poScreenRect != null);
            Debug.Assert(this.poAzulSprite != null);

            //set the screen rect
            this.poScreenRect.Set(pScreenRect);

            //sprites normally take textures - here we're using the texture bound to an image object;
            this.poAzulSprite.Swap(pImage.GetAzulTexture(), pImage.GetAzulRect(), pScreenRect, this.poColor);
            Debug.Assert(this.poAzulSprite != null);

            //set a default color or a specified color input via pColor
            if (pColor == null)
            {
                //this.poAzulSprite.Swap(pImage.GetAzulTexture(), pImage.GetAzulRect(), pScreenRect, pPrivColor);
                this.poColor.Set(defaultSpriteColor_White);
            }
            else
            {
                this.poColor.Set(pColor);
            }

            //set the coordinates (aligned with the AzulSprite object)
            this.x = poAzulSprite.x;
            this.y = poAzulSprite.y;
            this.sx = poAzulSprite.sx;
            this.sy = poAzulSprite.sy;
            this.angle = poAzulSprite.angle;
        }
        public void Set(GameSprite.Name name, Image pImage, float x, float y, float width, float height)
        {
            Debug.Assert(pImage != null);
            Debug.Assert(this.poAzulSprite != null);
            Debug.Assert(this.poColor != null);
            Debug.Assert(this.poScreenRect != null);
            Debug.Assert(defaultSpriteColor_White != null);

            this.pImage = pImage;
            this.name = name;
            this.poScreenRect.Set(x, y, width, height);
            this.poColor.Set(defaultSpriteColor_White);

            this.poAzulSprite.Swap(pImage.GetAzulTexture(), pImage.GetAzulRect(), this.poScreenRect, this.poColor);

            this.x = poAzulSprite.x;
            this.y = poAzulSprite.y;
            this.sx = poAzulSprite.sx;
            this.sy = poAzulSprite.sy;
            this.angle = poAzulSprite.angle;

        }
        public GameSprite.Name GetName()
        {
            return this.name;
        }
        public void SetName(GameSprite.Name inName)
        {
            this.name = inName;
        }

        public void WashNodeData()
        {
            //wash name and data;      
            this.name = Name.Blank;

            //reset the image to default blank
            this.pImage = ImageManager.Find(Image.Name.Blank);
            Debug.Assert(this.pImage != null);

            Debug.Assert(pPrivScreenRect != null);
            Debug.Assert(defaultSpriteColor_White != null);
            Debug.Assert(this.poColor != null);
            Debug.Assert(this.poScreenRect != null);

            //reset color to white
            this.poScreenRect.Set(pPrivScreenRect);
            this.poColor.Set(defaultSpriteColor_White);



            this.poAzulSprite.Swap(pImage.GetAzulTexture(), pImage.GetAzulRect(), this.poScreenRect, this.poColor);
            Debug.Assert(this.poAzulSprite != null);

            this.x = poAzulSprite.x;
            this.y = poAzulSprite.y;
            this.sx = poAzulSprite.sx;
            this.sy = poAzulSprite.sy;
            this.angle = poAzulSprite.angle;
        }
        public void DumpNodeData()
        {
            //this.MLinkDump();

            // we are using HASH code as its unique identifier 
            Debug.WriteLine("Sprite: {0}, hashcode: ({1})", this.name, this.GetHashCode());
            if (this.pMNext == null)
            {
                Debug.WriteLine("      next: null");
            }
            else
            {
                GameSprite pTmp = (GameSprite) this.pMNext;
                Debug.WriteLine("      next: {0}, hashcode: ({1})", pTmp.name, pTmp.GetHashCode());
            }

            if (this.pMPrev == null)
            {
                Debug.WriteLine("      prev: null");
            }
            else
            {
                GameSprite pTmp = (GameSprite) this.pMPrev;
                Debug.WriteLine("      prev: {0}, hashcode: ({1})", pTmp.name, pTmp.GetHashCode());
            }

            // Print Unique Node Data:         
            Debug.WriteLine("   x:{0}  y:{1}  sx:{2}  sy:{3}  angle:{4}", this.x, this.y, this.sx, this.sy, this.angle);
            if (this.pImage == null)
            {
                Debug.WriteLine("  Image: name: null");
            }
            else
            {
                Debug.WriteLine("  Image: name:{0}", this.pImage.GetName());
            }
            Debug.WriteLine("");
            Debug.WriteLine("------------------------");
        }


        //Modify functions
        public void ChangeImage(Image pNewImage)
        {
            Debug.Assert(this.poAzulSprite != null);
            Debug.Assert(pNewImage != null);
            this.pImage = pNewImage;

            this.poAzulSprite.SwapTexture(this.pImage.GetAzulTexture());
            this.poAzulSprite.SwapTextureRect(this.pImage.GetAzulRect());
        }

        public void ChangeColor(Azul.Color _pColor)
        {
            Debug.Assert(_pColor != null);
            Debug.Assert(this.poColor != null);
            Debug.Assert(this.poAzulSprite != null);
            this.poColor.Set(_pColor);
            this.poAzulSprite.SwapColor(_pColor);
        }
        public void ChangeColor(float red, float green, float blue, float alpha = 1.0f)
        {
            Debug.Assert(this.poColor != null);
            Debug.Assert(this.poAzulSprite != null);
            this.poColor.Set(red, green, blue, alpha);
            this.poAzulSprite.SwapColor(this.poColor);
        }

        public override Enum GetSpriteName()
        {
            return this.name;
        }
        public override void Update()
        {
            this.poAzulSprite.x = this.x;
            this.poAzulSprite.y = this.y;
            this.poAzulSprite.sx = this.sx;
            this.poAzulSprite.sy = this.sy;
            this.poAzulSprite.angle = this.angle;

            this.poAzulSprite.Update();
        }
        public override void Draw()
        {
            this.poAzulSprite.Render();
        }
    }


    public class GameSpriteManager : Manager
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
        
        //TODO Remove New Call
        private static GameSprite pSpriteRef = new GameSprite();
        //singleton reference ensures only one manager is created;
        private static GameSpriteManager pInstance = null;

        //----------------------------------------------------------------------
        // Constructor - Singleton Instantiation
        //----------------------------------------------------------------------
        private GameSpriteManager(int startReserveSize = 3, int refillSize = 1)
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
                pInstance = new GameSpriteManager(startReserveSize, refillSize);
            }

            // Add a default data node
            //Add(SpriteNode.Name.Blank, SpriteNode.Data.Blank);

            Debug.WriteLine("------GameSprite Manager Initialized-------");
        }
        private static GameSpriteManager privGetInstance()
        {
            // Safety - this forces users to call Create() first before using class
            Debug.Assert(pInstance != null);

            return pInstance;
        }

        ~GameSpriteManager()
        {
#if (TRACK_DESTRUCTOR)
            Debug.WriteLine("~GameSpriteMan():{0} ", this.GetHashCode());
#endif
            GameSpriteManager.pSpriteRef = null;
            GameSpriteManager.pInstance = null;
        }
        public static void Destroy()
        {
            // Get the instance
            GameSpriteManager pMan = GameSpriteManager.privGetInstance();
            Debug.WriteLine("--->GameSpriteMan.Destroy()");
            pMan.baseDestroy();

#if (TRACK_DESTRUCTOR)
            Debug.WriteLine("     {0} ({1})", GameSpriteMan.pSpriteRef, GameSpriteMan.pSpriteRef.GetHashCode());
            Debug.WriteLine("     {0} ({1})", GameSpriteMan.pInstance, GameSpriteMan.pInstance.GetHashCode());
#endif
            GameSpriteManager.pSpriteRef = null;
            GameSpriteManager.pInstance = null;
        }

        //----------------------------------------------------------------------
        // 4 Wrapper methods: baseAdd, baseFind, baseRemove, baseDump
        //----------------------------------------------------------------------
        public static GameSprite Add(GameSprite.Name spriteName, 
            Image.Name imageName, Azul.Rect pScreenRect, Azul.Color pColor = null)
        {
            GameSpriteManager pMan = GameSpriteManager.privGetInstance();
            Debug.Assert(pMan != null);

            Image pImage = ImageManager.Find(imageName);
            Debug.Assert(pImage != null);

            GameSprite pNode = (GameSprite)pMan.baseAddToFront();
            Debug.Assert(pNode != null);

            // wash it
            pNode.Set(spriteName, pImage, pScreenRect, pColor);
            return pNode;
        }
        public static GameSprite Add(GameSprite.Name spriteName, Image.Name imageName, float x, float y, float width, float height)
        {
            GameSpriteManager pMan = GameSpriteManager.privGetInstance();
            Debug.Assert(pMan != null);

            Image pImage = ImageManager.Find(imageName);
            Debug.Assert(pImage != null);

            GameSprite pNode = (GameSprite)pMan.baseAddToFront();
            Debug.Assert(pNode != null);

            pNode.Set(spriteName, pImage, x, y, width, height);
            return pNode;
        }
        public static GameSprite Find(GameSprite.Name name)
        {
            //get the singleton
            GameSpriteManager pMan = privGetInstance();

            Debug.Assert(pMan != null);
            // Compare functions only compares two Nodes

            // So:  Use a reference node
            //      fill in the needed data
            //      use in the Compare() function

            Debug.Assert(pSpriteRef != null);
            pSpriteRef.WashNodeData();

            //find the node by name
            pSpriteRef.SetName(name);

            GameSprite pData = (GameSprite) pMan.baseFindNode(pSpriteRef);

            return pData;
        }
        public static void Remove(GameSprite pNode)
        {
            //get the singleton
            GameSpriteManager pMan = privGetInstance();
            Debug.Assert(pMan != null);

            Debug.Assert(pNode != null);
            pMan.baseRemoveNode(pNode);
        }
        public static void DumpAll()
        {
            GameSpriteManager pMan = privGetInstance();
            Debug.Assert(pMan != null);

            Debug.WriteLine("------ GameSprite Manager Dump All ------");
            pMan.baseDumpAll();
        }

        public static void DumpStats()
        {
            GameSpriteManager pMan = privGetInstance();
            Debug.Assert(pMan != null);

            Debug.WriteLine("------ GameSprite Manager Stats ------");
            pMan.baseDumpStats();
        }

        //----------------------------------------------------------------------
        // 4 Override Abstract Methods (From Base Manager)
        //----------------------------------------------------------------------
        protected override Boolean derivedCompareNodes(MLink pLinkA, MLink pLinkB)
        {
            // This is used in baseFindNode() 
            Debug.Assert(pLinkA != null);
            Debug.Assert(pLinkB != null);

            GameSprite pDataA = (GameSprite) pLinkA;
            GameSprite pDataB = (GameSprite) pLinkB;

            Boolean status = false;

            if (pDataA.GetName() == pDataB.GetName())
            {
                status = true;
            }

            return status;
        }
        protected override MLink derivedCreateNode()
        {
            MLink pNode = new GameSprite();
            Debug.Assert(pNode != null);

            return pNode;
        }
        protected override void derivedDumpNode(MLink pLink)
        {
            Debug.Assert(pLink != null);
            GameSprite pNode = (GameSprite) pLink;
            pNode.DumpNodeData();
        }
        protected override void derivedWashNode(MLink pLink)
        {
            Debug.Assert(pLink != null);
            GameSprite pNode = (GameSprite) pLink;
            pNode.WashNodeData();
        }
    }
}