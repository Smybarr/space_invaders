using System;
using System.Diagnostics;


namespace SpaceInvaders
{
    public abstract class Container : MLink
    {
        //heads of active/reserve lists
        public CLink pActive;
        private CLink pReserve;
        //node counts
        private int mNumActive;
        private int mNumReserve;
        private int mTotalNodeCount;
        //refill by this much when reserve pool is empty (delta grow)
        private int mRefillSize;


        protected Container(int initialReserveSize = 3, int refillReserveSize = 1)
        {
            // Check now or pay later
            Debug.Assert(initialReserveSize >= 0);
            Debug.Assert(refillReserveSize > 0);

            this.mRefillSize = refillReserveSize;
            this.mNumReserve = 0;
            this.mNumActive = 0;
            this.mTotalNodeCount = 0;
            this.pActive = null;
            this.pReserve = null;

            // Preload the reserve
            this.privFillReservedPool(initialReserveSize);
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
                CLink pNode = this.derivedCreateNode();
                Debug.Assert(pNode != null);

                CLink.AddToFront(ref this.pReserve, pNode);
            }
        }

        //Unique to Container
        protected void baseSetReserve(int reserveNum, int reserveGrow)
        {
            this.mRefillSize = reserveGrow;

            if (reserveNum > this.mNumReserve)
            {
                // Preload the reserve
                this.privFillReservedPool(reserveNum - this.mNumReserve);
            }
        }

        //same as Manager
        protected CLink baseAddToFront()
        {
            // Are there any nodes on the Reserve list?
            if (this.pReserve == null)
            {
                // refill the reserve list by the DeltaGrow
                this.privFillReservedPool(this.mRefillSize);
            }

            // Always take from the reserve list
            CLink pNode = CLink.PullFromFront(ref this.pReserve);
            Debug.Assert(pNode != null);

            // Update stats
            this.mNumActive++;
            this.mNumReserve--;

            // copy to active
            CLink.AddToFront(ref this.pActive, pNode);

            // YES - here's your new one (may its reused from reserved)
            return pNode;
        }
        protected CLink baseFindNode(CLink pNodeRef)
        {
            // search the active list
            CLink pLink = this.pActive;

            // Walk through the nodes
            while (pLink != null)
            {
                if (derivedCompareNodes(pLink, pNodeRef))
                {
                    // found it
                    break;
                }
                pLink = pLink.pCNext;
            }

            return pLink;
        }
        protected void baseRemoveNode(CLink pNode)
        {
            Debug.Assert(pNode != null);

            // Don't do the work here... delegate it
            CLink.RemoveNode(ref this.pActive, pNode);

            // wash it before returning to reserve list
            this.derivedWashNode(pNode);

            // add it to the return list
            CLink.AddToFront(ref this.pReserve, pNode);

            // stats update
            this.mNumActive--;
            this.mNumReserve++;
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
            Debug.WriteLine("  Total Num Nodes: {0}", this.mTotalNodeCount);
            Debug.WriteLine("       Num Active: {0}", this.mNumActive);
            Debug.WriteLine("     Num Reserved: {0}", this.mNumReserve);
            Debug.WriteLine("       Delta Grow: {0}", this.mRefillSize);
        }
        protected void debugPrintLists()
        {
            Debug.WriteLine("");
            Debug.WriteLine("------ Active List: ---------------------------\n");

            CLink pNode = this.pActive;

            int i = 0;
            while (pNode != null)
            {
                Debug.WriteLine("{0}: -------------", i);
                this.derivedDumpNode(pNode);
                i++;
                pNode = pNode.pCNext;
            }

            Debug.WriteLine("");
            Debug.WriteLine("------ Reserve List: ---------------------------\n");

            pNode = this.pReserve;
            i = 0;
            while (pNode != null)
            {
                Debug.WriteLine("reserve index {0}:  -------------", i);
                this.derivedDumpNode(pNode);
                i++;
                pNode = pNode.pCNext;
            }
        }

        //----------------------------------------------------------------------
        // Abstract methods - derived class must implement
        //----------------------------------------------------------------------

        abstract protected Boolean derivedCompareNodes(CLink pLinkA, CLink pLinkB);
        abstract protected CLink derivedCreateNode();      
        abstract protected void derivedDumpNode(CLink pLink);
        abstract protected void derivedWashNode(CLink pLink);

    }
}
