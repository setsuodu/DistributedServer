using System;
using System.Diagnostics;
using UnityEngine;

// C#���������Ż���ʾ֮����װ���Ż�
// ���÷���<T> �� ����ʽ�е�װ����� IEquatable<T>
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
            act?.Invoke(); //ʹ��ί�����б�����
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
        // ʹ��ί�б���
        LoopTest(() => { c.Equals(d); });

        // ����ί�и���
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
        //return base.Equals(obj); //400-600ms����Rectangle1��ͬһ������
        return val == obj.val; //2.4-2.6ms //ʹ��ί��5.4-5.7ms
    }
}