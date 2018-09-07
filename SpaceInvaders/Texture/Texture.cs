using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public class Texture : DLink
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
            GameSprites,

            Shields,

            SpaceInvadersMono4, //30pt font;

            NullObject,
            Blank
        }

        public Name name;
        public Azul.Texture poAzulTexture;

        public Texture()
        {
            this.name = Name.Blank;
            this.poAzulTexture = null;
        }
        public void Set(Name name, string pTextureName)
        {
            this.name = name;

            Debug.Assert(pTextureName != null);

            // do the load: = default texture node - HotPink.tga?
            this.poAzulTexture = new Azul.Texture(pTextureName, Azul.Texture_Filter.NEAREST, Azul.Texture_Filter.NEAREST);
            Debug.Assert(this.poAzulTexture != null);
        }
        public void WashNodeData()
        {
            //wash name and data;
            this.name = Name.Blank;
            this.poAzulTexture = null;
        }
        public void DumpNodeData()
        {
            // we are using HASH code as its unique identifier 
            Debug.WriteLine("Texture: {0}, hashcode: ({1})", this.name, this.GetHashCode());
            if (this.pNext == null)
            {
                Debug.WriteLine("      next: null");
            }
            else
            {
                Texture pTmp = (Texture) this.pNext;
                Debug.WriteLine("      next: {0}, hashcode: ({1})", pTmp.name, pTmp.GetHashCode());
            }

            if (this.pPrev == null)
            {
                Debug.WriteLine("      prev: null");
            }
            else
            {
                Texture pTmp = (Texture) this.pPrev;
                Debug.WriteLine("      prev: {0}, hashcode: ({1})", pTmp.name, pTmp.GetHashCode());
            }

            // Print Unique Node Data:
            Debug.WriteLine("");
            Debug.WriteLine("------------------------");
        }

        public Azul.Texture GetAzulTexture()
        {
            Debug.Assert(this.poAzulTexture != null);
            return this.poAzulTexture;
        }
    }



    public class TextureManager : Manager
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

        private static Texture pNodeRef = new Texture();
        //singleton reference ensures only one manager is created;
        private static TextureManager pInstance = null;

        //----------------------------------------------------------------------
        // Constructor - Singleton Instantiation
        //----------------------------------------------------------------------
        private TextureManager(int startReserveSize = 3, int refillSize = 1)
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
                pInstance = new TextureManager(startReserveSize, refillSize);
            }

            // Add a default texture node - HotPink.tga
            // Default texture
            TextureManager.Add(Texture.Name.Blank, "HotPink.tga");


            Debug.WriteLine("------Texture Manager Initialized-------");
        }

        //----------------------------------------------------------------------
        // Unique Private helper methods
        //----------------------------------------------------------------------

        private static TextureManager privGetInstance()
        {
            // Safety - this forces users to call Create() first before using class
            Debug.Assert(pInstance != null);

            return pInstance;
        }

        //----------------------------------------------------------------------
        // 4 Wrapper methods: baseAdd, baseFind, baseRemove, baseDump
        //----------------------------------------------------------------------

        public static Texture Add(Texture.Name name, string pTextureName)
        {
            TextureManager pMan = privGetInstance();
            Debug.Assert(pMan != null);

            Texture pNode = (Texture) pMan.baseAddToFront();

            Debug.Assert(pNode != null);

            // set the data
            pNode.Set(name, pTextureName);

            return pNode;
        }
        public static Texture Find(Texture.Name name)
        {
            //get the singleton
            TextureManager pMan = privGetInstance();

            Debug.Assert(pMan != null);
            // Compare functions only compares two Nodes

            // So:  Use a reference node
            //      fill in the needed data
            //      use in the Compare() function
            Debug.Assert(pNodeRef != null);
            pNodeRef.WashNodeData();

            //find the node by name
            pNodeRef.name = name;

            Texture pData = (Texture) pMan.baseFindNode(pNodeRef);

            return pData;
        }
        public static void Remove(Texture pNode)
        {
            //get the singleton
            TextureManager pMan = privGetInstance();
            Debug.Assert(pMan != null);

            Debug.Assert(pNode != null);
            pMan.baseRemoveNode(pNode);
        }
        public static void Dump()
        {
            TextureManager pMan = privGetInstance();
            Debug.Assert(pMan != null);

            Debug.WriteLine("\n------ TextureNode Manager ------");
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

            Texture pDataA = (Texture) pLinkA;
            Texture pDataB = (Texture) pLinkB;

            Boolean status = false;

            if (pDataA.name == pDataB.name)
            {
                status = true;
            }

            return status;
        }
        protected override DLink derivedCreateNode()
        {
            DLink pNode = new Texture();
            Debug.Assert(pNode != null);

            return pNode;
        }
        protected override void derivedDumpNode(DLink pLink)
        {
            Debug.Assert(pLink != null);
            Texture pNode = (Texture) pLink;
            pNode.DumpNodeData();
        }
        protected override void derivedWashNode(DLink pLink)
        {
            Debug.Assert(pLink != null);
            Texture pNode = (Texture) pLink;
            pNode.WashNodeData();
        }
    }
}