using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

public class BaseFormula
{
    public virtual double Calculate(double x) {
        return 0;
    }
    public virtual long CalculateRound(double x)
    {
        return 0;
    }

    public static HashTable<T, double> Calculate<T>(HashTable<T, BaseFormula> hcf, double x)
    {
        HashTable<T, double> result = new HashTable<T, double>();
        foreach (T t in hcf.Keys)
        {
            result[t] = hcf[t].Calculate(x);
        }
        return result;
    }
}

public class HardcoreFormula : BaseFormula
{
    public List<double> values = new List<double>();
    public override double Calculate(double x)
    {
        int index = (int)x;
        if(index< 0 || index >= values.Count)
        {
            return 0;
        }
        return values[index];
    }

    public override long CalculateRound(double x)
    {
        return (long)Calculate(x);
    }


}



public class Formula : BaseFormula
{
    public double offset;

    public double linearMultiplier;

    public double powerFunctionMultiplier;
    public double powerFunctionPower;

    public double exponentialFunctionMultiplier;
    public double exponentialFunctionBase;
    public double exponentialFunctionPowerMultiplier;

    internal static Formula Exp(double exponentialFunctionBase, double exponentialFunctionMultiplier)
    {
        return new Formula()
        {
            exponentialFunctionPowerMultiplier = 1,
            exponentialFunctionBase = exponentialFunctionBase,
            exponentialFunctionMultiplier = exponentialFunctionMultiplier
        };
    }

    public double exponentialFunctionPowerOffset;

    public override double Calculate(double x)
    {
        return offset +
            linearMultiplier * x +
            powerFunctionMultiplier * Math.Pow(x, powerFunctionPower) +
            exponentialFunctionMultiplier * Math.Pow(exponentialFunctionBase, exponentialFunctionPowerMultiplier * x + exponentialFunctionPowerOffset);
    }

    public override long CalculateRound(double x)
    {
        double value = Calculate(x);
        if (Math.Abs(value) < 1) return value > 0 ? 1 : -1;
        return (long) Math.Round(value);
    }

    public static Formula Linear(double offset, double k)
    {
        return new Formula()
        {
            linearMultiplier = k,
            offset = offset
        };
    }

    public static Formula Const(double offset)
    {
        return new Formula()
        {
            offset = offset
        };
    }
}