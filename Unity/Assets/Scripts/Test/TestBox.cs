using System;
using System.Diagnostics;
using UnityEngine;

// C#代码性能优化演示之——装箱优化
// 采用泛型<T> 或 对隐式判等装箱采用 IEquatable<T>
public class TestBox : MonoBehaviour
{
    Rectangle1 a = new Rectangle1 { val = 1 };
    Rectangle1 b = new Rectangle1 { val = 2 };

    Rectangle2 c = new Rectangle2 { val = 1 };
    Rectangle2 d = new Rectangle2 { val = 2 };

    void LoopTest(Action act)
    {
        Stopwatch timer = new Stopwatch();
        timer.Start();
        for (int i = 0; i < 1000; i++)
        {
            //act();
            act?.Invoke(); //使用委托运行变慢？
        }
        timer.Stop();
        var micro = timer.Elapsed.Ticks / 10m;
        UnityEngine.Debug.Log($"Use {micro} ms");
    }

    public void OnClick1()
    {
        LoopTest(() => { a.Equals(b); });

        //Stopwatch timer = new Stopwatch();
        //timer.Start();
        //for (int i = 0; i < 1000; i++)
        //{
        //    a.Equals(b);
        //}
        //timer.Stop();
        //var micro = timer.Elapsed.Ticks / 10m;
        //UnityEngine.Debug.Log($"Use {micro} ms");
    }

    public void OnClick2()
    {
        // 使用委托变慢
        LoopTest(() => { c.Equals(d); });

        // 不用委托更快
        //Stopwatch timer = new Stopwatch();
        //timer.Start();
        //for (int i = 0; i < 1000; i++)
        //{
        //    c.Equals(d);
        //}
        //timer.Stop();
        //var micro = timer.Elapsed.Ticks / 10m;
        //UnityEngine.Debug.Log($"Use {micro} ms");
    }
}

struct Rectangle1
{
    public int val;

    public override bool Equals(object obj)
    {
        //return base.Equals(obj); //400-600ms
        return val == ((Rectangle1)obj).val; //140-190 ms
    }
}

struct Rectangle2 : IEquatable<Rectangle2>
{
    public int val;

    public bool Equals(Rectangle2 obj)
    {
        //return base.Equals(obj); //400-600ms，和Rectangle1是同一个函数
        return val == obj.val; //2.4-2.6ms //使用委托5.4-5.7ms
    }
}