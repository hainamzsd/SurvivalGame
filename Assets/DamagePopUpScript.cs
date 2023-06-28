using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopUpScript : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro textMeshPro;
    private Color currentColor;
    private float disappearingSpeed;

    public void SetupText(float damage, float disappearingSpeed)
    {
        textMeshPro.SetText(damage.ToString());
        currentColor = textMeshPro.color;
        this.disappearingSpeed = disappearingSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        disappearing();
    }

    void disappearing()
    {
        
        //Moving Upward
        transform.position = transform.position + new Vector3(0,1,0) * Time.deltaTime;
        //Disappearing
        currentColor.a = currentColor.a - disappearingSpeed * Time.deltaTime;
        textMeshPro.color = currentColor;
        if (currentColor.a <= 0) Destroy(this.gameObject);
    }
}
