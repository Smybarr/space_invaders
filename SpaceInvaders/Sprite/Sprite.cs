using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public class Sprite : SpriteBase
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

        //DEFAULT COLOR OF SPRITE = WHITE
        private static Azul.Color defaultSpriteColor_White = new Azul.Color(1, 1, 1);

        // Dynamic Sprite Data: -------------------------------------------
        public Name name;

        public Image pImage;
        private Azul.Color poColor;
        private Azul.Sprite poAzulSprite;

        public Sprite()
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

            this.poAzulSprite = new Azul.Sprite(pImage.GetAzulTexture(), pImage.GetAzulRect(), pPrivScreenRect, this.poColor);
            Debug.Assert(this.poAzulSprite != null);

            this.x = poAzulSprite.x;
            this.y = poAzulSprite.y;
            this.sx = poAzulSprite.sx;
            this.sy = poAzulSprite.sy;
            this.angle = poAzulSprite.angle;

        }
        public void Set(Name spriteName, Image pImage, Azul.Rect pScreenRect, Azul.Color pColor)
        {
            //set the name
            this.name = spriteName;

            //set the image
            Debug.Assert(pImage != null);
            this.pImage = pImage;

            Debug.Assert(pScreenRect != null);
            Debug.Assert(this.poAzulSprite != null);

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

            //reset color to white
            this.poColor.Set(defaultSpriteColor_White);

            this.poAzulSprite.Swap(pImage.GetAzulTexture(), pImage.GetAzulRect(), pPrivScreenRect, this.poColor);
            Debug.Assert(this.poAzulSprite != null);

            this.x = poAzulSprite.x;
            this.y = poAzulSprite.y;
            this.sx = poAzulSprite.sx;
            this.sy = poAzulSprite.sy;
            this.angle = poAzulSprite.angle;
        }
        public void DumpNodeData()
        {
            // we are using HASH code as its unique identifier 
            Debug.WriteLine("Sprite: {0}, hashcode: ({1})", this.name, this.GetHashCode());
            if (this.pMNext == null)
            {
                Debug.WriteLine("      next: null");
            }
            else
            {
                Sprite pTmp = (Sprite) this.pMNext;
                Debug.WriteLine("      next: {0}, hashcode: ({1})", pTmp.name, pTmp.GetHashCode());
            }

            if (this.pMrev == null)
            {
                Debug.WriteLine("      prev: null");
            }
            else
            {
                Sprite pTmp = (Sprite) this.pMrev;
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
                Debug.WriteLine("  Image: name:{0}", this.pImage.name);
            }
            Debug.WriteLine("");
            Debug.WriteLine("------------------------");
        }

        //Unique functions
        public void SwapColor(Azul.Color _pColor)
        {
            Debug.Assert(_pColor != null);
            this.poAzulSprite.SwapColor(_pColor);
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


    public class SpriteManager : Manager
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
        
        private static Sprite pNodeRef = new Sprite();
        //singleton reference ensures only one manager is created;
        private static SpriteManager pInstance = null;

        //----------------------------------------------------------------------
        // Constructor - Singleton Instantiation
        //----------------------------------------------------------------------
        private SpriteManager(int startReserveSize = 3, int refillSize = 1)
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
                pInstance = new SpriteManager(startReserveSize, refillSize);
            }

            // Add a default data node
            //Add(SpriteNode.Name.Blank, SpriteNode.Data.Blank);

            Debug.WriteLine("------Sprite Manager Initialized-------");
        }

        //----------------------------------------------------------------------
        // Unique Private helper methods
        //----------------------------------------------------------------------
        private static SpriteManager privGetInstance()
        {
            // Safety - this forces users to call Create() first before using class
            Debug.Assert(pInstance != null);

            return pInstance;
        }

        //----------------------------------------------------------------------
        // 4 Wrapper methods: baseAdd, baseFind, baseRemove, baseDump
        //----------------------------------------------------------------------

        public static Sprite Add(Sprite.Name spriteName, 
            Image.Name imageName, Azul.Rect pScreenRect, Azul.Color pColor = null)
        {
            SpriteManager pMan = SpriteManager.privGetInstance();
            Debug.Assert(pMan != null);

            Image pImage = ImageManager.Find(imageName);
            Debug.Assert(pImage != null);

            Sprite pNode = (Sprite)pMan.baseAddToFront();
            Debug.Assert(pNode != null);

            // wash it
            pNode.Set(spriteName, pImage, pScreenRect, pColor);
            return pNode;
        }
        public static Sprite Find(Sprite.Name name)
        {
            //get the singleton
            SpriteManager pMan = privGetInstance();

            Debug.Assert(pMan != null);
            // Compare functions only compares two Nodes

            // So:  Use a reference node
            //      fill in the needed data
            //      use in the Compare() function

            Debug.Assert(pNodeRef != null);
            pNodeRef.WashNodeData();

            //find the node by name
            pNodeRef.name = name;

            Sprite pData = (Sprite) pMan.baseFindNode(pNodeRef);

            return pData;
        }
        public static void Remove(Sprite pNode)
        {
            //get the singleton
            SpriteManager pMan = privGetInstance();
            Debug.Assert(pMan != null);

            Debug.Assert(pNode != null);
            pMan.baseRemoveNode(pNode);
        }
        public static void Dump()
        {
            SpriteManager pMan = privGetInstance();
            Debug.Assert(pMan != null);

            Debug.WriteLine("------ SpriteNode Manager ------");
            pMan.baseDumpAll();
        }

        //----------------------------------------------------------------------
        // Override Abstract methods
        //----------------------------------------------------------------------

        protected override Boolean derivedCompareNodes(MLink pLinkA, MLink pLinkB)
        {
            // This is used in baseFindNode() 
            Debug.Assert(pLinkA != null);
            Debug.Assert(pLinkB != null);

            Sprite pDataA = (Sprite) pLinkA;
            Sprite pDataB = (Sprite) pLinkB;

            Boolean status = false;

            if (pDataA.name == pDataB.name)
            {
                status = true;
            }

            return status;
        }
        protected override MLink derivedCreateNode()
        {
            MLink pNode = new Sprite();
            Debug.Assert(pNode != null);

            return pNode;
        }
        protected override void derivedDumpNode(MLink pLink)
        {
            Debug.Assert(pLink != null);
            Sprite pNode = (Sprite) pLink;
            pNode.DumpNodeData();
        }
        protected override void derivedWashNode(MLink pLink)
        {
            Debug.Assert(pLink != null);
            Sprite pNode = (Sprite) pLink;
            pNode.WashNodeData();
        }
    }
}