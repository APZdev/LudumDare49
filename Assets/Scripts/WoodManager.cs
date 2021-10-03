using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WoodManager : MonoBehaviour
{
    public Image wood;
    public int woodCount;
    public Sprite gotWood;
    public Sprite wereWood;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (woodCount == 0)
        {
            wood.sprite = wereWood;
        } else
        {
            wood.sprite = gotWood;
        }
    }
}
