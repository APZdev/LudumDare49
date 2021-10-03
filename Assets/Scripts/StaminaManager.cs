using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaManager : MonoBehaviour
{
    /*J'ai regl� les deux variables de stamina en float car bar.fillAmount ne supporte que �a. 
    Si les valeurs sont en "double", h�sitez pas � changer et faire un cast sur l'instruction*/
    public float stamina;
    public float maxStamina;
    public Image bar;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bar.fillAmount = stamina / maxStamina;
    }
}
