using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceBar : DataBehaviour
{
    public SimpleResourceId resource;
    public Image icon;
    public TextMeshProUGUI text;
    IResource myResource;

    public double GetValue()
    {
        return PlayerResources.Data.resources.Get(myResource);
    }

    public void Awake()
    {
        myResource = SimpleResource.Create(resource);
    }

    public void OnEnable()
    {
        Subscribe<ResourceUpdateEvent>(_ =>
        {
            text.text = Formats.Number(PlayerResources.Get(myResource));
        });
    }
}
