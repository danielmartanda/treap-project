

using System;
using System.Collections.Generic;

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
            TreapNode<T> current = _root;       //Starts search from the root

            while (current != null)
            {
                //Compares the search key with the current node to determine traversal direction
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
            - Recursively deletes a key from the Treap
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
                    - Rotate with the child that has the higher priority to preserve heap property while pushing this node down */
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

        /* Method 8: Split (Public)
            - Splits the current treap into two separate treaps based on a given key 
                -> leftTreap contains all keys which are <= splitKey
                -> rightTreap contains all keys which are > splitKey
            - Original treap root is no longer used
            - Expected time: O(log n) */
        public void Split(T key, out Treap<T> leftTreap, out Treap<T> rightTreap)
        {
            //Performs the recursive split from current root
            Split(_root, key, out TreapNode<T> leftRoot, out TreapNode<T> rightRoot);

            //Creates two new treap objects and assigns their roots
            leftTreap = new Treap<T>();
            rightTreap = new Treap<T>();

            //Directly assigns to their _root fields because we are inside Treap class 
            leftTreap._root = leftRoot;
            rightTreap._root = rightRoot;
        }

        /* Method 9: Split (Private)
            - Recursively splits the treap into two parts based on a given key 
                -> leftRoot contains all keys which are <= splitKey
                -> rightRoot contains all keys which are > splitKey
            - Expected time: O(log n) */
        private static void Split(TreapNode<T> root, T key, out TreapNode<T> leftRoot, out TreapNode<T> rightRoot)
        {
            //If subtree is empty
            if (root == null)
            {
                leftRoot = null;
                rightRoot = null;
                return;
            }

            //Compare the split key with the current node
            int cmp = key.CompareTo(root.Key);

            //If splitKey is smaller, current node belongs to right side
            if (cmp < 0)
            {
                //All keys <= splitKey must go into left subtree, recursively split the left subtree
                Split(root.Left, key, out leftRoot, out TreapNode<T> newLeftRight);

                root.Left = newLeftRight;       //Reattaches remaining right part of the left subtree
                rightRoot = root;               //Current root becomes part of right treap
            }
            else
            {
                //Current node's key <= splitKey, so it belongs in the left treap
                Split(root.Right, key, out TreapNode<T> newRightLeft, out rightRoot);

                root.Right = newRightLeft;      //Reattaches remaining left part of the right subtree
                leftRoot = root;                //Current root becomes part of the left treap
            }
        }

        /* Method 10: Merge (Public)
            - Merges two separate treaps into one combined treap 
            - Conditions: 
                -> All keys in left treap < every key in the right treap
                -> The root with the highest priority will be the new root
            - Calls the private merge helper method to recursively combine the nodes
            - Returns a fully merged treap that maintains BST and heap property
            - Expected time: O(log n) */
        public static Treap<T> Merge(Treap<T> leftTreap, Treap<T> rightTreap)
        {
            Treap<T> merged = new Treap<T>();           //Creates a new treap to hold the merged result
            merged._root = Merge(leftTreap._root, rightTreap._root);        //Calls private method on roots
            return merged;      //Returns the merged treap
        }

        /* Method 11: Merge (Private)
            - Recursively merges two treap roots into one combined treap 
                1) If one treap is empty, return the other
                2) Otherwise, choose the root with the highest priority as the new root
                    -> Recursively merge the opposite subtree of that root
            - Condition: 
                -> All keys in leftRoot are <= all keys in rightRoot
            - Preserves BST and heap property
            - Expected time: O(log n) */
        
        private static TreapNode<T> Merge(TreapNode<T> leftRoot, TreapNode<T> rightRoot)
        {
            //If one subtree is empty, return the other
            if (leftRoot == null)
                return rightRoot;
            if (rightRoot == null)
                return leftRoot;

            //Choose the node with the higher priority to stay as root
            if (leftRoot.Priority > rightRoot.Priority)
            {
                //leftRoot remains root & merge its right subtree with rightRoot
                leftRoot.Right = Merge(leftRoot.Right, rightRoot);
                return leftRoot;         //Returns new root
            }
            else
            {
                //rightRoot remains root & merge its left subtree with leftRoot
                rightRoot.Left = Merge(leftRoot, rightRoot.Left);
                return rightRoot;       //Returns new root
            }
        }

        /* Method 12: RangeQuery (Public)
            - Returns a list of all keys such that low <= key <= high
            - Uses the private RangeQuery helper method to traverse the treap
            - Expected Time: O(k + h), where k is number of keys returned and h is treap height  */
        public List<T> RangeQuery(T low, T high)
        {
            List<T> result = new List<T>();             //Stores all keys in the desired range
            RangeQuery(_root, low, high, result);       //Starts range query from the root
            return result;                              //Returns the collected keys
        }

        /* Method 13: RangeQuery (Private)
            - Recursively returns all keys between the low and high
            - Use BST traversal to skip subtrees out of range
                -> if root.Key > low, possible values in the left subtree may be in range, so we explore left
                -> if low <= root.Key <= high, add root.Key to result list
                -> if root.Key < high, possible values in right subtree may be in range, so we explore right
            - Expected Time: O(k + h), where k is number of keys returned and h is treap height */
        private static void RangeQuery(TreapNode<T> root, T low, T high, List<T> result)
        {
            //If subtree is empty, there is nothing to add
            if (root == null)
                return;

            //If low is less than current key, values in left subtree might be in range
            if (low.CompareTo(root.Key) < 0)
                RangeQuery(root.Left, low, high, result);

            //If current key is within range, add it to the result
            if (low.CompareTo(root.Key) <= 0 && root.Key.CompareTo(high) <= 0)
                result.Add(root.Key);

            //If current key is less than high, values in right subtree might be in range 
            if (root.Key.CompareTo(high) < 0)
                RangeQuery(root.Right, low, high, result);
        }

    }
}