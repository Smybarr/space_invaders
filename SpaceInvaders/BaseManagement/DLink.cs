using System;
using System.Diagnostics;


namespace SpaceInvaders
{
    public abstract class MLink
    {
        public MLink pMNext;
        public MLink pMPrev;

        protected MLink()
        {
            this.ClearNodeLinks();
        }
        public void ClearNodeLinks()
        {
            this.pMNext = null;
            this.pMPrev = null;
        }

        public static void AddToFront(ref MLink pHead, MLink newNode)
        {
            Debug.Assert(newNode != null);

            //2 scenarios: empty list or head not null;
            if (pHead == null)
            {
                newNode.pMNext = null;
                newNode.pMPrev = null;

                pHead = newNode;
                //Debug.WriteLine("First node added to list");
            }
            else
            {
                //push to front
                //fix the links
                newNode.pMPrev = null;
                newNode.pMNext = pHead;

                pHead.pMPrev = newNode;

                //set head reference as newNode;
                pHead = newNode;

                //Debug.WriteLine("New node added to front of list");

            }

            Debug.Assert(pHead != null);
        }
        public static void AddSorted(ref MLink pHead, MLink pNode)
        {
            // nothing on list, first node insert on list
            if (pHead == null)
            {
                pNode.pMNext = null;
                pNode.pMPrev = null;
                pHead = pNode;
            }
            else
            {  // active list, now insert it

                // insert at front of list if less or equal
                if (pHead.derivedCompareValue() > pNode.derivedCompareValue())
                {
                    pNode.pMNext = pHead;
                    pNode.pMPrev = null;
                    pHead.pMPrev = pNode;
                    pHead = pNode;
                }
                else
                {
                    /* Locate the node before the point of insertion */
                    // insert after pCurrent
                    MLink pCurrent = pHead;
                    while (pCurrent.pMNext != null)
                    {
                        if (pCurrent.pMNext.derivedCompareValue() > pNode.derivedCompareValue())
                        {
                            break;
                        }
                        pCurrent = pCurrent.pMNext;
                    }

                    pNode.pMNext = pCurrent.pMNext;
                    pNode.pMPrev = pCurrent;
                    pCurrent.pMNext = pNode;
                }

            }

        }

        //virtual functions are inherited and redefined by child classes
        //allows specific functionality unique to context of child class
        virtual protected float derivedCompareValue()
        {
            Debug.Assert(false);
            return 0.0f;
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
                pHead.pMPrev = null;
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
            if (targetNode.pMPrev != null)
            {	// middle or last node
                targetNode.pMPrev.pMNext = targetNode.pMNext;
            }
            else
            {  // first
                pHead = targetNode.pMNext;
            }

            if (targetNode.pMNext != null)
            {	// middle node
                targetNode.pMNext.pMPrev = targetNode.pMPrev;
            }

            //Debug.WriteLine("DLink.Remove Node called");

        }

    }
}