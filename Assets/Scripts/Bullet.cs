using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    public float bulletSpeed = 20f; // Adjust the bullet speed as desired
    private Vector2 direction;

    // Method to set the direction of the bullet
    public void SetDirection(Vector2 bulletDirection, bool isFacingRight)
    {
        direction = isFacingRight ? bulletDirection.normalized : new Vector2(-bulletDirection.x, bulletDirection.y).normalized;
    }

    void Update()
    {
        // Move the bullet in the specified direction
        transform.Translate(direction * bulletSpeed * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        // destroy the bullet when it collides with another object
        if (other.tag == "Enemy" || other.tag == "Obstacle")
        {
            Destroy(gameObject);
        }
    }

}
