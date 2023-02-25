using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePlayerMovementTestByYiwen : MonoBehaviour
{
    public Transform player;
    public Rigidbody playerBody;
    [SerializeField] private float JumpMagnitude = 10f;
    // Start is called before the first frame update
    void Start()
    {
        playerBody = GetComponent<Rigidbody>();
        player.position = new Vector3(0, 5, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Jump();
        }
        if (Input.GetKey(KeyCode.A)) {
            moveLeft();
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveRight();
        }
    }
    // fixed interval
    private void FixedUpdate()
    {

    }

    void Jump()
    {
        playerBody.AddForce(JumpMagnitude * Vector2.up);
    }

    private void moveLeft()
    {
        playerBody.AddForce(Vector2.left);
    }
    private void moveRight()
    {
        playerBody.AddForce(Vector2.right);
    }
}
