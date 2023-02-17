using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Random = System.Random;

public class SimpleCollisionTree
{
    private readonly Rect rect;
    private int depth;
    private int maxDepth;
    private SimpleCollisionTree[] scts = new SimpleCollisionTree[4] {null, null, null, null};
    private List<CellData> children = new List<CellData>();


    public SimpleCollisionTree(Rect rect, int maxDepth, int depth = 0)
    {
        this.maxDepth = maxDepth;
        this.depth = depth;
        this.rect = rect;
    }


    public float DetectMaximalCollision(CellData circle)
    {
        var max = 0;
        if (circle.Intersection(rect) < 0) return 0;
        return Mathf.Max(max,
            scts.Max((sct) => sct?.DetectMaximalCollision(circle) ?? 0),
            children.Count > 0 ? children.Max((c) => circle.Intersection(c)) : 0);
    }

    public Rect GetSubtreeRect(int subTree)
    {
        var middle = (rect.min + rect.max) / 2;
        return subTree switch
        {
            0 => new Rect {min = rect.min, max = middle},
            1 => new Rect {min = new Vector2(middle.x, rect.min.y), max = new Vector2(rect.max.x, middle.y)},
            2 => new Rect {min = new Vector2(rect.min.x, middle.y), max = new Vector2(middle.x, rect.max.y)},
            3 => new Rect {min = middle, max = rect.max},
            _ => throw new Exception()
        };
    }

    public void Add(CellData circle)
    {
        var subtree = circle.SubTree(rect);
        if (subtree == -1 || depth == maxDepth)
        {
            children.Add(circle);
            return;
        }

        scts[subtree] ??= new SimpleCollisionTree(GetSubtreeRect(subtree), maxDepth, depth + 1);
        scts[subtree].Add(circle);
    }
}

public class Connection : IDisposable
{
    private UnityEvent uEvent;
    private UnityAction uAction;
    private bool disposed;

    public Connection(UnityEvent uEvent, UnityAction uAction)
    {
        this.uEvent = uEvent;
        this.uAction = uAction;
        uEvent.AddListener(uAction);
    }

    public void Dispose()
    {
        if (disposed) return;
        uEvent.RemoveListener(uAction);
        disposed = true;
    }
}

public class Connection<T> : IDisposable
{
    private UnityEvent<T> uEvent;
    private UnityAction<T> uAction;
    private bool disposed;

    public Connection(UnityEvent<T> uEvent, UnityAction<T> uAction)
    {
        this.uEvent = uEvent;
        this.uAction = uAction;
        uEvent.AddListener(uAction);
    }

    public void Dispose()
    {
        if (disposed) return;
        uEvent.RemoveListener(uAction);
        disposed = true;
    }
}

public static class Extensions
{
    public static bool Is(this Animations anim, Animations anim2)
    {
        return (anim & anim2) != Animations.None;
    }

    public static float NextFloat(this Random random)
    {
        return (float) random.NextDouble();
    }

    public static Vector2 UnitCircle(this Random random)
    {
        var point = new Vector2(random.NextFloat() - 0.5f, random.NextFloat() - 0.5f);
        while (point.sqrMagnitude > 1)
            point = new Vector2(random.NextFloat() - 0.5f, random.NextFloat() - 0.5f);
        return point * 2;
    }

    public static void Dispose(this List<IDisposable> disposables)
    {
        foreach (var disposable in disposables)
        {
            disposable.Dispose();
        }

        disposables.Clear();
    }

    public static void AddListenerOnce(this UnityEvent unityEvent, Action action)
    {
        UnityAction call = default;
        call = () =>
        {
            action?.Invoke();
            unityEvent.RemoveListener(call);
        };
        unityEvent.AddListener(call);
    }

    public static IDisposable Subscribe(this UnityEvent unityEvent, Action action)
    {
        return new Connection(
            unityEvent,
            () => action?.Invoke()
        );
    }
    
    public static IDisposable Subscribe<T>(this UnityEvent<T> unityEvent, Action<T> action)
    {
        return new Connection<T>(
            unityEvent,
            (T arg) => action?.Invoke(arg)
        );
    }

    public static IDisposable SubscribeOnce(this UnityEvent unityEvent, Action action)
    {
        IDisposable conn = default;
        UnityAction call = () =>
        {
            action?.Invoke();
            conn.Dispose();
        };
        conn = new Connection(unityEvent, call);
        return conn;
    }

    public static IDisposable SubscribeOnce<T>(this UnityEvent<T> unityEvent, Action<T> action)
    {
        IDisposable conn = default;
        UnityAction<T> call = (T arg) =>
        {
            action?.Invoke(arg);
            conn.Dispose();
        };
        conn = new Connection<T>(unityEvent, call);
        return conn;
    }
    
    public static float Intersection(this CellData cd, CellData other)
    {
        return Mathf.Max(0, Mathf.Min(other.cellMass.size + cd.cellMass.size - (other.position - cd.position).magnitude, cd.cellMass.size));
    }

    public static float Intersection(this CellData cd, Rect rect)
    {
        var xDist = Mathf.Min(cd.cellMass.size + cd.position.x - rect.min.x, cd.cellMass.size - cd.position.x + rect.max.x);
        var yDist = Mathf.Min(cd.cellMass.size + cd.position.y - rect.min.y, cd.cellMass.size - cd.position.y + rect.max.y);
        if (yDist <= 0 || xDist <= 0) return 0;
        return Mathf.Min(cd.cellMass.size, Mathf.Sqrt(yDist * yDist + xDist * xDist));
    }

    public static int SubTree(this CellData cd, Rect rect)
    {
        var middle = (rect.max + rect.min) / 2;
        var dir = cd.position - middle;
        if (Mathf.Abs(dir.x) < cd.cellMass.size || Mathf.Abs(dir.y) < cd.cellMass.size) return -1;
        if (dir.y < 0)
            return dir.x < 0 ? 0 : 1;
        return dir.x < 0 ? 2 : 3;
    }
}