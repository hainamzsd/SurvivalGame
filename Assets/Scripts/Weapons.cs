using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : MonoBehaviour
{
    private Transform player;
    private Transform pivotPoint;

    public int maxClip { get; set; } = 20;
    public int currentAmmo { get; set; } = 20;

    private void Start()
    {
        pivotPoint = GameObject.FindGameObjectWithTag("WeaponPivot").transform;
    }

    private void Update()
    {
        if (player != null)
        {
            // Update the weapon's position to match the pivot point's position
            transform.position = pivotPoint.position;
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.transform;

            // Attach the weapon to the pivot point
            transform.SetParent(pivotPoint);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;

            
        }
    }






}
