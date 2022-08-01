using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFollow3DObject : MonoBehaviour
{


    public Transform target;
    public Vector3 offset;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(target.position) - new Vector3(Screen.width / 2f, Screen.height / 2f);
        GetComponent<RectTransform>().localPosition = screenPos;
    }


}
