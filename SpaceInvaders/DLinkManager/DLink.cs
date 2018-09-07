using System;
using System.Diagnostics;


namespace SpaceInvaders
{
    public abstract class DLink
    {
        public DLink pNext;
        public DLink pPrev;

        protected DLink()
        {
            this.ClearNodeLinks();
        }
        public void ClearNodeLinks()
        {
            this.pNext = null;
            this.pPrev = null;
        }

        public static void AddToFront(ref DLink pHead, DLink newNode)
        {
            Debug.Assert(newNode != null);

            //2 scenarios: empty list or head not null;
            if (pHead == null)
            {
                newNode.pNext = null;
                newNode.pPrev = null;

                pHead = newNode;
                //Debug.WriteLine("First node added to list");
            }
            else
            {
                //push to front
                //fix the links
                newNode.pPrev = null;
                newNode.pNext = pHead;

                pHead.pPrev = newNode;

                //set head reference as newNode;
                pHead = newNode;

                //Debug.WriteLine("New node added to front of list");

            }

            Debug.Assert(pHead != null);
        }
        public static DLink PullFromFront(ref DLink pHead)
        {
            // There should always be something on list
            Debug.Assert(pHead != null);

            // return node
            DLink pNode = pHead;

            // Update head (OK if it points to NULL)
            pHead = pHead.pNext;

            if (pHead != null)
            {
                pHead.pPrev = null;
            }

            // remove any lingering links/data
            pNode.ClearNodeLinks();

            //Debug.WriteLine("DLink.PullFromFront called");

            return pNode;
        }
        public static void RemoveNode(ref DLink pHead, DLink targetNode)
        {
            // protection
            Debug.Assert(targetNode != null);

            // 4 different conditions... 
            if (targetNode.pPrev != null)
            {	// middle or last node
                targetNode.pPrev.pNext = targetNode.pNext;
            }
            else
            {  // first
                pHead = targetNode.pNext;
            }

            if (targetNode.pNext != null)
            {	// middle node
                targetNode.pNext.pPrev = targetNode.pPrev;
            }

            //Debug.WriteLine("DLink.Remove Node called");

        }

    }
}