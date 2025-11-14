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
using System.ComponentModel.Design.Serialization;
using System.Runtime.CompilerServices;

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
        }
        
    }

}