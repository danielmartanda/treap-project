/* 
================================================================
Course: COIS 3020 - Data Structures and Algorithms II
Assignment 2: Treap Data Structure
Date: November 2025

Project Description: 
This program implements a treap data structure, a probabilistic binary search tree
that combines the properties of a binary search tree and a heap. It allows Insertion, 
Deletion, Search, Split, Merge & Range Queries operations to be performed. 

Authors:
1. Ussanth Balasingam - Student ID: 0765174
2. Daniel Martanda - Student ID: 0813510

================================================================
*/

/* 
--------------------------------------------
Treap Data Structure   
--------------------------------------------
- 
- 
- 
*/
using System;
using System.Collections.Generic;
using System.IO.Pipelines;

namespace treapproject
{
    /* Summaray
    Treap data structure:
    Insert: like BST insert + rotate to fix heap property
    Delete: rotate target node down to a leaf, then remove it
    Search: standard BST search
    */

    public class Treap<T> where T : IComparable<T>
    {
        private TreapNode<T> _root;
        private static readonly Random _rng = new Random();

        // summary
        // Public property to expose the root if you need it for testing
        public TreapNode<T> Root => _root;

        public Treap()
        {
            _root = null;
        }

        /*
        Rotations helpers
        */

        // Right rotation around node y: so y at top and x, y as children and x's children a and B 
        // which will rotate to x at the top having children of a and y then y's children B and Y
        private TreapNode<T> RotateRight(TreapNode<T> y)
        {
            TreapNode<T> x = y.Left;
            TreapNode<T> beta = x.Right;

            //Perform rotation
            x.Right = y;
            y.Left = beta;

            return x; // x becomes new root of this subtree

        }

        // Summary
        // Left rotation around node x:
        // x at the top with children of a and y with children of B and Y and would go left so y at the top with children x and Y, with x having children of a and B
        private TreapNode<T> RotateLeft(TreapNode<T> x)
        {
            TreapNode<T> y = x.Right;
            TreapNode<T> beta = y.Left;

            // Perform rotation
            y.Left = x;
            x.Right = beta;

            return y; // y becomes new root of this subtree
        }

        /*
        Insertion
        */

        //Summary
        // Public insert method
        // Generates a random priority for the new node
        // Expected time : O (log n)

        public void Insert(T key)
        {
            _root = Insert(_root, key);
        }

        // Summary
        // Recursive inserts:
        // 1) Insert by BST key
        // 2) Fix heap property using rotations
        private TreapNode<T> Insert(TreapNode<T> root, T key)
        {
            // Base casee: empty spot, creates a new node
            if (root == null)
            {
                int priority = _rng.Next();
                return new TreapNode<T>(key, priority);
            }

            int cmp = key.CompareTo(root.Key);

            if (cmp < 0)
            {
                // Insert into left subtree
                root.Left = Insert(root.Left, key);

                // If left child violates heap property, rotate right
                if (root.Left != null && root.Left.Priority > root.Priority)
                {
                    root = RotateRight(root);
                }
            }
            else if (cmp > 0)
            {
                // Insert into right subtree
                root.Right = Insert(root.Right, key);

                // If right child violates heap property, roate left
                if (root.Right != null && root.Right.Priority > root.Priority)
                {
                    root = RotateLeft(root);
                }
            }
            else
            {
                // cmp = 0: key already in treap 
                // For this assignment, we ignire duplicates
                // and do nothing no new node
            }

            return root;
        }

        /* 
        Search
        */

        // Summary
        // Returns true if key is found in the treap; false otherwise
        // Time: O(h) where h is height, expected 0(long n)
        public bool Search(T key)
        {
            TreapNode<T> current = _root;
            while (current != null)
            {
                int cmp = key.CompareTo(current.Key);

                if (cmp == 0)
                    return true;
                else if (cmp < 0)
                    current = current.Left;
                else
                    current = current.Right;
            }

            return false; // not found
        }

        /*
        Deletion
        */

        // Summary
        // Public delete method
        // Returns true if a node was actually removed; false if key not found
        // Expected time: O(log n)

        public bool Delete(T key)
        {
            bool removed;
            _root = Delete(_root, key, out removed);
            return removed;
        }

        /*Summary
        Recursive delete:
        1) Search for the node by BST property
        2) When found:
            If it has 0 or 1 child, delete in the usual BST way
            If it has 2 children, rotate the node down (towards the child
            with higher priority) to preserve heap property,
            and then delete once it has <= 1 child.
        */
        private TreapNode<T> Delete(TreapNode<T> root, T key, out bool removed)
        {
            if (root == null)
            {
                removed = false;
                return null;
            }

            int cmp = key.CompareTo(root.Key);

            if (cmp < 0)
            {
                // Go Left
                root.Left = Delete(root.Left, key, out removed);
            }
            else if (cmp > 0)
            {
                // Go Right
                root.Right = Delete(root.Right, key, out removed);
            }
            else
            {
                // Found the node to delete 
                removed = true;

                // case 1 : leaf node
                if (root.Left == null && root.Right == null)
                {
                    return null;
                }

                //case2: only one child

                if (root.Left == null)
                {
                    return root.Right;
                }
                if (root.Right == null)
                {
                    return root.Left;
                }

                /*
                Case 3 two children
                Rotate with the child that has higher priority
                so that heap property remains valid while we push 
                this node down the tree
                */
                if (root.Left.Priority > root.Right.Priority)
                {
                    root = RotateRight(root);

                    // After rotation, the original root is now root.Right
                    // We still want to delete 'key', but from the right subtree now
                    bool dummy;
                    root.Right = Delete(root.Right, key, out dummy);
                }
                else
                {
                    root = RotateLeft(root);

                    // After rotation. original root is now root.Left
                    bool dummy;
                    root.Left = Delete(root.Left, key, out dummy);
                }
            }

            return root;
        }

        /*
            Split:
            Given a key k a split one treap into two: 
            leftRoot: all keys <= k
            rightRoot: all keys > k
        */
        private static void Split(TreapNode<T> root, T key, out TreapNode<T> leftRoot, out TreapNode<T> rightRoot)
        {
            if (root == null)
            {
                leftRoot = null;
                rightRoot = null;
                return;
            }

            int cmp = key.CompareTo(root.Key);

            if (cmp < 0)
            {
                // All keys <= key must live entriely in root.Left
                // Split the left subtree
                Split(root.Left, key, out leftRoot, out TreapNode<T> newLeftRight);
                root.Left = newLeftRight; //Reattach leftover part
                rightRoot = root; // Root and its right subtree go to "right"
            }
            else
            {
                // root.Key <= key, so root belongs to left side
                Split(root.Left, key, out TreapNode<T> newRightLeft, out rightRoot);
                root.Right = newRightLeft; //Reattach leftover part
                leftRoot = root; // Root and its right subtree go to "left"
            }
        }

        /*
        Public split:
        Splits this treap into a new Treap objects based on key.
        Left treap gets keys <= key. Right treap gets keys key.
        After this call the original treap's root is not used anymore
        */
        public void Split(T key, out Treap<T> leftTreap, out Treap<T> rightTreap)
        {
            Split(_root, key, out TreapNode<T> leftRoot, out TreapNode<T> rightRoot);

            leftTreap = new Treap<T>();
            rightTreap = new Treap<T>();

            // Because we are inside the Treap class
            // we are allowed to assign to _root of other Treap instances 
            leftTreap._root = leftRoot;
            rightTreap._root = rightRoot;
        }

        /*
        Precondition very important every key in left treap < every key in right treap 
        and we pick whichever root has a higher priority to be the new root, then merges reursviely down 
        one side. That keeps the heap property and the BST ordering


        Private merge helper
        All keys in leftRoot are <= all keys in rightRoot Returns the root of the merged treap
        */
        private static TreapNode<T> Merge(TreapNode<T> leftRoot, TreapNode<T> rightRoot)
        {
            if (leftRoot == null)
                return rightRoot;
            if (rightRoot == null)
                return leftRoot;

            // Choose the higher-priority root to stay as root
            if (leftRoot.Priority > rightRoot.Priority)
            {
                // leftRoot remains root; merge its right subtree with rightRoot
                leftRoot.Right = Merge(leftRoot.Right, rightRoot);
                return leftRoot;
            }
            else
            {
                // rightRoot remains root; merge its left subtree with leftRoot
                rightRoot.Left = Merge(leftRoot, rightRoot.Left);
                return rightRoot;
            }
        }

        /*
        Public Merge:
        Given two treaps where all keys in leftTreap are <= all keys in rightTreap,
        returns a new treap that contains all the keys
        */
        public static Treap<T> Merge(Treap<T> leftTreap, Treap<T> rightTreap)
        {
            Treap<T> merged = new Treap<T>();
            merged._root = Merge(leftTreap._root, rightTreap._root);
            return merged;
        }

        /*
        Ranged Queries:
        Given low and high, return all keys x such that low <= x <= high

        we exploit the 

        if root.Key is bigger than low, explore left
        if root.Key is inrange, add it
        if root.Key is smaller than high, explore right

        Private helper for range query
        adds all keys in [low, high] into result using BST-style traversal
        */
        private static void RangeQuery(TreapNode<T> root, T low, T high, List<T> result)
        {
            if (root == null)
                return;

            // If low is less than current key, values in left subtree might be in range
            if (low.CompareTo(root.Key) < 0)
                RangeQuery(root.Left, low, high, result);

            // If current key is within range, add it
            if (low.CompareTo(root.Key) <= 0 && root.Key.CompareTo(high) <= 0)
                result.Add(root.Key);

            // If current key is less than high, values in right subtree might be in range 
            if (root.Key.CompareTo(high) < 0)
                RangeQuery(root.Right, low, high, result);
        }

        /*
        Returns a list of all keys x such that low <= x <= high
        Time: O(k + h), where k is number of keys returned and h is treap height
        */
        public List<T> RangeQuery(T low, T high)
        {
            List<T> result = new List<T>();
            RangeQuery(_root, low, high, result);
            return result;
        }
    }
}