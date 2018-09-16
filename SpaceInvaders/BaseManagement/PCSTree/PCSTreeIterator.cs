using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public class PCSTreeIterator
    {

        private static GameObject root;
        private static GameObject current;

        public static void CalculateIterators(GameObject pRootNode)
        {
            // FIX Todo have this backed into PCSTree

            Debug.Assert(pRootNode != null);
            PCSTreeIterator.root = pRootNode;
            PCSTreeIterator.current = PCSTreeIterator.root;

            GameObject pPrevGameObj = (GameObject)pRootNode;
            // Initialize the reserve pointer
            GameObject pGameObj = (GameObject)pRootNode;


            while (pGameObj != null)
            {
                // fill the basis
                pPrevGameObj = pGameObj;

                // Advance
                pGameObj = PCSTreeIterator.privSecretNext();
                pPrevGameObj.pForward = pGameObj;

                if (pGameObj != null)
                {
                    pGameObj.pReverse = pPrevGameObj;
                }
            }

            pRootNode.pReverse = pPrevGameObj;

        }

        private static GameObject privSecretNext()
        {
            PCSTreeIterator.current = privGetNext(PCSTreeIterator.current);

            return (GameObject)PCSTreeIterator.current;
        }

        private static GameObject privGetNext(GameObject node, bool UseChild = true)
        {
            GameObject tmp = null;

            if ((node.pChild != null) && UseChild)
            {
                tmp = (GameObject)node.pChild;
            }
            else if (node.pSibling != null)
            {
                tmp = (GameObject)node.pSibling;
            }
            else if (node.pParent != PCSTreeIterator.root)
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
