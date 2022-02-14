using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FibonacciSearch : MonoBehaviour
{


 
    /// <summary>
    ///  Function to find minimum of two elements
    /// </summary>
    /// <param name="x">first value</param>
    /// <param name="y">second value</param>
    /// <returns></returns>
    public static int min(int x, int y)
    {
        return (x <= y) ? x : y;
    }

   
    /// <summary>
    /// Main Search Formula
    /// </summary>
    /// <param name="array"></param>
    /// <param name="final"></param>
    /// <param name="gap"></param>
    /// <returns> index of x if present, else returns -1 </returns>
    public static int FibonacciSearchMain(int[] array, int final,
                                         int gap)
    {
        // Starting fibonacci sequence from 0
        int fibNo1 = 0; // Fibonacci starting number 1
        int fibNo2 = 1; // Fibonacci starting number 2
        int fibAdd = fibNo1 + fibNo2; // The fibonacci sequence

        //fibAdd is going to store the smallest fibonacci number greater than or equal to gap
        while (fibAdd < gap)
        {
            fibNo1 = fibNo2;
            fibNo2 = fibAdd;
            fibAdd = fibNo1 + fibNo2;
        }

        // The array offset
        int offset = -1;

        //Compare the second number with the final number and move to the next pair of numbers
        while (fibAdd > 1)
        {
            // Check if number 2 is in the right place
            int i = min(offset + fibNo1, gap - 1);

            // offset if value is greater than next fib
            if (array[i] < final)
            {
                fibAdd = fibNo2;
                fibNo2 = fibNo1;
                fibNo1 = fibAdd - fibNo2;
                offset = i;
            }

            //Dont offset if value is less than next fib
            else if (array[i] > final)
            {
                fibAdd = fibNo1;
                fibNo2 = fibNo2 - fibNo1;
                fibNo1 = fibAdd - fibNo2;
            }

            // Ladies and Gentlemen... We got em
            else
                return i;
        }

       // Compare the last value
        if (fibNo2 == 1 && array[gap - 1] == final)
            return gap - 1;

        // Cant find the item searched for
        return -1;
    }

    /// <summary>
    /// The values to be used in the search
    /// </summary>
    public static void Values()
    {
        int[] searchArray = { 11, 33,54,56,59,91,105 };

        int gap = searchArray[1] - searchArray[0];
        int final = searchArray[searchArray.Length];

        int index = FibonacciSearchMain(searchArray, final, gap);
        if (index >= 0)
            Debug.Log("Found at index: " + index);
        else
            Debug.Log(final + " isn't in the array");
    }
}

