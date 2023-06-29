using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralUIScript : MonoBehaviour
{
    [SerializeField]
    private Transform pfDamagePopUp;
    [SerializeField]
    private float disappearingSpeed;

    //Create Pop Up number text.
    //Type 1 = heal (green), Type 2 = damage (red), Type 3 = atkSpeed (yellow), Type 4 = buff (blue), Type 5 = notification (white)
    public void createPopUp(Vector3 position, string text, int type)
    {
        Transform damagePopUpTransform = Instantiate(pfDamagePopUp, position, Quaternion.identity);
        DamagePopUpScript damagePopUpScript = damagePopUpTransform.GetComponent<DamagePopUpScript>();
        damagePopUpScript.SetupText(text, disappearingSpeed, type);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
