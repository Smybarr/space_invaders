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

            AlienExplosion,

            AlienUFO,

            Ship,

            Wall,

            Missile,

            AlienBombStraight,
            AlienBombZigZag,
            AlienBombCross,

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

        private Name name;
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

            // Add a default data node as hot pink
            Add(Image.Name.Blank, Texture.Name.Blank, 0.0f, 0.0f, 128.0f, 128.0f);

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
        public static void Dump()
        {
            ImageManager pMan = privGetInstance();
            Debug.Assert(pMan != null);

            Debug.WriteLine("------ ImageNode Manager ------");
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