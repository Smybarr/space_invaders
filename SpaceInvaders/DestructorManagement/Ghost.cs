using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public class GhostNode : MLink
    {
        /* All nodes inheriting from DLink should contain AT
         * LEAST the following components:
         *      - enum Name (to identify subtype of node - Name.ShipSprite for example)
         *      - constructor(s)
         *      - Setter (to set unique fields of the node)
         *      - Wash() (to clear the data fields of the node for recycling)
         *      - Dump/Print methods for debugging
         */

        // Data: ------------------
        public GameObject pGameObj;

        //ADD UNIQUE NODE DATA FIELDS HERE---------------------

        //---------------------



        //EDIT THE FOLLOWING METHODS---------------------
        public GhostNode()
            : base()
        {
            this.pGameObj = null;

        }

        ~GhostNode()
        {
            #if(TRACK_DESTRUCTOR)
            Debug.WriteLine("~GameObjectNode():{0}", this.GetHashCode());
            #endif
            this.pGameObj = null;
        }

        public void Set(GameObject pGameObject)
        {
            Debug.Assert(pGameObject != null);
            this.pGameObj = pGameObject;
        }

        public void WashNodeData()
        {
            this.pGameObj = null;
        }

        public void Reset()
        {
            this.pGameObj = null;
        }


        public Enum GetName()
        {
            return this.pGameObj.GetName();
        }

        public void DumpNodeData()
        {
            Debug.Assert(this.pGameObj != null);
            Debug.WriteLine("\t\t     GameObject: {0}", this.GetHashCode());

            this.pGameObj.Dump();
        }
 
    }



    public class GhostManager : Manager
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

        private GhostNode pRefNode;
        private static GhostManager pInstance = null;

        //----------------------------------------------------------------------
        // Constructor - Singleton Instantiation
        //----------------------------------------------------------------------
        private GhostManager(int startReserveSize = 3, int refillSize = 1)
            : base(startReserveSize, refillSize)
        {
            this.pRefNode = (GhostNode)this.derivedCreateNode();
            Debug.Assert(this.pRefNode != null);

            // Used only in compare
            this.pRefNode.pGameObj = new NullGameObject();
            Debug.Assert(this.pRefNode.pGameObj != null);
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
                pInstance = new GhostManager(startReserveSize, refillSize);
            }
            //create a default node?
        }
        private static GhostManager privGetInstance()
        {
            // Safety - this forces users to call Create() first before using class
            Debug.Assert(pInstance != null);

            return pInstance;
        }


        ~GhostManager()
        {
            #if(TRACK_DESTRUCTOR)
            Debug.WriteLine("~GhostMan():{0}", this.GetHashCode());
            #endif
            this.pRefNode = null;
            GhostManager.pInstance = null;
        }
        public static void Destroy()
        {
            // Get the instance
            GhostManager pMan = GhostManager.privGetInstance();
            #if(TRACK_DESTRUCTOR)
            Debug.WriteLine("--->GhostMan.Destroy()");
            #endif
            pMan.baseDestroy();
        }


        //----------------------------------------------------------------------
        // 4 Wrapper methods: baseAdd, baseFind, baseRemove, baseDump
        //----------------------------------------------------------------------

        public static GhostNode Add(GameObject pGameObject)
        {
            GhostManager pMan = privGetInstance();
            Debug.Assert(pMan != null);

            GhostNode pNode = (GhostNode) pMan.baseAddToFront();

            Debug.Assert(pNode != null);

            // set the data
            pNode.Set(pGameObject);
            return pNode;

        }

        public static GameObject Find(GameObject.Name pGameObjectName)
        {
            //get the singleton
            GhostManager pMan = privGetInstance();

            Debug.Assert(pMan != null);
            // Compare functions only compares two Nodes

            // Compare functions only compares two Nodes
            pMan.pRefNode.pGameObj.SetName(pGameObjectName);

            GhostNode pData = (GhostNode)pMan.baseFindNode(pMan.pRefNode);
            Debug.Assert(pData != null);

            return pData.pGameObj;
        }

        public static void Remove(GameObject pGameObject)
        {
            //get the singleton
            GhostManager pMan = privGetInstance();
            Debug.Assert(pMan != null);

            // Compare functions only compares two Nodes
            pMan.pRefNode.pGameObj.SetName(pGameObject.GetName());
            GhostNode pData = (GhostNode)pMan.baseFindNode(pMan.pRefNode);

            // release the resource
            pData.pGameObj = null;
            pMan.baseRemoveNode(pData);
        }

        public static void DumpAll()
        {
            GhostManager pMan = GhostManager.privGetInstance();
            Debug.Assert(pMan != null);

            Debug.WriteLine("------ Ghost Manager Dump All ------");
            pMan.baseDumpAll();
        }

        public static void DumpStats()
        {
            GhostManager pMan = GhostManager.privGetInstance();
            Debug.Assert(pMan != null);

            Debug.WriteLine("------ Ghost Manager Stats ------");
            pMan.baseDumpStats();
        }

        public static void DumpLists()
        {
            GhostManager pMan = GhostManager.privGetInstance();
            Debug.Assert(pMan != null);

            Debug.WriteLine("------ Ghost Manager Lists ------");
            pMan.baseDumpLists();
        }


        //----------------------------------------------------------------------
        // Override Abstract methods
        //----------------------------------------------------------------------

        protected override Boolean derivedCompareNodes(MLink pLinkA, MLink pLinkB)
        {
            // This is used in baseFindNode() 
            Debug.Assert(pLinkA != null);
            Debug.Assert(pLinkB != null);

            GhostNode pDataA = (GhostNode) pLinkA;
            GhostNode pDataB = (GhostNode) pLinkB;

            Boolean status = false;

            if (pDataA.GetName() == pDataB.GetName())
            {
                status = true;
            }

            return status;
        }
        protected override MLink derivedCreateNode()
        {
            MLink pNode = new GhostNode();
            Debug.Assert(pNode != null);

            return pNode;
        }
        protected override void derivedDumpNode(MLink pLink)
        {
            Debug.Assert(pLink != null);
            GhostNode pNode = (GhostNode) pLink;
            pNode.DumpNodeData();
        }
        protected override void derivedWashNode(MLink pLink)
        {
            Debug.Assert(pLink != null);
            GhostNode pNode = (GhostNode) pLink;
            pNode.WashNodeData();
        }
    }
}