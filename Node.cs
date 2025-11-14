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

namespace treapproject
{
    /*
    Node of treap: stores a key, a random priority,
    and references to left/right children

    Invariants:
    - BST property on Key
    - Max-heap property on Priority
    */

    public class TreapNode<T> where T : IComparable<T>
    {
        public T Key;
        public int Priority;
        public TreapNode<T> Left;
        public TreapNode<T> Right;

        public TreapNode(T key, int priority)
        {
            Key = key;
            Priority = priority;
            Left = null;
            Right = null;
        }
    }
}