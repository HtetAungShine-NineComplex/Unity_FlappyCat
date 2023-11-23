using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Example : MonoBehaviour
{
    private void Start()
    {
        /*MyClass classOne = new MyClass(7);
        MyClass classTwo = new MyClass(8);*/
        // StructFunc();
        ClassFunc();
        
    }
    public void StructFunc() //Value Type //Copied the data //Used for value type
    {
        MyStruct structOne = new MyStruct(1);
        MyStruct structTwo = structOne;
        structTwo.value = 2;
        Debug.Log(structOne.value);
        Debug.Log(structTwo.value);
    }

    public void ClassFunc() //Reference Type //Created brand new pointer 
    {
        MyClass classOne = new MyClass(1);
        MyClass classTwo = classOne;
        classTwo.value = 2;
        Debug.Log("The value of class one = "+classOne.value);
        Debug.Log("The value of class two = "+classTwo.value);
    }
}


public struct MyStruct
{
    public int value;

    public MyStruct(int value)
    {
        this.value = value;
    }
}

public class MyClass
{
    public int value;

    public MyClass(int value)
    {
        this.value = value;
    }
}

//In c# data is stored in 2 places. Stack and Heap,
//Stack, fixed data //Used for value type //Hold the data
//Heap, //Used for ref type //Hold a ref to the data
