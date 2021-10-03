using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WoodCounter : MonoBehaviour
{
    public Text text;
    // woodCount devrait �tre regl� sur la m�me variable que son �quivalent dans "WoodManager" si possible
    public int woodCount;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (woodCount != 0)
        {
            text.text = ("x" + woodCount);
        } else
        {
            text.text = "";
        }
    }
}
