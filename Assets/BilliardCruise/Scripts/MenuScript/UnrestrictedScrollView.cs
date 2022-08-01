using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnrestrictedScrollView : MonoBehaviour
{
    [SerializeField]
    int buttonSize = 10;
    [SerializeField]
    int numberOfButtons = 4;
    [SerializeField]
    RectTransform cont;
    // Start is called before the first frame update
    void Start()
    {
        int mValue = buttonSize * numberOfButtons;
    }

    // Update is called once per frame
    void Update()
    {
        if(cont.offsetMax.y < 0)
        {
            cont.offsetMax = new Vector2();
            cont.offsetMin = new Vector2();
        }

        if(cont.offsetMax.y > (numberOfButtons * buttonSize) - buttonSize)
        {
            cont.offsetMax = new Vector2(0, (numberOfButtons * buttonSize) - buttonSize);
            cont.offsetMin = new Vector2();
        }
    }
}
