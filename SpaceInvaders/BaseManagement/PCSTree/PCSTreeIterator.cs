using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public class PCSTreeIterator
    {

        private static GameObject pRoot;
        private static GameObject pCurrent;
        private static GameObject pWrongParent;

        public static void CalculateIterators(GameObject pRootNode)
        {
            // FIX Todo have this backed into PCSTree

            Debug.Assert(pRootNode != null);
            PCSTreeIterator.pRoot = pRootNode;
            PCSTreeIterator.pWrongParent = (GameObject)pRootNode.pParent;
            PCSTreeIterator.pCurrent = PCSTreeIterator.pRoot;

            GameObject pPrevGameObj = (GameObject)pRootNode;
            // Initialize the reserve pointer
            GameObject pGameObj = (GameObject)pRootNode;

            //iterate through
            while (pGameObj != null)
            {
                // fill the basis
                pPrevGameObj = pGameObj;

                // Advance
                pGameObj = PCSTreeIterator.privSecretNext();
                //set the forward path
                pPrevGameObj.pForward = pGameObj;

                //if there is no next, at end of list
                if (pGameObj != null)
                {
                    //set the reverse path
                    pGameObj.pReverse = pPrevGameObj;
                }
            }

            ////Todo check on forward or reverse iterator for Calculating Iterators in PCSTreeIterators         
            //pRootNode.pForward.pReverse = pRootNode;
            pRootNode.pReverse = pPrevGameObj;

        }

        private static GameObject privSecretNext()
        {
            PCSTreeIterator.pCurrent = privGetNext(PCSTreeIterator.pCurrent);

            return (GameObject)PCSTreeIterator.pCurrent;
        }

        private static GameObject privGetNext(GameObject node, bool UseChild = true)
        {
            GameObject tmp = null;

            Debug.Assert(node != null);

            if ((node.pChild != null) && UseChild)
            {
                tmp = (GameObject)node.pChild;
            }
            else if (node.pSibling != null)
            {
                tmp = (GameObject)node.pSibling;
            }
            else if (node.pParent != PCSTreeIterator.pRoot && node.pParent != PCSTreeIterator.pWrongParent)
            {
                // recurse here
                tmp = PCSTreeIterator.privGetNext((GameObject)node.pParent, false);
            }
            else
            {
                tmp = null;
            }
            return tmp;
        }

    }
}
