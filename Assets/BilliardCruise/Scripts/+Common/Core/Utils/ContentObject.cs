using System;

[Serializable]
public class ContentObject<T>
{
    public T entity;
    public double count;
    public ContentObject(T e, double c)
    {
        entity = e;
        count = c;
    }
    public override string ToString()
    {
        return "[" + entity + "]x" + count;
    }
}
