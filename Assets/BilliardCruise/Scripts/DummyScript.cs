using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DummyScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DOTween.Sequence()
        .Append(transform.DOScale(new Vector2(1.2f, 1.2f), 0.3f).SetDelay(0.3f))
        .Append(transform.DOScale(Vector2.one, 0.3f))
        .SetLoops(-1, LoopType.Restart);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public MyClass[] _myClass;
}



[Serializable]
public class MyClass
{
    [SerializeField]
    public int age;
}
