using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public class SBNode : MLink
    {
        /* All nodes inheriting from DLink should contain AT
         * LEAST the following components:
         *      - enum Name (to identify subtype of node - Name.ShipSprite for example)
         *      - constructor(s)
         *      - Setter (to set unique fields of the node)
         *      - Wash() (to clear the data fields of the node for recycling)
         *      - Dump/Print methods for debugging
         */

        // Data: ----------------------------------------------
        public SpriteBase pSpriteBase;
        private SBNodeManager pSBNodeManager;

        public SBNode()
            : base()
        {
            this.pSpriteBase = null;
            this.pSBNodeManager = null;
        }
        ~SBNode()
        {
            #if (TRACK_DESTRUCTOR)
            Debug.WriteLine("~SBNode():{0} ", this.GetHashCode());
            #endif
            this.pSpriteBase = null;
            this.pSBNodeManager = null;
        }

        public SBNodeManager GetSBNodeMan()
        {
            Debug.Assert(this.pSBNodeManager != null);
            return this.pSBNodeManager;
        }

        public SpriteBatch GetSpriteBatch()
        {
            Debug.Assert(this.pSBNodeManager != null);
            return this.pSBNodeManager.GetSpriteBatch();
        }

        public SpriteBase GetSpriteBase()
        {
            Debug.Assert(this.pSpriteBase != null);
            return this.pSpriteBase;
        }

        /*
                public void Set(GameSprite.Name targetSpriteName, SBNodeManager _pSBNodeMan, Boolean render = true)
                {
                    // Go find it
                    this.pSpriteBase = GameSpriteManager.Find(targetSpriteName);

                    Debug.Assert(_pSBNodeMan != null);
                    this.pSBNodeManager = _pSBNodeMan;

                    Debug.Assert(this.pSpriteBase != null);

                    //set whether to render the sprite
                    this.pSpriteBase.render = render;
                }
                //todo verify that setting the render value for sprite base to default as true screws anything up
                public void Set(BoxSprite.Name targetBoxName, SBNodeManager _pSBNodeMan, Boolean render = true)
                {
                    // Go find it
                    this.pSpriteBase = BoxSpriteManager.Find(targetBoxName);

                    Debug.Assert(_pSBNodeMan != null);
                    this.pSBNodeManager = _pSBNodeMan;

                    Debug.Assert(this.pSpriteBase != null);

                    //set whether to render the box sprite
                    this.pSpriteBase.render = render;
                }
 
        public void Set(ProxySprite pNode, SBNodeManager _pSBNodeMan)
                {
                    // associate it
                    Debug.Assert(pNode != null);

                    // Should verify that (pNode) its real and active?
                    this.pSpriteBase = pNode;
                    Debug.Assert(this.pSpriteBase != null);

                    Debug.Assert(_pSBNodeMan != null);
                    this.pSBNodeManager = _pSBNodeMan;
                }
        */
        public void Set(SpriteBase pNode, SBNodeManager _pSBNodeMan)
        {
            Debug.Assert(pNode != null);
            Debug.Assert(_pSBNodeMan != null);

            this.pSpriteBase = pNode;
            this.pSBNodeManager = _pSBNodeMan;

            // Set the back pointer
            // Allows easier deletion in the future
            Debug.Assert(pSpriteBase != null);
            this.pSpriteBase.SetSBNode(this);

        }

        public void WashNodeData()
        {
            this.pSpriteBase = null;
        }

        public void Dump()
        {
            Debug.WriteLine("SBNode: ({0})", this.GetHashCode());
            this.MLinkDump();

            // Data:
            if (this.pSpriteBase != null)
            {
                Debug.WriteLine("      pSpriteBase: {0} ({1})", this.pSpriteBase.GetSpriteName(), this.pSpriteBase.GetHashCode());
            }
            else
            {
                Debug.WriteLine("      pSpriteBase: null ");
            }
        }

    }



    public class SBNodeManager : Manager
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
        private static SBNode pSBNodeRef = new SBNode();
        //name reference to parent sprite batch;
        private SpriteBatch.Name spriteBatchName;
        private SpriteBatch pSpriteBatch;

        //----------------------------------------------------------------------
        // Constructor - Singleton Instantiation
        //----------------------------------------------------------------------
        public SBNodeManager(int startReserveSize = 3, int refillSize = 1)
            : base(startReserveSize, refillSize)
        {
            this.spriteBatchName = SpriteBatch.Name.Blank;
            this.pSpriteBatch = null;

        }



        ~SBNodeManager()
        {
            #if (TRACK_DESTRUCTOR)
            Debug.WriteLine("~SBNodeMan():{0} ", this.GetHashCode());
            #endif
            SBNodeManager.pSBNodeRef = null;
            this.spriteBatchName = SpriteBatch.Name.Blank;
            this.pSpriteBatch = null;
        }
        public void Destroy()
        {
            // Get the instance
            Debug.WriteLine("      SBNodeManager.Destroy()");
            this.baseDestroy();

            #if (TRACK_DESTRUCTOR)
                        if (SBNodeMan.pSBNodeRef != null)
            {
                Debug.WriteLine("     {0} ({1})", SBNodeMan.pSBNodeRef, SBNodeMan.pSBNodeRef.GetHashCode());
            }
            #endif
            SBNodeManager.pSBNodeRef = null;

        }
        public void Wash()
        {
            this.spriteBatchName = SpriteBatch.Name.Blank;
        }


        public void Set(SpriteBatch.Name spriteBatchName, int reserveNum, int reserveGrow)
        {
            this.spriteBatchName = spriteBatchName;

            Debug.Assert(reserveNum > 0);
            Debug.Assert(reserveGrow > 0);

            this.baseSetReserve(reserveNum, reserveGrow);
        }
        public void SetSpriteBatch(SpriteBatch _pSpriteBatch)
        {
            this.pSpriteBatch = _pSpriteBatch;
        }

        public SpriteBatch GetSpriteBatch()
        {
            return this.pSpriteBatch;
        }



        public SBNode GetActive()
        {
            return (SBNode)this.baseGetActive();
        }
        //----------------------------------------------------------------------
        // Unique Private helper methods
        //----------------------------------------------------------------------
        /*
        public SBNode Attach(GameSprite.Name name)
        {
            // Go to Man, get a node from reserve, add to active, return it
            SBNode pSpriteBatchNode = (SBNode)this.baseAddToFront();
            Debug.Assert(pSpriteBatchNode != null);

            // Initialize SpriteBatchNode
            pSpriteBatchNode.Set(name, this);

            return pSpriteBatchNode;
        }
        public SBNode Attach(BoxSprite.Name name)
        {
            // Go to Man, get a node from reserve, add to active, return it
            SBNode pSpriteBatchNode = (SBNode)this.baseAddToFront();
            Debug.Assert(pSpriteBatchNode != null);

            // Initialize SpriteBatchNode
            pSpriteBatchNode.Set(name, this);

            return pSpriteBatchNode;
        }
        //public SBNode Add(ProxySprite pNode)
        //{
        //    // Go to Man, get a node from reserve, add to active, return it
        //    SBNode pSBNode = (SBNode)this.baseAddToFront();
        //    Debug.Assert(pSBNode != null);

        //    // Initialize SpriteBatchNode
        //    pSBNode.Set(pNode, this);

        //    return pSBNode;
        //}
        */
        public SBNode Add(SpriteBase pNode)
        {
            // Go to Man, get a node from reserve, add to active, return it
            SBNode pSBNode = (SBNode)this.baseAddToFront();
            Debug.Assert(pSBNode != null);

            // Initialize SpriteBatchNode
            pSBNode.Set(pNode, this);

            return pSBNode;
        }

        public void Remove(SBNode pNode)
        {
            Debug.Assert(pNode != null);
            this.baseRemoveNode(pNode);
        }

        public void DumpSBNodeData()
        {
            Debug.WriteLine("      ------ (SBNodeMan) ------");
            this.baseDumpAll();
        }
        //----------------------------------------------------------------------
        // Override Abstract methods
        //----------------------------------------------------------------------

        protected override Boolean derivedCompareNodes(MLink pLinkA, MLink pLinkB)
        {
            //this function is stubbed out....

            Boolean status = false;

            //// This is used in baseFindNode() 
            //Debug.Assert(pLinkA != null);
            //Debug.Assert(pLinkB != null);
            //
            //SpriteBatch pDataA = (SpriteBatch)pLinkA;
            //SpriteBatch pDataB = (SpriteBatch)pLinkB;
            //
            //Boolean status = false;
            //
            //if (pDataA.name == pDataB.name)
            //{
            //    status = true;
            //}

            return status;
        }
        protected override MLink derivedCreateNode()
        {
            MLink pNode = new SBNode();
            Debug.Assert(pNode != null);

            return pNode;
        }
        protected override void derivedDumpNode(MLink pLink)
        {
            Debug.Assert(pLink != null);
            SBNode pNode = (SBNode) pLink;
            pNode.Dump();
        }
        protected override void derivedWashNode(MLink pLink)
        {
            Debug.Assert(pLink != null);
            SBNode pNode = (SBNode) pLink;
            pNode.WashNodeData();
        }
    }
}


//Manager Functions Not Implemented
//public static SBNode Add(aNode.Name name, aNode.Data data)
//{
//    SBNodeManager pMan = privGetInstance();
//    Debug.Assert(pMan != null);

//    SBNodeNode pNode = (SBNodeNode) pMan.baseAddToFront();

//    Debug.Assert(pNode != null);

//    // set the data

//    pNode.Set(name, data);

//    return pNode;
//}

//public static SBNode Find(aNode.Name name)
//{
//    //get the singleton
//    SBNodeManager pMan = privGetInstance();

//    Debug.Assert(pMan != null);
//    // Compare functions only compares two Nodes

//    // So:  Use a reference node
//    //      fill in the needed data
//    //      use in the Compare() function
//    Debug.Assert(pSBNodeRef != null);
//    pSBNodeRef.WashNodeData();

//    //find the node by name
//    pSBNodeRef.name = name;

//    SBNode pData = (SBNode) pMan.baseFindNode(pSBNodeRef);

//    return pData;
//}

//public static void Dump()
//{
//SBNodeManager pMan = privGetInstance();
//Debug.Assert(pMan != null);

//Debug.WriteLine("------ SBNode Manager ------");
//pMan.baseDumpAll();
//}
