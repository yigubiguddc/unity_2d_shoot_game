using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MOVE : MonoBehaviour
{
    public int x;
    public int y;
    public Rigidbody2D _rigidbody;
    public Vector2 input;
    public int moveSpeed = 20;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
        _rigidbody.velocity = input.normalized * moveSpeed;
    }
}
