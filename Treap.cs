

using System;

namespace treapproject
{
    /* Class for Treap data structure */

    public class Treap<T> where T : IComparable
    {
        //References the root of the Treap
        private TreapNode<T> _root;     //Reference to the root node of the treap (private field per C# naming guidelines)

        /* Method 1: RotateRight
            - Performs a right rotation around the given root
            - Used when the left child has a higher priority than the parent
            - Pushes the parent down to the right and the left child becomes the new root
            - Time complexity:  O(1) */ 

        private TreapNode<T> RotateRight(TreapNode<T> root)
        {
            TreapNode<T> leftChild = root.Left;                     //Stores the left child of root (pointer reference)
            TreapNode<T> leftChildRightSubtree = leftChild.Right;   //Stores the left child's right subtree (will become roots left child)
            
            //Performs rotation
            leftChild.Right = root;                                 //Makes root the right child of leftChild
            root.Left = leftChildRightSubtree;                      //Connects stored subtree as new left child of root

            return leftChild;         //Returns with leftChild as the new root with rotated subtrees
        }

        /* Method 2: RotateLeft
            - Performs a left rotation around the given root
            - Used when the right child has a higher priority than the parent
            - Pushes the parent down to the left and the right child becomes the new root
            - Time complexity:  O(1) */ 

        private TreapNode<T> RotateLeft(TreapNode<T> root)
        {
            TreapNode<T> rightChild = root.Right;                     //Stores the right child of root (pointer reference)
            TreapNode<T> rightChildLeftSubtree = rightChild.Left;     //Stores the right child's left subtree (will become roots right child)
            
            rightChild.Left = root;                                 //Makes root the left child of rightChild
            root.Right = rightChildLeftSubtree;                     //Connects stored subtree as new right child of root

            return rightChild;         //Returns with rightChild as the new root with rotated subtrees
        }

        /* Method 3: Insertion (Public)
            - Inserts a new key into the Treap 
            - TreapNode constructor generates a random priority for the new node
            - Calls the private Insert method to perform the recursive BST insert and necessary rotations
            - Expected time: O(log n) */ 

        public void Insert(T key)
        {
            _root = Insert(_root, key);         //Updates the root after insertion and rotations
        }

        /* Method 4: Insertion (Private)
            - Recursively inserts a new key into the Treap 
                1) Inserts key according to Binary Search Tree rules
                2) Fixes heap property using rotations (if a child has higher priority) 
            - Creates a new node when an empty spot is found
            - Expected time: O (log n) */ 
        private TreapNode<T> Insert(TreapNode<T> root, T key)
        {
            //If empty spot found
            if (root == null)
            {
                return new TreapNode<T>(key);       //Creates new node, random priority determined in constructor
            }

            //Compares the new key with the current root to determine traversal direction to insert
            int cmp = key.CompareTo(root.Key);

            //If new key is smaller, insert into the left subtree
            if (cmp < 0)
            {
                //Insert into the left subtree
                root.Left = Insert(root.Left, key);     

                //If left child violates heap property, rotate right
                if (root.Left != null && root.Left.Priority > root.Priority)
                {
                    root = RotateRight(root);
                }
            }
            //If new key is larger, insert into the right subtree
            else if (cmp > 0)
            {
                //Insert into right subtree
                root.Right = Insert(root.Right, key);

                //If right child violates heap property, rotate left
                if (root.Right != null && root.Right.Priority > root.Priority)
                {
                    root = RotateLeft(root);
                }
            }
            //If key already exists, do not insert
            else
            {
                //Key already in treap, so we ignore duplicates
            }

            return root;
        }

        /* Method 5: Search
            - Searches the treap for a given key
            - Traverses like a Binary Search Tree
                -> Returns true if the key is found
                -> Returns false otherwise
            - Expected time: O(log n) */ 
        public bool Search(T key)
        {
            TreapNode<T> current = _root;       //Starts search from the node

            while (current != null)
            {
                //Compares the search key with the current root to determine traversal direction
                int cmp = key.CompareTo(current.Key);

                if (cmp == 0)
                    return true;                //Key found
                else if (cmp < 0)
                    current = current.Left;     //Move left if key is smaller
                else
                    current = current.Right;    //Move right if key is larger
            }

            return false;                       //Key not found
        }

        /* Method 6: Deletion (Public)
            - Deletes a key from the Treap 
                -> Returns true if key was removed
                -> Returns false otherwise (key not found)
            - Calls the private Deletion method to perform the recursive deletion and necessary rotations
            - Expected time: O(log n) */ 
        public bool Delete(T key)
        {
            bool removed;                                   //Tracks if a node was actually deleted
            _root = Delete(_root, key, out removed);        //Updates the root after deletion and rotations
            return removed;
        }

        /* Method 7: Deletion (Private)
            - Recursively deletes a new key from the Treap
                1) Search for the key using BST property
                2) When found:
                    -> If node has 0 or 1 child, delete in the usual BST way
                    -> If node has 2 children:
                        - Rotate the node down (towards the child with higher priority) to preserve heap property,
                        - Continue deletion once node has <= 1 child */ 
        private TreapNode<T> Delete(TreapNode<T> root, T key, out bool removed)
        {
            //If empty spot found
            if (root == null)
            {
                removed = false;        //Update tracker
                return null;
            }

            //Compares key with the current node to determine traversal direction
            int cmp = key.CompareTo(root.Key);

            //If key is smaller
            if (cmp < 0)
            {
                //Go Left
                root.Left = Delete(root.Left, key, out removed);
            }
            //If key is larger
            else if (cmp > 0)
            {
                //Go Right
                root.Right = Delete(root.Right, key, out removed);
            }
            //If key is found
            else
            {
                removed = true;

                //Case 1: No children (leaf node)
                if (root.Left == null && root.Right == null)
                {
                    return null;
                }

                //Case 2: One child
                //If only right child
                if (root.Left == null)
                {
                    return root.Right;
                }
                //If only left child
                if (root.Right == null)
                {
                    return root.Left;
                }

                /* Case 3: Two children
                    - Rotate with the child that has the higher priority to 
                    - Preserves heap property while pushing this node down */
                if (root.Left.Priority > root.Right.Priority)
                {
                    //Rotate right, then continue deleting key
                    root = RotateRight(root);

                    //After rotation, the original root is now root.Right
                    bool dummy;
                    root.Right = Delete(root.Right, key, out dummy);       //Delete the key from the right subtree
                }
                else
                {
                    //Rotate left, then continue deleting key
                    root = RotateLeft(root);

                    //After rotation. original root is now root.Left
                    bool dummy;
                    root.Left = Delete(root.Left, key, out dummy);          //Delete the key from the left subtree
                }
            }

            return root;        //Returns the updated root 
        }


    }
}