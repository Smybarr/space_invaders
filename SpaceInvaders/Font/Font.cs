using System;
using System.Xml;
using System.Diagnostics;

namespace SpaceInvaders
{
    public class FontNode : MLink
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
            TestMessage,
            TestOneOff,

            NullObject,
            Blank,
        }

        private Name name;
        public FontSprite pFontSprite;
        static private String pNullString = "null";



        //EDIT THE FOLLOWING METHODS---------------------
        public FontNode()
            : base()
        {
            this.name = Name.Blank;
            this.pFontSprite = new FontSprite();

        }

        ~FontNode()
        {
            #if (TRACK_DESTRUCTOR)
            Debug.WriteLine("~Font():{0} ", this.GetHashCode());
            #endif
            this.name = Name.Blank;
        }


        public void WashNodeData()
        {
            //wash name and data;
            this.name = Name.Blank;
            this.pFontSprite.Set(FontNode.Name.NullObject, pNullString, Glyph.Name.NullObject, 0.0f, 0.0f);
        }

        //SetData here;
        public void Set(FontNode.Name name, String pMessage, Glyph.Name glyphName, float xStart, float yStart)
        {
            Debug.Assert(pMessage != null);

            this.name = name;
            this.pFontSprite.Set(name, pMessage, glyphName, xStart, yStart);
        }

        public void SetName(Name name)
        {
            this.name = name;
        }

        public Enum GetName()
        {
            return this.name;
        }

        public void UpdateMessage(String pMessage)
        {
            Debug.Assert(pMessage != null);
            Debug.Assert(this.pFontSprite != null);
            this.pFontSprite.UpdateMessage(pMessage);
        }

        public void DumpNodeData()
        {
            // we are using HASH code as its unique identifier 
            Debug.WriteLine("FontNode: {0}, hashcode: ({1})", this.name, this.GetHashCode());
            if (this.pMNext == null)
            {
                Debug.WriteLine("      next: null");
            }
            else
            {
                FontNode pTmp = (FontNode)this.pMNext;
                Debug.WriteLine("      next: {0}, hashcode: ({1})", pTmp.name, pTmp.GetHashCode());
            }

            if (this.pMPrev == null)
            {
                Debug.WriteLine("      prev: null");
            }
            else
            {
                FontNode pTmp = (FontNode)this.pMPrev;
                Debug.WriteLine("      prev: {0}, hashcode: ({1})", pTmp.name, pTmp.GetHashCode());
            }

            // Print Unique Node Data:         
            Debug.WriteLine("   FontNode Name: {0}", this.name);

        }

    }



    public class FontManager : Manager
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

        private FontNode pRefNode;
        //singleton reference ensures only one manager is created;
        private static FontManager pInstance = null;

        //----------------------------------------------------------------------
        // Constructor - Singleton Instantiation
        //----------------------------------------------------------------------
        private FontManager(int startReserveSize = 3, int refillSize = 1)
            : base(startReserveSize, refillSize)
        {
            /*delegate to parent manager*/
            this.pRefNode = (FontNode)this.derivedCreateNode();
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
                pInstance = new FontManager(startReserveSize, refillSize);
            }
        }

        //----------------------------------------------------------------------
        // Unique Private helper methods
        //----------------------------------------------------------------------

        private static FontManager privGetInstance()
        {
            // Safety - this forces users to call Create() first before using class
            Debug.Assert(pInstance != null);

            return pInstance;
        }


        ~FontManager()
        {
            #if (TRACK_DESTRUCTOR)
            Debug.WriteLine("~FontMan():{0}", this.GetHashCode());
            #endif
            this.pRefNode = null;
            FontManager.pInstance = null;
        }

        public static void Destroy()
        {
            // Get the instance
            FontManager pMan = FontManager.privGetInstance();
            #if (TRACK_DESTRUCTOR)
            Debug.WriteLine("--->FontMan.Destroy()");
            #endif
            pMan.baseDestroy();
        }





        //----------------------------------------------------------------------
        // 4 Wrapper methods: baseAdd, baseFind, baseRemove, baseDump
        //----------------------------------------------------------------------

        public static FontNode Add(FontNode.Name name, SpriteBatch.Name SB_Name, String pMessage, Glyph.Name glyphName, float xStart, float yStart)
        {
            FontManager pMan = FontManager.privGetInstance();

            FontNode pNode = (FontNode)pMan.baseAddToFront();
            Debug.Assert(pNode != null);

            pNode.Set(name, pMessage, glyphName, xStart, yStart);

            // Add to sprite batch
            SpriteBatch pSB = SpriteBatchManager.Find(SB_Name);
            Debug.Assert(pSB != null);
            Debug.Assert(pNode.pFontSprite != null);
            pSB.Attach(pNode.pFontSprite);

            return pNode;
        }

        public static void AddXml(Glyph.Name glyphName, String assetName, Texture.Name textName)
        {
            GlyphManager.AddXml(assetName, glyphName, textName);
            // Debug.Write("\n");
        }


        public static FontNode Find(FontNode.Name name)
        {
            //get the singleton
            FontManager pMan = privGetInstance();

            // Compare functions only compares two Nodes
            pMan.pRefNode.SetName(name);

            FontNode pData = (FontNode)pMan.baseFindNode(pMan.pRefNode);
            return pData;
        }

        public static void Remove(FontNode pNode)
        {
            //get the singleton
            FontManager pMan = privGetInstance();
            Debug.Assert(pMan != null);

            Debug.Assert(pNode != null);
            pMan.baseRemoveNode(pNode);
        }

        public static void DumpAll()
        {
            FontManager pMan = privGetInstance();
            Debug.Assert(pMan != null);

            Debug.WriteLine("------ Font Manager Dump All------");
            pMan.baseDumpAll();
        }

        public static void DumpStats()
        {
            FontManager pMan = privGetInstance();
            Debug.Assert(pMan != null);

            Debug.WriteLine("------ Font Manager Dump Stats------");
            pMan.baseDumpStats();
        }

        public static void DumpLists()
        {
            FontManager pMan = privGetInstance();
            Debug.Assert(pMan != null);

            Debug.WriteLine("------ Font Manager Dump Lists------");
            pMan.baseDumpLists();
        }

        //----------------------------------------------------------------------
        // Override Abstract methods
        //----------------------------------------------------------------------

        //EDIT THE FOLLOWING METHODS---------------------
        protected override Boolean derivedCompareNodes(MLink pLinkA, MLink pLinkB)
        {
            // This is used in baseFindNode() 
            Debug.Assert(pLinkA != null);
            Debug.Assert(pLinkB != null);

            FontNode pDataA = (FontNode)pLinkA;
            FontNode pDataB = (FontNode)pLinkB;

            Boolean status = false;

            if (pDataA.GetName() == pDataB.GetName())
            {
                status = true;
            }

            return status;
        }

        protected override MLink derivedCreateNode()
        {
            MLink pNode = new FontNode();
            Debug.Assert(pNode != null);

            return pNode;
        }
        //---------------------



        protected override void derivedDumpNode(MLink pLink)
        {
            Debug.Assert(pLink != null);
            FontNode pNode = (FontNode)pLink;
            pNode.DumpNodeData();
        }

        protected override void derivedWashNode(MLink pLink)
        {
            Debug.Assert(pLink != null);
            FontNode pNode = (FontNode)pLink;
            pNode.WashNodeData();
        }

        public static void LoadFonts()
        {

            //-----------------------------------------------
            //Font, Glyph Load
            FontManager.AddXml(Glyph.Name.SpaceInvadersMono4, "SpaceInvadersMono4.xml", Texture.Name.SpaceInvadersMono4);
        }
    }
}