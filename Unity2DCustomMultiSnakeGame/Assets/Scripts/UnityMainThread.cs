using System;
using System.Collections.Generic;
using UnityEngine;

internal class UnityMainThread : MonoBehaviour
{
    internal static UnityMainThread thread;
    private readonly Queue<Action> jobs = new Queue<Action>();

    void Awake()
    {
        thread = this;
    }

    void Update()
    {
        while (jobs.Count > 0)
            jobs.Dequeue().Invoke();
    }

    internal void AddJob(Action newJob)
    {
        jobs.Enqueue(newJob);
    }
}
