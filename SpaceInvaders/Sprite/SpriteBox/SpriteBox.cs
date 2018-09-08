using System;
using System.Diagnostics;

namespace SpaceInvaders.SpriteBox
{
    public class SpriteBox : SpriteBase
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
            Box,

            NullObject,
            Blank,
        }


        // Static Data: ------------------------------------

        private static Azul.Rect pPrivScreenRect = new Azul.Rect(0, 0, 1, 1);
        private static Azul.Color pWhiteAzulColor = new Azul.Color(1, 1, 1);

        // Data: -------------------------------------------
        public Name name;
        public Azul.Color poLineColor;
        private Azul.SpriteBox poAzulSpriteBox;


        public SpriteBox()
        {
            this.name = Name.Blank;
            Debug.Assert(pPrivScreenRect != null);
            Debug.Assert(pWhiteAzulColor != null);

            this.poLineColor = new Azul.Color(pWhiteAzulColor);
            Debug.Assert(this.poLineColor != null);

            this.poAzulSpriteBox = new Azul.SpriteBox(pPrivScreenRect, this.poLineColor);
            Debug.Assert(this.poAzulSpriteBox != null);

            this.x = poAzulSpriteBox.x;
            this.y = poAzulSpriteBox.y;
            this.sx = poAzulSpriteBox.sx;
            this.sy = poAzulSpriteBox.sy;
            this.angle = poAzulSpriteBox.angle;

        }

        public SpriteBox(Name name)
        {
            this.name = name;
        }
        public void WashNodeData()
        {
            //wash name and data;
            this.name = Name.Blank;
        }
        public void Set(Name name)
        {
            this.name = name;
            
        }
        public void DumpNodeData()
        {
            // we are using HASH code as its unique identifier 
            Debug.WriteLine("SpriteBox: {0}, hashcode: ({1})", this.name, this.GetHashCode());
            if (this.pNext == null)
            {
                Debug.WriteLine("      next: null");
            }
            else
            {
                SpriteBox pTmp = (SpriteBox) this.pNext;
                Debug.WriteLine("      next: {0}, hashcode: ({1})", pTmp.name, pTmp.GetHashCode());
            }

            if (this.pPrev == null)
            {
                Debug.WriteLine("      prev: null");
            }
            else
            {
                SpriteBox pTmp = (SpriteBox) this.pPrev;
                Debug.WriteLine("      prev: {0}, hashcode: ({1})", pTmp.name, pTmp.GetHashCode());
            }

            // Print Unique Node Data:         
            
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
            this.poAzulSpriteBox.Render();
        }
    }



    public class SpriteBoxManager : Manager
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

        //UNIQUE NODE DATA---------------------
        //private static SpriteBoxNode pNodeRef = new SpriteBoxNode();

        //---------------------

        //singleton reference ensures only one manager is created;
        private static SpriteBoxManager pInstance = null;

        //----------------------------------------------------------------------
        // Constructor - Singleton Instantiation
        //----------------------------------------------------------------------
        private SpriteBoxManager(int startReserveSize = 3, int refillSize = 1)
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
                pInstance = new SpriteBoxManager(startReserveSize, refillSize);
            }

            // Add a default data node?

        }

        //----------------------------------------------------------------------
        // Unique Private helper methods
        //----------------------------------------------------------------------

        private static SpriteBoxManager privGetInstance()
        {
            // Safety - this forces users to call Create() first before using class
            Debug.Assert(pInstance != null);

            return pInstance;
        }

        //----------------------------------------------------------------------
        // 4 Wrapper methods: baseAdd, baseFind, baseRemove, baseDump
        //----------------------------------------------------------------------

        //EDIT THE FOLLOWING METHODS---------------------
        public static SpriteBox Add(SpriteBox.Name name)
        {
            SpriteBoxManager pMan = privGetInstance();
            Debug.Assert(pMan != null);

            SpriteBox pNode = (SpriteBox) pMan.baseAddToFront();

            Debug.Assert(pNode != null);

            // set the data

            pNode.Set(name);

            return pNode;
        }
        public static SpriteBox Find(SpriteBox.Name name)
        {
            //get the singleton
            SpriteBoxManager pMan = privGetInstance();

            Debug.Assert(pMan != null);
            // Compare functions only compares two Nodes

            // So:  Use a reference node
            //      fill in the needed data
            //      use in the Compare() function

            SpriteBox pNodeRef = new SpriteBox();
            Debug.Assert(pNodeRef != null);
            pNodeRef.WashNodeData();

            //find the node by name
            pNodeRef.name = name;

            SpriteBox pData = (SpriteBox) pMan.baseFindNode(pNodeRef);

            return pData;
        }
        public static void Remove(SpriteBox pNode)
        {
            //get the singleton
            SpriteBoxManager pMan = privGetInstance();
            Debug.Assert(pMan != null);

            Debug.Assert(pNode != null);
            pMan.baseRemoveNode(pNode);
        }
        public static void Dump()
        {
            SpriteBoxManager pMan = privGetInstance();
            Debug.Assert(pMan != null);

            Debug.WriteLine("------ SpriteBox Manager ------");
            pMan.baseDumpAll();
        }

        //----------------------------------------------------------------------
        // Override Abstract methods
        //----------------------------------------------------------------------

        protected override Boolean derivedCompareNodes(DLink pLinkA, DLink pLinkB)
        {
            // This is used in baseFindNode() 
            Debug.Assert(pLinkA != null);
            Debug.Assert(pLinkB != null);

            SpriteBox pDataA = (SpriteBox) pLinkA;
            SpriteBox pDataB = (SpriteBox) pLinkB;

            Boolean status = false;

            if (pDataA.name == pDataB.name)
            {
                status = true;
            }

            return status;
        }

        protected override DLink derivedCreateNode()
        {
            DLink pNode = new SpriteBox();
            Debug.Assert(pNode != null);

            return pNode;
        }

        protected override void derivedDumpNode(DLink pLink)
        {
            Debug.Assert(pLink != null);
            SpriteBox pNode = (SpriteBox) pLink;
            pNode.DumpNodeData();
        }

        protected override void derivedWashNode(DLink pLink)
        {
            Debug.Assert(pLink != null);
            SpriteBox pNode = (SpriteBox) pLink;
            pNode.WashNodeData();
        }
    }
}