using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody rb;

    public Vector3 moveInput;

	private void Update()
	{
		moveInput.x = Input.GetAxis("Horizontal");
		moveInput.y = Input.GetAxis("Vertical");
		transform.position += moveInput * moveSpeed * Time.deltaTime;
	}
	private void OnTriggerEnter2D(Collider2D item)
	{
		/*if (item.tag.Equals("Item"))
		{
			Destroy(item.gameObject);
		}*/
	}
}
