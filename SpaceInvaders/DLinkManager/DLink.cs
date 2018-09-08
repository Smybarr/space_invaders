using System;
using System.Diagnostics;


namespace SpaceInvaders
{
    public abstract class MLink
    {
        public MLink pMNext;
        public MLink pMrev;

        protected MLink()
        {
            this.ClearNodeLinks();
        }
        public void ClearNodeLinks()
        {
            this.pMNext = null;
            this.pMrev = null;
        }

        public static void AddToFront(ref MLink pHead, MLink newNode)
        {
            Debug.Assert(newNode != null);

            //2 scenarios: empty list or head not null;
            if (pHead == null)
            {
                newNode.pMNext = null;
                newNode.pMrev = null;

                pHead = newNode;
                //Debug.WriteLine("First node added to list");
            }
            else
            {
                //push to front
                //fix the links
                newNode.pMrev = null;
                newNode.pMNext = pHead;

                pHead.pMrev = newNode;

                //set head reference as newNode;
                pHead = newNode;

                //Debug.WriteLine("New node added to front of list");

            }

            Debug.Assert(pHead != null);
        }
        public static MLink PullFromFront(ref MLink pHead)
        {
            // There should always be something on list
            Debug.Assert(pHead != null);

            // return node
            MLink pNode = pHead;

            // Update head (OK if it points to NULL)
            pHead = pHead.pMNext;

            if (pHead != null)
            {
                pHead.pMrev = null;
            }

            // remove any lingering links/data
            pNode.ClearNodeLinks();

            //Debug.WriteLine("DLink.PullFromFront called");

            return pNode;
        }
        public static void RemoveNode(ref MLink pHead, MLink targetNode)
        {
            // protection
            Debug.Assert(targetNode != null);

            // 4 different conditions... 
            if (targetNode.pMrev != null)
            {	// middle or last node
                targetNode.pMrev.pMNext = targetNode.pMNext;
            }
            else
            {  // first
                pHead = targetNode.pMNext;
            }

            if (targetNode.pMNext != null)
            {	// middle node
                targetNode.pMNext.pMrev = targetNode.pMrev;
            }

            //Debug.WriteLine("DLink.Remove Node called");

        }

    }
}