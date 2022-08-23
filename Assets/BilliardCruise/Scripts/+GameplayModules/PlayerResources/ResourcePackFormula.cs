using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

[Serializable]
public class FormulaObject
{
    public IResource entity;
    public Formula count;

    public FormulaObject()
    {

    }

    public FormulaObject(IResource _entity, Formula _count)
    {
        entity = _entity;
        count = _count;
    }
}

public class ResourcePackFormula : List<FormulaObject>
{

    public ResourcePackFormula Add(IResource entity, Formula formula)
    {
        Add(new FormulaObject(entity, formula));
        return this;
    }

    public ResourcePack Calculate(double x)
    {
        ResourcePack result = new ResourcePack(); 
        foreach(FormulaObject fo in this)
        {
            result.Add(fo.entity, fo.count.Calculate(x));
        }
        return result;
    }
    public ResourcePack CalculateRound(double x)
    {
        ResourcePack result = new ResourcePack();
        foreach (FormulaObject fo in this)
        {
            result.Add(fo.entity, fo.count.CalculateRound(x));
        }
        return result;
    }
    public ResourcePack CalculateRoundPositive(double x)
    {
        ResourcePack result = new ResourcePack();
        foreach (FormulaObject fo in this)
        {
            result.Add(fo.entity, Mathf.Max(0,fo.count.CalculateRound(x)));
        }
        return result;
    }
}