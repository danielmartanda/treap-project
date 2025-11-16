/* 
================================================================
Course: COIS 3020 - Data Structures and Algorithms II
Assignment 2: Treap Data Structure
Date: November 2025

Project Description: 
This program defines the generic node class used by the treap data structure. 
Each TreapNode stores a key of type T (used for binary search tree ordering),
a random integer priority (used for the heap property) & references to its 
left and right child nodes. This class will be used by the Treap class for 
insertion, deletion, search, split, merge and range query operations.  

Authors:
1. Ussanth Balasingam - Student ID: 0765174
2. Daniel Martanda - Student ID: 0813510

================================================================
*/

using System;

namespace treapproject
{
    /* Generic Node Class for Treap data structure
        -> Stores a key, random priority and pointers to left and right children
        -> Combines the properties of a randomly built binary search tree with a binary heap
            - Binary Search Tree Properties:
                -> All values in left subtree of node are less than the value of the node
                -> All values in right subtree of node are more than the value of the node
            - Binary Heap Properties:
                -> For each node, the priority is less than or equal to the priority of its parent */
    public class TreapNode<T> where T : IComparable
    {
        //Generates a unique random priority when the node is created
        private static Random random = new Random();        

        //Node Properties
        public T Key { get; set; }              //Stores a key, most likely integer (key is main property of a Treap)
        public int Priority { get; set; }       //Stores a random int priority (priority is secondary property of a Treap)
        
        //Node children pointers
        public TreapNode<T> Left { get; set; }     
        public TreapNode<T> Right { get; set; }     

        //Node Constructor with randomized priority
        public TreapNode(T key)
        {
            Key = key;
            Priority = random.Next(10, 100);        //Random number between 10 and 99
            Left = null;
            Right = null;
        }

        //Node Constructor with specified priority
        public TreapNode(T key, int priority)
        {
            Key = key;
            Priority = priority;
            Left = null;
            Right = null;
        }
    }
}