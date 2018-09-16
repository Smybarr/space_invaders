using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public class BoxSprite : SpriteBase
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
            TestBox,

            Box,

            AlienBox,
            ColumBox,
            GridBox,

            WallBox,

            HeroShipBox,

            BombBox,
    
            NullObject,
            Blank,


        }

        // Static Data: ------------------------------------
        private static Azul.Rect pPrivScreenRect = new Azul.Rect(0, 0, 1, 1);
        private static Azul.Color defaultBoxColor_Red = new Azul.Color(1, 0, 0);

        // Data: -------------------------------------------
        private Name name;

        //pulled from sprite base due to proxy pattern;
        public float x;
        public float y;
        public float sx;
        public float sy;
        public float angle;

        private Azul.Color poLineColor;
        private Azul.Rect poScreenRect;
        private Azul.SpriteBox poAzulSpriteBox;

        public BoxSprite()
        {
            //set the name;
            this.name = Name.Blank;

            Debug.Assert(pPrivScreenRect != null);
            Debug.Assert(defaultBoxColor_Red != null);

            //initialize line color
            this.poLineColor = new Azul.Color(defaultBoxColor_Red);
            Debug.Assert(this.poLineColor != null);

            //initialize screen coordinates
            this.poScreenRect = new Azul.Rect(pPrivScreenRect);
            Debug.Assert(this.poScreenRect != null);

            //initialize azul sprite box
            this.poAzulSpriteBox = new Azul.SpriteBox(pPrivScreenRect, this.poLineColor);
            Debug.Assert(this.poAzulSpriteBox != null);

            //pull the coordinates
            this.x = poAzulSpriteBox.x;
            this.y = poAzulSpriteBox.y;
            this.sx = poAzulSpriteBox.sx;
            this.sy = poAzulSpriteBox.sy;
            this.angle = poAzulSpriteBox.angle;

        }
        ~BoxSprite()
        {
#if (TRACK_DESTRUCTOR)
            Debug.WriteLine("~BoxSprite():{0} ", this.GetHashCode());
#endif
            this.name = Name.Blank;
            this.poLineColor = null;
            this.poScreenRect = null;
            this.poAzulSpriteBox = null;

            BoxSprite.pPrivScreenRect = null;
            BoxSprite.defaultBoxColor_Red = null;
        }
        public void WashNodeData()
        {
            //wash name and data;
            this.name = Name.Blank;

            Debug.Assert(pPrivScreenRect != null);
            Debug.Assert(defaultBoxColor_Red != null);
            Debug.Assert(this.poLineColor != null);
            Debug.Assert(this.poScreenRect != null);

            //reset color and rect to default;
            this.poLineColor.Set(defaultBoxColor_Red);
            this.poScreenRect.Set(pPrivScreenRect);

            //transfer the data;
            this.poAzulSpriteBox.Swap(this.poScreenRect, this.poLineColor);
            Debug.Assert(this.poAzulSpriteBox != null);

            //pull the coordinates
            this.x = poAzulSpriteBox.x;
            this.y = poAzulSpriteBox.y;
            this.sx = poAzulSpriteBox.sx;
            this.sy = poAzulSpriteBox.sy;
            this.angle = poAzulSpriteBox.angle;
        }
        public void Set(Name boxName, Azul.Rect pScreenRect, Azul.Color pColor)
        {
            //null checks
            Debug.Assert(pScreenRect != null);
            Debug.Assert(defaultBoxColor_Red != null);
            Debug.Assert(this.poAzulSpriteBox != null);
            Debug.Assert(this.poScreenRect != null);

            //set the name
            this.name = boxName;

            Debug.Assert(pPrivScreenRect != null);

            //set either a default color or a color input;
            if (pColor == null)
            {
                //set to static red (default)
                this.poLineColor.Set(defaultBoxColor_Red);
            }
            else
            {
                this.poLineColor.Set(pColor);
            }

            this.poAzulSpriteBox.Swap(pScreenRect, this.poLineColor);
            Debug.Assert(this.poAzulSpriteBox != null);

            this.x = poAzulSpriteBox.x;
            this.y = poAzulSpriteBox.y;
            this.sx = poAzulSpriteBox.sx;
            this.sy = poAzulSpriteBox.sy;
            this.angle = poAzulSpriteBox.angle;

        }
        public void Set(Name boxName, float x, float y, float width, float height)
        {
            Debug.Assert(pPrivScreenRect != null);
            Debug.Assert(defaultBoxColor_Red != null);
            Debug.Assert(this.poAzulSpriteBox != null);
            Debug.Assert(this.poScreenRect != null);

            //set the name
            this.name = boxName;

            //set the default color and screen rect coordinates
            this.poLineColor.Set(defaultBoxColor_Red);
            this.poScreenRect.Set(x, y, width, height);

            this.poAzulSpriteBox.Swap(this.poScreenRect, this.poLineColor);
            Debug.Assert(this.poAzulSpriteBox != null);
        
            //pull the coordinates
            this.x = poAzulSpriteBox.x;
            this.y = poAzulSpriteBox.y;
            this.sx = poAzulSpriteBox.sx;
            this.sy = poAzulSpriteBox.sy;
            this.angle = poAzulSpriteBox.angle;
        }
        public BoxSprite.Name GetName()
        {
            return this.name;
        }
        public void SetName(Name inName)
        {
            this.name = inName;
        }
        public void ChangeColor(Azul.Color _pColor)
        {
            Debug.Assert(_pColor != null);
            Debug.Assert(this.poLineColor != null);
            Debug.Assert(this.poAzulSpriteBox != null);
            this.poLineColor.Set(_pColor);
            this.poAzulSpriteBox.SwapColor(this.poLineColor);
        }
        //todo clean up boxsprite line color management
        public void ChangeColor(float red, float green, float blue, float alpha = 1.0f)
        {
            Debug.Assert(this.poLineColor != null);
            Debug.Assert(this.poAzulSpriteBox != null);
            this.poLineColor.Set(red, green, blue, alpha);
            this.poAzulSpriteBox.SwapColor(this.poLineColor);
        }
        public void SetLineColor(float red, float green, float blue, float alpha = 1.0f)
        {
            this.poLineColor.Set(red, green, blue, alpha);
        }
        public void DumpNodeData()
        {
            this.MLinkDump();

            // we are using HASH code as its unique identifier 
            Debug.WriteLine("SpriteBox: {0}, hashcode: ({1})", this.name, this.GetHashCode());
            if (this.pMNext == null)
            {
                Debug.WriteLine("      next: null");
            }
            else
            {
                BoxSprite pTmp = (BoxSprite) this.pMNext;
                Debug.WriteLine("      next: {0}, hashcode: ({1})", pTmp.name, pTmp.GetHashCode());
            }

            if (this.pMPrev == null)
            {
                Debug.WriteLine("      prev: null");
            }
            else
            {
                BoxSprite pTmp = (BoxSprite) this.pMPrev;
                Debug.WriteLine("      prev: {0}, hashcode: ({1})", pTmp.name, pTmp.GetHashCode());
            }

            // Print Unique Node Data:         
            
        }


        public override Enum GetSpriteName()
        {
            return this.name;
        }
        public override void Update()
        {
            this.poAzulSpriteBox.x = this.x;
            this.poAzulSpriteBox.y = this.y;
            this.poAzulSpriteBox.sx = this.sx;
            this.poAzulSpriteBox.sy = this.sy;
            this.poAzulSpriteBox.angle = this.angle;

            this.poAzulSpriteBox.Update();
        }
        public override void Draw()
        {
            //only draw sprite boxes if render flag is true;
            if (SpriteBatchManager.renderBoxes)
            {
                this.poAzulSpriteBox.Render();
            }


        }
    }



    public class BoxSpriteManager : Manager
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

        private static BoxSprite pBoxSpriteRef = new BoxSprite();
        private static BoxSpriteManager pInstance = null;

        private BoxSpriteManager(int startReserveSize = 3, int refillSize = 1)
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
                pInstance = new BoxSpriteManager(startReserveSize, refillSize);
            }

            // Add a default data node?

            Debug.WriteLine("------BoxSprite Manager Initialized-------");

        }
        private static BoxSpriteManager privGetInstance()
        {
            // Safety - this forces users to call Create() first before using class
            Debug.Assert(pInstance != null);

            return pInstance;
        }


        ~BoxSpriteManager()
        {
#if (TRACK_DESTRUCTOR)
            Debug.WriteLine("~BoxSpriteMan():{0} ", this.GetHashCode());
#endif
            BoxSpriteManager.pBoxSpriteRef = null;
            BoxSpriteManager.pInstance = null;
        }
        public static void Destroy()
        {
            // Get the instance
            BoxSpriteManager pMan = BoxSpriteManager.privGetInstance();
            Debug.WriteLine("--->BoxSpriteMan.Destroy()");
            pMan.baseDestroy();

#if (TRACK_DESTRUCTOR)
            Debug.WriteLine("     {0} ({1})", BoxSpriteMan.pBoxSpriteRef, BoxSpriteMan.pBoxSpriteRef.GetHashCode());
            Debug.WriteLine("     {0} ({1})", BoxSpriteMan.pInstance, BoxSpriteMan.pInstance.GetHashCode());
#endif
            BoxSpriteManager.pBoxSpriteRef = null;
            BoxSpriteManager.pInstance = null;
        }


        //----------------------------------------------------------------------
        // 4 Wrapper methods: baseAdd, baseFind, baseRemove, baseDump
        //----------------------------------------------------------------------

        //EDIT THE FOLLOWING METHODS---------------------
        public static BoxSprite Add(BoxSprite.Name name, Azul.Rect pScreenRect, Azul.Color pColor = null)
        {
            BoxSpriteManager pMan = privGetInstance();
            Debug.Assert(pMan != null);

            BoxSprite pNode = (BoxSprite)pMan.baseAddToFront();
            Debug.Assert(pNode != null);

            // set the data
            pNode.Set(name, pScreenRect, pColor);

            return pNode;
        }
        public static BoxSprite Add(BoxSprite.Name spriteName, float x, float y, float width, float height)
        {
            BoxSpriteManager pMan = BoxSpriteManager.privGetInstance();
            Debug.Assert(pMan != null);

            BoxSprite pNode = (BoxSprite)pMan.baseAddToFront();
            Debug.Assert(pNode != null);

            // wash it
            pNode.Set(spriteName, x, y, width, height);
            return pNode;
        }
        public static BoxSprite Find(BoxSprite.Name name)
        {
            //get the singleton
            BoxSpriteManager pMan = privGetInstance();

            Debug.Assert(pMan != null);
            // Compare functions only compares two Nodes

            // So:  Use a reference node
            //      fill in the needed data
            //      use in the Compare() function
            Debug.Assert(pBoxSpriteRef != null);
            pBoxSpriteRef.WashNodeData();

            //find the node by name
            pBoxSpriteRef.SetName(name);

            BoxSprite pData = (BoxSprite) pMan.baseFindNode(pBoxSpriteRef);

            return pData;
        }
        public static void Remove(BoxSprite pNode)
        {
            //get the singleton
            BoxSpriteManager pMan = privGetInstance();
            Debug.Assert(pMan != null);

            Debug.Assert(pNode != null);
            pMan.baseRemoveNode(pNode);
        }
        public static void DumpAll()
        {
            BoxSpriteManager pMan = privGetInstance();
            Debug.Assert(pMan != null);

            Debug.WriteLine("------ BoxSprite Manager Dump All ------");
            pMan.baseDumpAll();
        }
        public static void DumpStats()
        {
            BoxSpriteManager pMan = privGetInstance();
            Debug.Assert(pMan != null);

            Debug.WriteLine("------ BoxSprite Manager Stats ------");
            pMan.baseDumpStats();
        }
        //----------------------------------------------------------------------
        // Override Abstract methods
        //----------------------------------------------------------------------

        protected override Boolean derivedCompareNodes(MLink pLinkA, MLink pLinkB)
        {
            // This is used in baseFindNode() 
            Debug.Assert(pLinkA != null);
            Debug.Assert(pLinkB != null);

            BoxSprite pDataA = (BoxSprite) pLinkA;
            BoxSprite pDataB = (BoxSprite) pLinkB;

            Boolean status = false;

            if (pDataA.GetName() == pDataB.GetName())
            {
                status = true;
            }

            return status;
        }
        protected override MLink derivedCreateNode()
        {
            MLink pNode = new BoxSprite();
            Debug.Assert(pNode != null);

            return pNode;
        }
        protected override void derivedDumpNode(MLink pLink)
        {
            Debug.Assert(pLink != null);
            BoxSprite pNode = (BoxSprite) pLink;
            pNode.DumpNodeData();
        }
        protected override void derivedWashNode(MLink pLink)
        {
            Debug.Assert(pLink != null);
            BoxSprite pNode = (BoxSprite) pLink;
            pNode.WashNodeData();
        }
    }
}