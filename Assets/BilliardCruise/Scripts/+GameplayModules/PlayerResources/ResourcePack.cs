using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


[Serializable]
public class ResourcePack : List<ContentObject<IResource>>
{
    public ResourcePack(ResourcePack toClone)
    {
        foreach(ContentObject<IResource> co in toClone)
        {
            Add(co);
        }
    }

    public ResourcePack()
    {

    }

    public bool IsEnough(ResourcePack price)
    {
        if (price == null) return false;
        foreach (ContentObject<IResource> another in price)
        {
            ContentObject<IResource> mine = this.Find(co => co.entity.Equals(another.entity));
            if (mine == null || mine.count < another.count) return false;
        }
        return true;
    }

    
    public bool IsEmpty()
    {
        foreach (ContentObject<IResource> mine in this)
        {
            if (mine.count > 0) return false;
        }
        return true;
    }

    public double TotalSumm()
    {
        double result = 0;
        foreach (ContentObject<IResource> mine in this)
        {
            result += mine.count;
        }
        return result;
    }

    public ResourcePack Add(ResourcePack price)
    {
        foreach (ContentObject<IResource> another in price)
        {
            Add(another.entity, another.count);
        }
        return this;
    }


    public static ResourcePack operator +(ResourcePack left, ResourcePack right)
    {
        ResourcePack result = new ResourcePack(left);

        foreach (ContentObject<IResource> co in right)
        {
            result.Add(co.entity, co.count);
        }

        return result;
    }

    public double Get(IResource resource)
    {
        ContentObject<IResource> co = this.Find(co => co.entity.Equals(resource));
        if (co == null) return 0;
        return co.count;
    }

    public ResourcePack Add(IResource resource, double count)
    {
        ContentObject<IResource> mine = this.Find(co => co.entity.Equals(resource));
        if (mine == null)
        {
            mine = new ContentObject<IResource>(resource, count);
            this.Add(mine);
        }
        else
        {
            mine.count += count;
        }
        return this;
    }

    public static ResourcePack Create(IResource resource, double count)
    {
        ResourcePack result = new ResourcePack();
        result.Add(resource, count);
        return result;
    }

    internal static ResourcePack Create(ContentObject<IResource> co)
    {
        ResourcePack result = new ResourcePack();
        result.Add(co.entity, co.count);
        return result;
    }


    internal ResourcePack Set(IResource resource, double count)
    {
        ContentObject<IResource> mine = this.Find(co => co.entity.Equals(resource));
        if (mine == null)
        {
            mine = new ContentObject<IResource>(resource, count);
            this.Add(mine);
        }
        else
        {
            mine.count = count;
        }
        return this;
    }

    public static ResourcePack Create()
    {
        return new ResourcePack();
    }

    public override string ToString()
    {
        string result = "";
        this.ForEach(e =>
        {
            result += e.entity + "x" + e.count + " ";
        });
        return result;
    }

    public ResourcePack Subtract(ResourcePack price)
    {
        foreach(ContentObject<IResource> another in price)
        {
            Add(another.entity, -another.count);
        }
        return this;
    }

}
