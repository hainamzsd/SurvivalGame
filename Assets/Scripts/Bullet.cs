using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;

    private Vector2 direction;


    public void SetDirection(Quaternion rotation)
    {
        direction = rotation * Vector2.right;
    }

    private void Update()
    {
        // Move the bullet in the specified direction
        transform.Translate(direction * speed * Time.deltaTime);
        
    }
    private void ontriggerenter2d(Collider2D other)
    {
        // destroy the bullet when it collides with another object
        if(other.tag != "Player" ||  other.tag != "Weapon" || other.tag != "Bullet") { }
        Destroy(gameObject);
    }
}
