using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Scripting;

[Serializable]
public class SimpleResource : IResource
{
    public static SimpleResource None = new SimpleResource(SimpleResourceId.None);
    public static SimpleResource Gold = new SimpleResource(SimpleResourceId.Gold);
    public static SimpleResource Gem = new SimpleResource(SimpleResourceId.Gem);

    public SimpleResourceId resourceId;

    public override int GetHashCode()
    {
        return (int)resourceId;
    }

    public override bool Equals(object obj)
    {
        base.Equals(obj);
        if (obj.GetType() == GetType() && obj.GetHashCode() == GetHashCode())
        {
            return true;
        }
        return false;
    }

    public override string ToString()
    {
        return resourceId.ToString();
    }

    public Sprite GetSprite()
    {
        if (resourceId == SimpleResourceId.None) return null;
        return AssetDB.LoadSprite("Icons/Simple/" + resourceId.ToString());
    }

    internal static SimpleResource Create(SimpleResourceId res)
    {
        return new SimpleResource(res);
    }

    public Sprite GetSmallSprite()
    {
        return GetSprite();
    }

    public string GetTitle()
    {
        return resourceId.ToString();
        //return Localization.Translate("SimpleResource." + resourceId);
    }

    public string GetSubtitle()
    {
        return "";
    }
    
    public SimpleResource()
    {

    }
    public SimpleResource(SimpleResource sr)
    {
        resourceId = sr.resourceId;
    }
    
    public SimpleResource(SimpleResourceId id)
    {
        resourceId = id;
    }
    
}


