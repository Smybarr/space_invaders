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

        public SBNode()
            : base()
        {
            this.pSpriteBase = null;
        }

        public void Set(GameSprite.Name targetSpriteName, Boolean render = true)
        {
            // Go find it
            this.pSpriteBase = GameSpriteManager.Find(targetSpriteName);
            Debug.Assert(this.pSpriteBase != null);
            this.pSpriteBase.render = render;
        }
        public void Set(BoxSprite.Name targetBoxName)
        {
            // Go find it
            this.pSpriteBase = BoxSpriteManager.Find(targetBoxName);
            Debug.Assert(this.pSpriteBase != null);
        }
        public void WashNodeData()
        {
            this.pSpriteBase = null;
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
        private SpriteBatch.Name spriteBatchName;

        //----------------------------------------------------------------------
        // Constructor - Singleton Instantiation
        //----------------------------------------------------------------------
        public SBNodeManager(int startReserveSize = 3, int refillSize = 1)
            : base(startReserveSize, refillSize)
        {
            this.spriteBatchName = SpriteBatch.Name.Blank;


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
        public SBNode GetActive()
        {
            return (SBNode)this.baseGetActive();
        }
        //----------------------------------------------------------------------
        // Unique Private helper methods
        //----------------------------------------------------------------------

        public SBNode Attach(GameSprite.Name name)
        {
            // Go to Man, get a node from reserve, add to active, return it
            SBNode pSpriteBatchNode = (SBNode)this.baseAddToFront();
            Debug.Assert(pSpriteBatchNode != null);

            // Initialize SpriteBatchNode
            pSpriteBatchNode.Set(name);

            return pSpriteBatchNode;
        }
        public SBNode Attach(BoxSprite.Name name)
        {
            // Go to Man, get a node from reserve, add to active, return it
            SBNode pSpriteBatchNode = (SBNode)this.baseAddToFront();
            Debug.Assert(pSpriteBatchNode != null);

            // Initialize SpriteBatchNode
            pSpriteBatchNode.Set(name);

            return pSpriteBatchNode;
        }

        public void Remove(SBNode pNode)
        {
            Debug.Assert(pNode != null);
            this.baseRemoveNode(pNode);
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
            //pNode.DumpNodeData();
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
