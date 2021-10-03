using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoatMapRemplissage : MonoBehaviour
{
    public float boatHP;
    public float boatMaxHP;
    public Image sprite;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        sprite.fillAmount = 1 - (boatHP / boatMaxHP);
    }
}
