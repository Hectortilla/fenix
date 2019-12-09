using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class AsyncQueue<T> {
    private Queue<T> queue = new Queue<T>();

    public void Enqueue(T item) {
        lock(queue) {
            queue.Enqueue(item);
        }
    }
    public T Dequeue() {
        lock(queue) {
            if (queue.Count > 0) {
                return queue.Dequeue();
            }
        }
        return default(T);
    }
}
