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

    public void SetupText(string text, float disappearingSpeed, int type)
    {
        switch (type)
        {
            case 1: 
                textMeshPro.color = Color.green;
                break;
            case 2:
                textMeshPro.color = Color.red;
                break;
            case 3:
                textMeshPro.color = Color.yellow;
                break;
            case 4:
                textMeshPro.color = Color.cyan;
                break;
            case 5:
                textMeshPro.color = Color.white;
                break;
        }
        textMeshPro.SetText(text);
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
