using System.Collections;
using System;
using UnityEngine;

public class Heap<T> where T : IHeapItem<T>
{
    public T[] items;
    int currentItemCount;

    public Heap(int maxHeapSize)
    {
        items = new T[maxHeapSize];
    }

    public void Add(T item)
    {
        item.HeapIndex = currentItemCount;
        items[currentItemCount] = item;
        SortUp(item);
        currentItemCount++;
    }

    public T RemoveFirst()
    {
        T firstItem = items[0];
        currentItemCount--;
        items[0] = items[currentItemCount];
        items[0].HeapIndex = 0;
        SortDown(items[0]);
        return firstItem;

    }
    public int Count
    {
        get { return currentItemCount; }
    }
    public bool Contains(T item)
    {
        return Equals(items[item.HeapIndex], item);
    }
    

    public void SortDown(T t)
    {
        //Take the item and check against it's children
        //if the children have a higher priority (aka the fcost is lower which means they have a better path)...
        //swap the child with the lowest fCost with the item
        int leftChildIndex = t.HeapIndex * 2 + 1;
        int rightChildIndex = t.HeapIndex * 2 + 2;
        int swapIndex = 0;

        if (leftChildIndex < currentItemCount)
        {
            swapIndex = leftChildIndex;
            if (rightChildIndex < currentItemCount)
            {
                if (items[leftChildIndex].CompareTo(items[rightChildIndex]) < 0)
                {
                    swapIndex = rightChildIndex;

                }
            }
            if(t.CompareTo(items[swapIndex]) < 0)
            {
                Swap(t, items[swapIndex]);
            }
            else
            {
                return;
            }
        }
        else
        {
            return;
        }
    }

    public void Swap(T item, T swapTarget)
    {
        int itemIndex = item.HeapIndex;
        items[item.HeapIndex] = swapTarget;
        items[swapTarget.HeapIndex] = item;
        item.HeapIndex = swapTarget.HeapIndex;
        swapTarget.HeapIndex = itemIndex;

    }

    public void SortUp(T item)
    {
        int parentIndex = (item.HeapIndex - 1) / 2;
        while (true)
        {
            T parentItem = items[parentIndex];
            if (item.CompareTo(parentItem) > 0)
            {
                Swap(item, parentItem);
            }
            else
            {
                break;
            }

            parentIndex = (item.HeapIndex - 1) / 2;

        }


    }
    public void UpdateItem(T item)
    {
        SortUp(item);
    }
    
}

public interface IHeapItem<T> : IComparable<T>
{
    int HeapIndex { get; set; }
}
