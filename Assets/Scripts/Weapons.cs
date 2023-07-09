using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : MonoBehaviour
{

    private Transform attachmentPoint;

    // Start is called before the first frame update
    void Start()
    {
        attachmentPoint = GameObject.FindGameObjectWithTag("WeaponPivot").transform;

        // Make the weapon a child of the attachment point
        transform.SetParent(attachmentPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(0f, 0f, -90f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
