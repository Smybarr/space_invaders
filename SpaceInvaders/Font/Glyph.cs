using System;
using System.Xml;
using System.Diagnostics;

namespace SpaceInvaders
{
    public class Glyph : MLink
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
            Consolas36pt,
            SpaceInvadersMono4,

            NullObject,
            Blank,

        }

        private Name name;
        public int key;
        private Azul.Rect pSubRect;
        private Texture pTexture;



        //EDIT THE FOLLOWING METHODS---------------------
        public Glyph()
            : base()
        {
            this.name = Name.Blank;
            this.pTexture = null;
            this.pSubRect = new Azul.Rect();
            this.key = 0;

        }

        ~Glyph()
        {
            #if (TRACK_DESTRUCTOR)
            Debug.WriteLine("~Glyph():{0} ", this.GetHashCode());
            #endif
            this.name = Name.Blank;
            this.pSubRect = null;
            this.pTexture = null;
        }


        public void WashNodeData()
        {
            //wash name and data;
            this.name = Name.Blank;
            this.pTexture = null;
            this.pSubRect.Set(0, 0, 1, 1);
            this.key = 0;
        }

        //SetData here;
        public void Set(Glyph.Name name, int key, Texture.Name textName, float x, float y, float width, float height)
        {
            Debug.Assert(this.pSubRect != null);
            this.name = name;

            this.pTexture = TextureManager.Find(textName);
            Debug.Assert(this.pTexture != null);

            this.pSubRect.Set(x, y, width, height);

            this.key = key;

        }

        public void SetName(Name name)
        {
            this.name = name;
        }

        public Enum GetName()
        {
            return this.name;
        }

        public Azul.Rect GetAzulSubRect()
        {
            Debug.Assert(this.pSubRect != null);
            return this.pSubRect;
        }

        public Azul.Texture GetAzulTexture()
        {
            Debug.Assert(this.pTexture != null);
            return this.pTexture.GetAzulTexture();
        }

        public void DumpNodeData()
        {
            // we are using HASH code as its unique identifier 
            Debug.WriteLine("GlyphNode: {0}, hashcode: ({1})", this.name, this.GetHashCode());
            if (this.pMNext == null)
            {
                Debug.WriteLine("      next: null");
            }
            else
            {
                Glyph pTmp = (Glyph) this.pMNext;
                Debug.WriteLine("      next: {0}, hashcode: ({1})", pTmp.name, pTmp.GetHashCode());
            }

            if (this.pMPrev == null)
            {
                Debug.WriteLine("      prev: null");
            }
            else
            {
                Glyph pTmp = (Glyph) this.pMPrev;
                Debug.WriteLine("      prev: {0}, hashcode: ({1})", pTmp.name, pTmp.GetHashCode());
            }

            // Print Unique Node Data:         
            Debug.WriteLine("   GlyphNode Name: {0}", this.name);
            Debug.WriteLine("\t\t\tkey: {0}", this.key);
            if (this.pTexture != null)
            {
                Debug.WriteLine("\t\t   pTexture: {0}", this.pTexture.GetName());
            }
            else
            {
                Debug.WriteLine("\t\t   pTexture: null");
            }
            Debug.WriteLine("\t\t      pRect: {0}, {1}, {2}, {3}", this.pSubRect.x, this.pSubRect.y, this.pSubRect.width, this.pSubRect.height);
        }

    }



    public class GlyphManager : Manager
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

        private Glyph pRefNode;
        //singleton reference ensures only one manager is created;
        private static GlyphManager pInstance = null;

        //----------------------------------------------------------------------
        // Constructor - Singleton Instantiation
        //----------------------------------------------------------------------
        private GlyphManager(int startReserveSize = 3, int refillSize = 1)
            : base(startReserveSize, refillSize)
        {
            /*delegate to parent manager*/
            this.pRefNode = (Glyph)this.derivedCreateNode();
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
                pInstance = new GlyphManager(startReserveSize, refillSize);
            }
        }

        //----------------------------------------------------------------------
        // Unique Private helper methods
        //----------------------------------------------------------------------

        private static GlyphManager privGetInstance()
        {
            // Safety - this forces users to call Create() first before using class
            Debug.Assert(pInstance != null);

            return pInstance;
        }


        ~GlyphManager()
        {
            #if(TRACK_DESTRUCTOR)
            Debug.WriteLine("~GlyphMan():{0}", this.GetHashCode());
            #endif
            this.pRefNode = null;
            GlyphManager.pInstance = null;
        }

        public static void Destroy()
        {
            // Get the instance
            GlyphManager pMan = GlyphManager.privGetInstance();
            #if(TRACK_DESTRUCTOR)
            Debug.WriteLine("--->GlyphMan.Destroy()");
            #endif
            pMan.baseDestroy();
        }








        //----------------------------------------------------------------------
        // 4 Wrapper methods: baseAdd, baseFind, baseRemove, baseDump
        //----------------------------------------------------------------------

        public static Glyph Add(Glyph.Name name, int key, Texture.Name textName, float x, float y, float width, float height)
        {
            GlyphManager pMan = GlyphManager.privGetInstance();

            Glyph pNode = (Glyph)pMan.baseAddToFront();
            Debug.Assert(pNode != null);

            pNode.Set(name, key, textName, x, y, width, height);
            return pNode;
        }

        public static void AddXml(String assetName, Glyph.Name glyphName, Texture.Name textName)
        {
            //GlyphMan pMan = GlyphMan.privGetInstance();


            System.Xml.XmlTextReader reader = new XmlTextReader(assetName);

            int key = -1;
            int x = -1;
            int y = -1;
            int width = -1;
            int height = -1;

            // I'm sure there is a better way to do this... but this works for now
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element: // The node is an element.
                        if (reader.GetAttribute("key") != null)
                        {
                            key = Convert.ToInt32(reader.GetAttribute("key"));
                        }
                        else if (reader.Name == "x")
                        {
                            while (reader.Read())
                            {
                                if (reader.NodeType == XmlNodeType.Text)
                                {
                                    x = Convert.ToInt32(reader.Value);
                                    break;
                                }
                            }
                        }
                        else if (reader.Name == "y")
                        {
                            while (reader.Read())
                            {
                                if (reader.NodeType == XmlNodeType.Text)
                                {
                                    y = Convert.ToInt32(reader.Value);
                                    break;
                                }
                            }
                        }
                        else if (reader.Name == "width")
                        {
                            while (reader.Read())
                            {
                                if (reader.NodeType == XmlNodeType.Text)
                                {
                                    width = Convert.ToInt32(reader.Value);
                                    break;
                                }
                            }
                        }
                        else if (reader.Name == "height")
                        {
                            while (reader.Read())
                            {
                                if (reader.NodeType == XmlNodeType.Text)
                                {
                                    height = Convert.ToInt32(reader.Value);
                                    break;
                                }
                            }
                        }
                        break;

                    case XmlNodeType.EndElement: //Display the end of the element 
                        if (reader.Name == "character")
                        {
                            // have all the data... so now create a glyph
                            // Debug.WriteLine("key:{0} x:{1} y:{2} w:{3} h:{4}", key, x, y, width, height);
                            GlyphManager.Add(glyphName, key, textName, x, y, width, height);
                        }
                        break;
                }
            }

            // Debug.Write("\n");
        }


        public static Glyph Find(Glyph.Name name, int key)
        {
            //get the singleton
            GlyphManager pMan = privGetInstance();

            // Compare functions only compares two Nodes
            pMan.pRefNode.SetName(name);
            pMan.pRefNode.key = key;

            Glyph pData = (Glyph)pMan.baseFindNode(pMan.pRefNode);
            return pData;
        }

        public static void Remove(Glyph pNode)
        {
            //get the singleton
            GlyphManager pMan = privGetInstance();
            Debug.Assert(pMan != null);

            Debug.Assert(pNode != null);
            pMan.baseRemoveNode(pNode);
        }

        public static void DumpAll()
        {
            GlyphManager pMan = privGetInstance();
            Debug.Assert(pMan != null);

            Debug.WriteLine("------ Glyph Manager Dump All------");
            pMan.baseDumpAll();
        }

        public static void DumpStats()
        {
            GlyphManager pMan = privGetInstance();
            Debug.Assert(pMan != null);

            Debug.WriteLine("------ Glyph Manager Dump Stats------");
            pMan.baseDumpStats();
        }

        public static void DumpLists()
        {
            GlyphManager pMan = privGetInstance();
            Debug.Assert(pMan != null);

            Debug.WriteLine("------ Glyph Manager Dump Lists------");
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

            Glyph pDataA = (Glyph) pLinkA;
            Glyph pDataB = (Glyph) pLinkB;

            Boolean status = false;

            if (pDataA.GetName() == pDataB.GetName())
            {
                status = true;
            }

            return status;
        }

        protected override MLink derivedCreateNode()
        {
            MLink pNode = new Glyph();
            Debug.Assert(pNode != null);

            return pNode;
        }
        //---------------------



        protected override void derivedDumpNode(MLink pLink)
        {
            Debug.Assert(pLink != null);
            Glyph pNode = (Glyph) pLink;
            pNode.DumpNodeData();
        }

        protected override void derivedWashNode(MLink pLink)
        {
            Debug.Assert(pLink != null);
            Glyph pNode = (Glyph) pLink;
            pNode.WashNodeData();
        }
    }
}