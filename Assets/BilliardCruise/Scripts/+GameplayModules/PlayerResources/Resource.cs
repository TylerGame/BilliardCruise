using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public interface IResource
{
    public Sprite GetSprite();
    public Sprite GetSmallSprite();

    public int GetHashCode();

    public bool Equals(object obj);

    public string GetTitle();
    public string GetSubtitle();


}

