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


using System;
using System.Collections.Generic;

namespace treapproject
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.Clear();        //Clears the output

            Console.WriteLine("---- TREAP TEST CASES ----\n");

            Console.WriteLine("------Test 1: Insert Key and Search ------");
            TestInsertAndSearch();

            Console.WriteLine("--------- Test 2: Delete -----------");
            TestDelete();

            Console.WriteLine("--------- Test 3: Split ------------");
            TestSplit();

            Console.WriteLine("--------- Test 4: Merge ------------");
            TestMerge();

            Console.WriteLine("--------- Test 5: Range Query ------");
            TestRangeQuery();

            Console.WriteLine("--------- Test 6: Edge Cases -------");
            TestEdgeCases();

            Console.WriteLine("--------- Test 7: Invalid Test Edge Cases -------");
            TestInvalidInputs();

            Console.WriteLine("\nAll tests completed.");
        }

        //TEST 1: INSERT & SEARCH
        static void TestInsertAndSearch()
        {
            Treap<int> t = new Treap<int>();                    //Initializes new treap with int datatype
            int[] values = { 50, 20, 70, 10, 30, 60, 80 };        //Initializes an array of 7 values

            Console.WriteLine("Inserting keys: {50, 20, 70, 10, 30, 60, 80}");

            //Inserts each value in the array as a key in the treap
            foreach (int v in values)
            {
                t.Insert(v);
            }

            //Searches each value
            Console.WriteLine("Searching for 50 (expecting true) -> Result " + t.Search(50));      //Expected true
            Console.WriteLine("Searching for 60 (expecting true) -> Result " + t.Search(60));      //Expected true
            Console.WriteLine("Searching for 99 (expecting false) -> Result " + t.Search(99));     //Expected false

            Console.WriteLine();
        }

        //TEST 2: DELETE
        static void TestDelete()
        {
            Treap<int> t = new Treap<int>();                //Initializes new treap with int datatype
            int[] values = { 50, 20, 70, 10, 30, 60, 80 };    //Initializes an array of 7 values

            Console.WriteLine("Inserting keys: {50, 20, 70, 10, 30, 60, 80}");

            //Inserts each value in the array as a key in the treap
            foreach (int v in values)
            {
                t.Insert(v);
            }

            //Deletes a node with no children (leaf)
            Console.WriteLine("Deleteing keys and checking search results.....");
            //Deletes a node with no children (leaf)
            Console.WriteLine("Deleting 10 (expecting true) -> Result " + t.Delete(10));
            Console.WriteLine("Searching for 10 (expecting false) -> Result " + t.Search(10));

            //Delete a node with one or two children
            Console.WriteLine("Deleting 20 (expecting true) -> Result " + t.Delete(20));
            Console.WriteLine("Searching 20 (expecting false) -> Result " + t.Search(20));

            //Delete a non-existing key
            Console.WriteLine("Deleting 999 (expecting false) -> Result " + t.Delete(999));

            Console.WriteLine();
        }

        //TEST 3: SPLIT
        static void TestSplit()
        {
            Treap<int> t = new Treap<int>();                //Initializes new treap with int datatype
            int[] values = { 50, 20, 70, 10, 30, 60, 80 };    //Initializes an array of 7 values

            Console.WriteLine("Inserting keys: {50, 20, 70, 10, 30, 60, 80}");

            //Inserts each value in the array as a key in the treap
            foreach (int v in values)
            {
                t.Insert(v);
            }

            //Split at key = 40
            Console.WriteLine("Splitting treap at key: ");
            t.Split(40, out Treap<int> left, out Treap<int> right);

            //Left treap should contain: 10, 20, 30
            //Right treap should contain: 50, 60, 70, 80
            Console.WriteLine("Left treap keys (<= 40):");
            PrintTreapInOrder(left);

            Console.WriteLine("Right treap keys (> 40):");
            PrintTreapInOrder(right);

            Console.WriteLine();
        }

        //TEST 4: MERGE
        static void TestMerge()
        {
            int[] leftValues = { 10, 20, 30 };
            int[] rightValues = { 50, 60, 70 };

            //Inserts left: 10, 20, 30
            Console.WriteLine("Inserting keys: {50, 20, 70, 10, 30, 60, 80}");
            Console.WriteLine("Creating left treap keys:");
            Console.WriteLine(string.Join(",", leftValues));

            //Inserts right: 50, 60, 70
            Console.WriteLine("Creating right treap keys:");
            Console.WriteLine(string.Join(",", rightValues));

            Treap<int> left = new Treap<int>();     //Initializes new left treap
            Treap<int> right = new Treap<int>();    //Initializes new right treap

            foreach (int v in leftValues)
            {
                left.Insert(v);
            }
            foreach (int v in rightValues)
            {
                right.Insert(v);
            }

            Treap<int> merged = Treap<int>.Merge(left, right);

            //Expected merged keys: 10, 20, 30, 50, 60, 70
            Console.WriteLine("Merged treap keys (expect 10, 20, 30, 50, 60, 70):");
            PrintTreapInOrder(merged);

            Console.WriteLine();
        }

        //TEST 5: MERGE QUERY
        static void TestRangeQuery()
        {

            Treap<int> t = new Treap<int>();
            int[] values = { 50, 20, 70, 10, 30, 60, 80, 25, 35, 65 };

            Console.WriteLine("Inserting keys: {50, 20, 70, 10, 30, 60, 80, 25, 35, 65}");

            foreach (int v in values)
            {
                t.Insert(v);
            }

            // Query range [25, 60]
            Console.WriteLine("Performing RangeQuery(25, 60).....");
            List<int> range = t.RangeQuery(25, 60);

            Console.WriteLine("RangeQuery [25, 60] (expect keys between 25 and 60):");
            Console.WriteLine(string.Join(", ", range));

            Console.WriteLine();
        }

        //TEST 6: EDGE CASES
        static void TestEdgeCases()
        {
            Treap<int> t = new Treap<int>();

            Console.WriteLine("Testing operations on an empty treap");

            // Delete on empty treap
            Console.WriteLine("Deleting from empty (expecting false) -> Result " + t.Delete(10));

            // Search on empty treap
            Console.WriteLine("Searching on empty (expecting false) -> Result " + t.Search(10));

            // Split empty treap
            t.Split(50, out Treap<int> left, out Treap<int> right);
            Console.WriteLine("Split empty treap: ");
            Console.WriteLine(" Left empty? " + IsEmpty(left));
            Console.WriteLine(" Right empty? " + IsEmpty(right));

            // Range query on empty treap
            List<int> range = t.RangeQuery(0, 100);
            Console.WriteLine("RangeQuery on empty (expecting 0 items) -> Result " + range.Count);

            Console.WriteLine();
        }

        //TEST 7: INVALID TEST EDGE CASES
        static void TestInvalidInputs()
        {
            Console.WriteLine("Treap should only accept integer keys.");
            Console.WriteLine("---------------------------------------");

            Treap<int> t = new Treap<int>();

            Console.WriteLine("Testing invalid input hannding: ");

            // Trys inserting invalid inputs
            Console.WriteLine("Testing invalid inserts:");
            SafeInsert(t, "$");     
            SafeInsert(t, "6.7");
            SafeInsert(t, "wow");
            SafeInsert(t, "");
            SafeInsert(t, "S");     

            // Trys seearching invalid inputs
            Console.WriteLine("Testing invalid inserts:");
            SafeSearch(t, "john");
            SafeSearch(t, ";*");

            // Try deleting invalid data types
            Console.WriteLine("Testing invalid deltes:");
            SafeSearch(t, "why");
            SafeSearch(t, "@");

            Console.WriteLine();
        } 

        //HELPER METHODS
        // Inorder print using RangeQuery over a wide range
        static void PrintTreapInOrder(Treap<int> t)
        {
            List<int> keys = t.RangeQuery(int.MinValue, int.MaxValue);
            Console.WriteLine(string.Join(", ", keys));
        }

        //Method checks if treap is empty
        static bool IsEmpty(Treap<int> t)
        {
            //If range query is null, then treap is empty
            return t.RangeQuery(int.MinValue, int.MaxValue).Count == 0;
        }

        // Safe insert vaildatation
        static bool SafeInsert(Treap<int> t, string input)
        {
            if (int.TryParse(input, out int num))
            {
                t.Insert(num);
                return true; // On vaild input
            }
            else
            {
                Console.WriteLine($"Invalid insert attempt: '{input}' (non-interger)");
                return false;
            }
        }
        static bool SafeDelete(Treap<int> t, string input)
        {
            if (int.TryParse(input, out int num))
            {
                return t.Delete(num); 
            }
            else
            {
                Console.WriteLine($"Invalid delete attempt: '{input}' (non-interger)");
                return false;
            }
        }
        static bool SafeSearch(Treap<int> t, string input)
        {
            if (int.TryParse(input, out int num))
            {
                return t.Search(num);
            }
            else
            {
                Console.WriteLine($"Invalid search attempt: '{input}' (non-interger)");
                return false;
            }
        }
    }

}
