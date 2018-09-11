using System;
using System.Diagnostics;

namespace SpaceInvaders.DestructorManagement
{
    public class DeathNode : MLink
    {
        /* All nodes inheriting from DLink should contain AT
         * LEAST the following components:
         *      - enum Name (to identify subtype of node - Name.ShipSprite for example)
         *      - constructor(s)
         *      - Setter (to set unique fields of the node)
         *      - Wash() (to clear the data fields of the node for recycling)
         *      - Dump/Print methods for debugging
         */

        // Data: --------------------------
        public object pObject;


        //ADD UNIQUE NODE DATA FIELDS HERE---------------------

        //---------------------



        //EDIT THE FOLLOWING METHODS---------------------
        public DeathNode()
            : base()
        {
            this.pObject = null;

        }
        ~DeathNode()
        {
#if(TRACK_DESTRUCTOR)
            Debug.WriteLine("~DeathNode():{0}", this.GetHashCode());
#endif
            this.pObject = null;
        }
        public void Set(object pObj)
        {
            Debug.Assert(pObj != null);
            this.pObject = pObj;
        }
        public void WashNodeData()
        {
            //wash name and data;
            this.pObject = null;
        }

        public void DumpNodeData()
        {
            Debug.WriteLine("\t\tDeathNode: {0} ", this.GetHashCode());
        }

    }



    public class DeathManager : Manager
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

        private static DeathNode pDeathNodeRef = new DeathNode();
        //singleton reference ensures only one manager is created;
        private static DeathManager pInstance = null;

        //----------------------------------------------------------------------
        // Constructor - Singleton Instantiation
        //----------------------------------------------------------------------
        private DeathManager(int startReserveSize = 3, int refillSize = 1)
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
                pInstance = new DeathManager(startReserveSize, refillSize);
            }
        }
        private static DeathManager privGetInstance()
        {
            // Safety - this forces users to call Create() first before using class
            Debug.Assert(pInstance != null);

            return pInstance;
        }

        ~DeathManager()
        {
#if(TRACK_DESTRUCTOR)
            Debug.WriteLine("~DeathMan():{0}", this.GetHashCode());
#endif
            DeathManager.pDeathNodeRef = null;
            DeathManager.pInstance = null;

        }
        public static void Destroy()
        {
            // Get the instance
            DeathManager pMan = DeathManager.privGetInstance();
            Debug.WriteLine("--->DeathMan.Destroy()");
            pMan.baseDestroy();

#if(TRACK_DESTRUCTOR)
            Debug.WriteLine("     {0} ({1})", DeathMan.pDeathNodeRef, DeathMan.pDeathNodeRef.GetHashCode());
            Debug.WriteLine("     {0} ({1})", DeathMan.pInstance, DeathMan.pInstance.GetHashCode());
#endif
            DeathManager.pDeathNodeRef = null;
            DeathManager.pInstance = null;
        }

        //----------------------------------------------------------------------
        // Unique Private helper methods
        //----------------------------------------------------------------------


        public static DeathNode Attach(object pObj)
        {
            DeathManager pMan = DeathManager.privGetInstance();

            DeathNode pNode = (DeathNode)pMan.baseAddToFront();
            Debug.Assert(pNode != null);

            Debug.Assert(pObj != null);
            pNode.Set(pObj);
            return pNode;
        }

        //----------------------------------------------------------------------
        // 4 Wrapper methods: baseAdd, baseFind, baseRemove, baseDump
        //----------------------------------------------------------------------

        public static void Dump()
        {
            DeathManager pMan = privGetInstance();
            Debug.Assert(pMan != null);

            Debug.WriteLine("------ DeathNode Manager ------");
            pMan.baseDumpAll();
        }

        //----------------------------------------------------------------------
        // Override Abstract methods
        //----------------------------------------------------------------------

        //EDIT THE FOLLOWING METHODS---------------------
        protected override void derivedDestroyNode(MLink pLink)
        {
            DeathNode pNode = (DeathNode)pLink;

#if (TRACK_DESTRUCTOR)
           Debug.WriteLine("     {0} ({1})", pNode.pObject, pNode.pObject.GetHashCode());
#endif
            pNode.pObject = null;
        }
        protected override Boolean derivedCompareNodes(MLink pLinkA, MLink pLinkB)
        {
            // This is used in baseFindNode() 
            Debug.Assert(pLinkA != null);
            Debug.Assert(pLinkB != null);

            DeathNode pDataA = (DeathNode) pLinkA;
            DeathNode pDataB = (DeathNode) pLinkB;

            Boolean status = false;

            if (pDataA.pObject == pDataB.pObject)
            {
                status = true;
            }

            return status;
        }

        protected override MLink derivedCreateNode()
        {
            MLink pNode = new DeathNode();
            Debug.Assert(pNode != null);

            return pNode;
        }
        //---------------------



        protected override void derivedDumpNode(MLink pLink)
        {
            Debug.Assert(pLink != null);
            DeathNode pNode = (DeathNode) pLink;
            pNode.DumpNodeData();
        }

        protected override void derivedWashNode(MLink pLink)
        {
            Debug.Assert(pLink != null);
            DeathNode pNode = (DeathNode) pLink;
            pNode.WashNodeData();
        }
    }
}