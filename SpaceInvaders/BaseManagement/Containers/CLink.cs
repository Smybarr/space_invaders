using System;
using System.Diagnostics;


namespace SpaceInvaders
{
    //CLink = Circular Link(?) or Container Link?
    public abstract class CLink
    {
        public CLink pCNext;
        public CLink pCPrev;

        protected CLink()
        {
            this.ClearNodeLinks();
        }

        public void ClearNodeLinks()
        {
            this.pCNext = null;
            this.pCPrev = null;
        }

        public static void AddToFront(ref CLink pHead, CLink newNode)
        {
            // Will work for Active or Reserve List

            // add to front
            Debug.Assert(newNode != null);

            // add node
            if (pHead == null)
            {
                newNode.pCNext = null;
                newNode.pCPrev = null;

                // push to the front
                pHead = newNode;
            }
            else
            {
                // push to front
                //fix the links
                newNode.pCPrev = null;
                newNode.pCNext = pHead;

                pHead.pCPrev = newNode;

                //set head reference as newNode;
                pHead = newNode;
            }

            Debug.Assert(pHead != null);
        }

        public static CLink PullFromFront(ref CLink pHead)
        {
            // There should always be something on list
            Debug.Assert(pHead != null);

            // return node
            CLink pNode = pHead;

            // Update head (OK if it points to NULL)
            pHead = pHead.pCNext;
            if (pHead != null)
            {
                pHead.pCPrev = null;
            }

            // remove any lingering links
            pNode.ClearNodeLinks();

            return pNode;
        }

        public static void RemoveNode(ref CLink pHead, CLink targetNode)
        {
            // protection
            Debug.Assert(targetNode != null);

            // 4 different conditions... 
            if (targetNode.pCPrev != null)
            {
                // middle or last node
                targetNode.pCPrev.pCNext = targetNode.pCNext;
            }
            else
            {
                // first
                pHead = targetNode.pCNext;
            }

            if (targetNode.pCNext != null)
            {
                // middle node
                targetNode.pCNext.pCPrev = targetNode.pCPrev;

            }

        }

    }
}