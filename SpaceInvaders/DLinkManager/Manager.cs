using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public abstract class Manager
    {
        //heads of active/reserve lists
        private DLink pActive;
        private DLink pReserve;
        //node counts
        private int mNumActive;
        private int mNumReserve;
        private int mTotalNodeCount;
        //refill by this much when reserve pool is empty (delta grow)
        private int mRefillSize;

        protected Manager(int initialReserveSize = 3, int refillReserveSize = 1)
        {
            Debug.Assert(initialReserveSize > 0);
            Debug.Assert(refillReserveSize > 0);

            //set the refill rate of reserve pool
            this.mRefillSize = refillReserveSize;

            //everything else starts as 0/null,
            //data will get fixed when pool is created below;
            this.mNumActive = 0;
            this.mNumReserve = 0;
            this.mTotalNodeCount = 0;

            this.pActive = null;
            this.pReserve = null;

            //fill the reserve pool
            //relevent stats updated in this method
            this.privFillReservedPool(initialReserveSize);

            //double check relevant reserve stats;
            Debug.Assert(this.mTotalNodeCount > 0);
            Debug.Assert(this.mNumReserve == initialReserveSize);
            Debug.Assert(this.pReserve != null);
            //check for proper linkage - headNode.pPrev = null;
            Debug.Assert(this.pReserve.pPrev == null);

        }
        private void privFillReservedPool(int count)
        {
            // doesn't make sense if its not at least 1
            Debug.Assert(count > 0);

            this.mTotalNodeCount += count;
            this.mNumReserve += count;

            // Preload the reserve
            for (int i = 0; i < count; i++)
            {
                DLink pNode = this.derivedCreateNode();
                Debug.Assert(pNode != null);

                //add each newly created node to front of reserve list
                DLink.AddToFront(ref this.pReserve, pNode);
            }
        }

        protected DLink baseAddToFront()
        {
            // Are there any nodes on the Reserve list?
            if (this.pReserve == null)
            {
                // refill the reserve list by the refill size
                this.privFillReservedPool(this.mRefillSize);
            }

            // Always take from the reserve list
            DLink pNode = DLink.PullFromFront(ref this.pReserve);
            Debug.Assert(pNode != null);

            // Update stats
            this.mNumActive++;
            this.mNumReserve--;

            // copy to active
            DLink.AddToFront(ref this.pActive, pNode);

            //Debug.WriteLine("Base Add Node called");

            // YES - here's your new one (may its reused from reserved)
            return pNode;

        }
        protected DLink baseFindNode(DLink pNodeRef)
        {
            // search the active list
            DLink pLink = this.pActive;

            // Walk through the nodes
            while (pLink != null)
            {
                if (derivedCompareNodes(pLink, pNodeRef))
                {
                    // found it
                    break;
                }
                pLink = pLink.pNext;
            }

            //Debug.WriteLine("Base Find Node called");

            return pLink;
        }
        protected void baseRemoveNode(DLink targetNode)
        {
            //make sure node exists
            Debug.Assert(targetNode != null);

            // Don't do the work here...
            // abstract/delegate it to DLink 
            DLink.RemoveNode(ref this.pActive, targetNode);

            // wash node before returning to reserve list
            this.derivedWashNode(targetNode);
            Debug.Assert(targetNode.pNext == null);
            Debug.Assert(targetNode.pPrev == null);


            // add pulled node to the reserve list
            DLink.AddToFront(ref this.pReserve, targetNode);

            // stats update
            this.mNumActive--;
            this.mNumReserve++;

            Debug.WriteLine("Base Remove called");
        }
        protected void baseDumpAll()
        {
            this.debugPrintManagerStats();
            this.debugPrintLists();
        }

        protected void debugPrintManagerStats()
        {
            Debug.WriteLine("");
            Debug.WriteLine("-------- Stats: -------------");
            Debug.WriteLine("Total Num Nodes:       {0}", this.mTotalNodeCount);
            Debug.WriteLine("Num Active:            {0}", this.mNumActive);
            Debug.WriteLine("Num Reserved:          {0}", this.mNumReserve);
            Debug.WriteLine("Refill ReserveList By: {0}", this.mRefillSize);
            Debug.WriteLine("------------------------------\n");
        }
        protected void debugPrintLists()
        {
            Debug.WriteLine("");
            Debug.WriteLine("------ Active List: ---------------------------\n");

            //print starting with active head;
            DLink pNode = this.pActive;

            int i = 0;
            while (pNode != null)
            {
                Debug.WriteLine("active index {0}: -------------", i);
                this.derivedDumpNode(pNode);
                i++;
                pNode = pNode.pNext;
            }
            Debug.WriteLine("");
            Debug.WriteLine("------ END OF ACTIVE LIST: ---------------------------\n");


            Debug.WriteLine("");
            Debug.WriteLine("------ Reserve List: ---------------------------\n");

            pNode = this.pReserve;
            i = 0;
            while (pNode != null)
            {
                Debug.WriteLine("reserve index {0}:  -------------", i);
                this.derivedDumpNode(pNode);
                i++;
                pNode = pNode.pNext;
            }

            Debug.WriteLine("");
            Debug.WriteLine("------ END OF RESERVE LIST: ---------------------------\n");
        }

        //----------------------------------------------------------------------
        // Abstract methods - derived classes must implement
        //----------------------------------------------------------------------

        abstract protected Boolean derivedCompareNodes(DLink pLinkA, DLink pLinkB);
        abstract protected DLink derivedCreateNode();
        abstract protected void derivedDumpNode(DLink pLink);
        abstract protected void derivedWashNode(DLink pLink);

    }
}