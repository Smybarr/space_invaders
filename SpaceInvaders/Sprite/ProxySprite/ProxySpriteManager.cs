using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public class ProxySpriteManager : Manager
    {
        //----------------------------------------------------------------------
        // Data
        private static ProxySprite pSpriteRef = new ProxySprite();
        private static ProxySpriteManager pInstance = null;

        //----------------------------------------------------------------------
        // Constructor
        //----------------------------------------------------------------------
        private ProxySpriteManager(int startReserveSize = 3, int refillSize = 1)
            : base(startReserveSize, refillSize)
        {
        }
        private static ProxySpriteManager privGetInstance()
        {
            // Safety - this forces users to call Create() first before using class
            Debug.Assert(pInstance != null);

            return pInstance;
        }
        public static void Create(int startReserveSize = 3, int refillSize = 1)
        {
            // make sure values are ressonable 
            Debug.Assert(startReserveSize > 0);
            Debug.Assert(refillSize > 0);

            // initialize the singleton here
            Debug.Assert(pInstance == null);

            // Do the initialization
            if (pInstance == null)
            {
                pInstance = new ProxySpriteManager(startReserveSize, refillSize);

                // Add a NULL Sprite into the Manager, allows find to work without breaking;
                ProxySprite pPSprite = ProxySpriteManager.Add(GameSprite.Name.NullObject);
                Debug.Assert(pPSprite != null);
            }

            Debug.WriteLine("------ProxySprite Manager Initialized-------");
        }


        ~ProxySpriteManager()
        {
            #if (TRACK_DESTRUCTOR)
            Debug.WriteLine("~ProxySpriteManager():{0} ", this.GetHashCode());
            #endif
            ProxySpriteManager.pSpriteRef = null;
            ProxySpriteManager.pInstance = null;
        }
        public static void Destroy()
        {
            // Get the instance
            ProxySpriteManager pMan = ProxySpriteManager.privGetInstance();
            Debug.WriteLine("--->ProxySpriteManager.Destroy()");
            pMan.baseDestroy();

            #if (TRACK_DESTRUCTOR)
            Debug.WriteLine("     {0} ({1})", ProxySpriteManager.pSpriteRef, ProxySpriteManager.pSpriteRef.GetHashCode());
            Debug.WriteLine("     {0} ({1})", ProxySpriteManager.pInstance, ProxySpriteManager.pInstance.GetHashCode());
            #endif
            ProxySpriteManager.pSpriteRef = null;
            ProxySpriteManager.pInstance = null;
        }

        //----------------------------------------------------------------------
        // Methods
        //----------------------------------------------------------------------

        public static ProxySprite Add(GameSprite.Name name)
        {
            ProxySpriteManager pMan = ProxySpriteManager.privGetInstance();
            Debug.Assert(pMan != null);
            //todo look into edge case for null game sprite name - create a null game sprite to add?
            ProxySprite pNode = (ProxySprite)pMan.baseAddToFront();
            Debug.Assert(pNode != null);

            pNode.Set(name);

            return pNode;
        }
        public static void Remove(ProxySprite pNode)
        {
            ProxySpriteManager pMan = ProxySpriteManager.privGetInstance();
            Debug.Assert(pMan != null);

            Debug.Assert(pNode != null);
            pMan.baseRemoveNode(pNode);
        }
        public static ProxySprite Find(ProxySprite.Name name)
        {
            ProxySpriteManager pMan = ProxySpriteManager.privGetInstance();
            Debug.Assert(pMan != null);
            // Compare functions only compares two Nodes

            // So:  Use a reference node
            //      fill in the needed data
            //      use in the Compare() function
            Debug.Assert(pSpriteRef != null);
            pSpriteRef.SetName(name);

            ProxySprite pData = (ProxySprite)pMan.baseFindNode(pSpriteRef);
            return pData;
        }

        public static void DumpAll()
        {
            ProxySpriteManager pMan = ProxySpriteManager.privGetInstance();
            Debug.Assert(pMan != null);

            Debug.WriteLine("------ ProxySprite Manager Dump All ------");
            pMan.baseDumpAll();
        }
        public static void DumpStats()
        {
            ProxySpriteManager pMan = ProxySpriteManager.privGetInstance();
            Debug.Assert(pMan != null);

            Debug.WriteLine("------ ProxySprite Manager Stats ------");
            pMan.baseDumpStats();
        }
        public static void DumpLists()
        {
            ProxySpriteManager pMan = ProxySpriteManager.privGetInstance();
            Debug.Assert(pMan != null);

            Debug.WriteLine("------ ProxySprite Manager Lists ------");
            pMan.baseDumpLists();
        }
        //----------------------------------------------------------------------
        // Override Abstract methods
        //----------------------------------------------------------------------
        override protected MLink derivedCreateNode()
        {
            MLink pNode = new ProxySprite();
            Debug.Assert(pNode != null);

            return pNode;
        }
        override protected Boolean derivedCompareNodes(MLink pLinkA, MLink pLinkB)
        {
            // This is used in baseFind() 
            Debug.Assert(pLinkA != null);
            Debug.Assert(pLinkB != null);

            ProxySprite pDataA = (ProxySprite)pLinkA;
            ProxySprite pDataB = (ProxySprite)pLinkB;

            Boolean status = false;

            if (pDataA.GetName() == pDataB.GetName())
            {
                status = true;
            }

            return status;
        }
        override protected void derivedWashNode(MLink pLink)
        {
            Debug.Assert(pLink != null);
            ProxySprite pNode = (ProxySprite)pLink;
            pNode.Wash();
        }
        override protected void derivedDumpNode(MLink pLink)
        {
            Debug.Assert(pLink != null);
            ProxySprite pData = (ProxySprite)pLink;
            pData.DumpNodeData();
        }



    }
}