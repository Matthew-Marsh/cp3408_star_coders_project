using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node 
{
    public enum Status { SUCCESS, RUNNING, FAILURE};
    public Status status;
    public List<Node> children = new List<Node>();
    public int currentChild = 0;
    public string name;

    public Node() { }

    public Node(string n)
    {
        name = n;
    }

    public virtual Status Process()
    {
        return children[currentChild].Process();
    }

    public void AddChild(Node n)
    {
        children.Add(n);
    }

}

public class Sequence : Node
{
    public Sequence(string n)
    {
        name = n;
    }

    public override Status Process()
    {
        Status childstatus = children[currentChild].Process();
        if (childstatus == Status.RUNNING) return Status.RUNNING;
        if (childstatus == Status.FAILURE) return childstatus;

        currentChild++;
        if (currentChild >= children.Count)
        {
            currentChild = 0;
            return Status.SUCCESS;
        }

        return Status.RUNNING;
    }
}

public class Selector : Node
{
    public Selector(string n)
    {
        name = n;
    }

    public override Status Process()
    {
        Status childstatus = children[currentChild].Process();
        if (childstatus == Status.RUNNING) return Status.RUNNING;

        if (childstatus == Status.SUCCESS)
        {
            currentChild = 0;
            return childstatus;
        }

        currentChild++;
        if (currentChild >= children.Count)
        {
            currentChild = 0;
            return Status.FAILURE;
        }

        return Status.RUNNING;
    }

}

public class Leaf : Node
{
    public delegate Status Tick();
    public Tick ProcessMethod;


    public Leaf() { }

    public Leaf(string n, Tick pm)
    {
        name = n;
        ProcessMethod = pm;
    }

    public override Status Process()
    {
        if (ProcessMethod != null)
        {
            return ProcessMethod();
        }
        return Status.FAILURE;
    }
}