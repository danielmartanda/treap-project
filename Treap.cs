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
    }

}