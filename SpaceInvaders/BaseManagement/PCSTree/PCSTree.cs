﻿using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public class PCSTree
    {
        //Nested Objects -----------------------------------------------------
        public enum Name
        {
            RootTree,
            MissileTree,
            AlienGridTree,
            AlienColumnTree,
            BombTree,
            WallTree,
            Not_Initialized
        }
        //nested class = PCSRootNode;
        public class PCSRootNode : PCSNode
        {

            // Data -----------------------------------------------------
            private PCSTree.Name name;
            private int index;

            public PCSRootNode(PCSTree.Name treeName, int _index = 0)
                : base()
            {
                this.name = treeName;
                //root index will always be zero
                this.index = _index;
            }
            public override int GetIndex()
            {
                return this.index;
            }
            public override Enum GetPCSName()
            {
                return this.name;
            }
        }

        //Data -----------------------------------------------------
        private PCSNode pRoot;
        public int numNodes;
        public int maxNodeCount;


        //Methods -----------------------------------------------------

        // constructor
        public PCSTree()
        {
            this.maxNodeCount = 0;
            this.numNodes = 0;

            // create the root node
            PCSNode pcsRoot = new PCSRootNode(PCSTree.Name.RootTree);
            //parent is null since inserting root;
            this.Insert(pcsRoot, null);
        }


        public void SetRoot(PCSNode pRoot)
        {
            Debug.Assert(pRoot != null);
            this.pRoot = pRoot;
        }
        public PCSNode GetRoot()
        {
            Debug.Assert(this.pRoot != null);
            return this.pRoot;
        }
        
        // insert
        public void Insert(PCSNode inNode, PCSNode pParent)
        {
            Debug.Assert(inNode != null);

            // insert to root
            if (pParent == null)
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
        public void DumpTree()
        {
            Debug.WriteLine("");
            Debug.WriteLine("DumpTree () -------------------------------");
            this.privDumpTreeDepthFirst(this.pRoot);
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
        private void privDumpTreeDepthFirst(PCSNode pNode)
        {
            PCSNode pChild = null;

            // dump
            pNode.DumpPCSNode();

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






    }
}
