using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIScript : MonoBehaviour
{
    [SerializeField]
    private Transform pfDamagePopUp;
    [SerializeField]
    private float disappearingSpeed;
    public void createPopUp(Vector3 position, float damage)
    {
        Transform damagePopUpTransform = Instantiate(pfDamagePopUp, position, Quaternion.identity);
        DamagePopUpScript damagePopUpScript = damagePopUpTransform.GetComponent<DamagePopUpScript>();
        damagePopUpScript.SetupText(damage, disappearingSpeed);
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
