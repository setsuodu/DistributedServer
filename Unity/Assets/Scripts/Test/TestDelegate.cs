using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Random = System.Random;
using Debug = UnityEngine.Debug;

// ί����ʹ�����½�
public class TestDelegate : MonoBehaviour
{
    public Random RandomGeneration = new Random();
    private Func<int> exprField;

    void Awake()
    {
        exprField = () => RandomGeneration.Next() * RandomGeneration.Next();
    }

    public void CalculateNormal()
    {
        var s = RandomGeneration.Next() * RandomGeneration.Next();
    }

    public void CalculateUsingFunc()
    {
        Calculate(() => RandomGeneration.Next() * RandomGeneration.Next());
    }

    public void CalculateUsingFuncField()
    {
        //�ں����ⲿ�����괫�룬�ɽ�ʡ����

        //ʵʱ���㣬�����ܲ��粻��ί��
        //exprField = () => RandomGeneration.Next() * RandomGeneration.Next();
        Calculate(exprField);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Calculate(Func<int> expr)
    {
        return expr();
    }

    public void OnClick1()
    {
        Stopwatch timer = new Stopwatch();
        timer.Start();
        for (int i = 0; i < 1000; i++)
        {
            CalculateNormal();
        }
        timer.Stop();
        var micro = timer.Elapsed.Ticks / 10m;
        Debug.Log($"Use {micro} ms");
    }

    public void OnClick2()
    {
        Stopwatch timer = new Stopwatch();
        timer.Start();
        for (int i = 0; i < 1000; i++)
        {
            CalculateUsingFunc();
        }
        timer.Stop();
        var micro = timer.Elapsed.Ticks / 10m;
        Debug.Log($"Use {micro} ms");
    }

    public void OnClick3()
    {
        Stopwatch timer = new Stopwatch();
        timer.Start();
        for (int i = 0; i < 1000; i++)
        {
            CalculateUsingFuncField();
        }
        timer.Stop();
        var micro = timer.Elapsed.Ticks / 10m;
        Debug.Log($"Use {micro} ms");
    }
}