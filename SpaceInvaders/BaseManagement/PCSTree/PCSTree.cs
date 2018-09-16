using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public class PCSTree
    {
        public enum Name
        {
            Root,
            Not_Initialized
        }

        // Data -----------------------------------------------------

        private PCSNode pRoot;
        public int numNodes;
        public int maxNodeCount;

        // constructor
        public PCSTree()
        {
            this.pRoot = null;
            this.maxNodeCount = 0;
            this.numNodes = 0;

            // create the root
            PCSNode pcsRoot = new PCSRootNode(PCSTree.Name.Root);
            this.Insert(pcsRoot, null);
        }

        public PCSNode getRoot()
        {
            return this.pRoot;
        }

        // insert
        public void Insert(PCSNode inNode, PCSNode pParent)
        {
            Debug.Assert(inNode != null);

            // insert to root
            if (null == pParent)
            {
                this.pRoot = inNode;
                inNode.pChild = null;
                inNode.pParent = null;
                inNode.pSibling = null;

                this.privInfoAddNode();
            }
            else  // insert inside the tree
            {
                if (pParent.pChild == null)
                { // child is 0, just add child
                    pParent.pChild = inNode;

                    inNode.pParent = pParent;
                    inNode.pChild = null;
                    inNode.pSibling = null;

                    this.privInfoAddNode();
                }
                else
                { // add as sibling

                    // Get first child
                    PCSNode first = pParent.pChild;

                    inNode.pParent = pParent;
                    inNode.pChild = null;
                    inNode.pSibling = first;

                    pParent.pChild = inNode;

                    this.privInfoAddNode();
                }
            }
        }

        // remove
        public void Remove(PCSNode inNode)
        {
            Debug.Assert(inNode != null);

            if (inNode.pChild == null)
            {
                // last node
                if (inNode.pSibling == null)
                {
                    // find the previous child
                    PCSNode pParent;
                    pParent = inNode.pParent;

                    // special case root
                    if (pParent == null)
                    {
                        this.pRoot = null;
                    }
                    else
                    {   // no children, no younger siblings
                        privRemoveNodeNoYoungerSiblings(inNode, pParent);
                    }
                }
                else
                {   // No children, but has other younger siblings
                    privRemoveNodeHasYoungerSiblings(inNode);
                }

                inNode.pChild = null;
                inNode.pParent = null;
                inNode.pSibling = null;
                this.privInfoRemoveNode();
                return;
            }
            else
            {
                // If we are here, recursively call
                PCSNode pTmp = inNode.pChild;
                Debug.Assert(pTmp != null);

                this.Remove(pTmp);
                this.Remove(inNode);
            }
        }

        private void privRemoveNodeNoYoungerSiblings(PCSNode inNode, PCSNode pParent)
        {
            Debug.Assert(pParent != null);

            PCSNode pTmp;
            // goto eldest child
            pTmp = pParent.pChild;
            Debug.Assert(pTmp != null);

            if (pTmp.pSibling == null)
            {   // delete inNode so it's parent is 0
                // in child has no siblings
                pParent.pChild = null;
            }
            else
            {   // now iterate until child
                while (pTmp.pSibling != inNode)
                {
                    pTmp = pTmp.pSibling;
                }
                // this point we found the sibling
                PCSNode pPrev = pTmp;
                pPrev.pSibling = null;
            }
        }
        private void privRemoveNodeHasYoungerSiblings(PCSNode inNode)
        {
            // I have a sibling to the right of me
            // find the previous child
            PCSNode pParent;
            pParent = inNode.pParent;
            Debug.Assert(pParent != null);

            PCSNode pTmp;

            // goto eldest child
            pTmp = pParent.pChild;
            Debug.Assert(pTmp != null);

            if (pTmp == inNode)
            {   // we are deleting a eldest child with siblings
                pParent.pChild = pTmp.pSibling;
            }
            else
            {   // now iterate until child
                while (pTmp.pSibling != inNode)
                {
                    pTmp = pTmp.pSibling;
                }

                // this point we found the sibling
                PCSNode pPrev = pTmp;
                pPrev.pSibling = inNode.pSibling;
            }
        }

        public void dumpTree()
        {
            Debug.WriteLine("");
            Debug.WriteLine("dumpTree () -------------------------------");
            this.privDumpTreeDepthFirst(this.pRoot);
        }

        private void privDumpTreeDepthFirst(PCSNode pNode)
        {
            PCSNode pChild = null;

            // dump
            pNode.dumpNode();

            // iterate through all of the active children 
            if (pNode.pChild != null)
            {
                pChild = pNode.pChild;
                // make sure that allocation is not a child node 
                while (pChild != null)
                {
                    privDumpTreeDepthFirst(pChild);
                    // goto next sibling
                    pChild = pChild.pSibling;
                }
            }
            else
            {
                // bye bye exit condition
            }
        }

        private void privInfoAddNode()
        {
            this.numNodes += 1;

            if (this.numNodes > this.maxNodeCount)
            {
                this.maxNodeCount += 1;
            }
        }
        private void privInfoRemoveNode()
        {
            numNodes -= 1;
        }


        public class PCSRootNode : PCSNode
        {

            // Data -----------------------------------------------------
            private PCSTree.Name name;

            public PCSRootNode(PCSTree.Name treeName)
                : base()
            {
                this.name = treeName;
            }

            public override Enum getName()
            {
                return this.name;
            }
        }



    }
}
