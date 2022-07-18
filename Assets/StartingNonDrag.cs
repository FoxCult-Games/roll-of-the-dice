using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingNonDrag : MonoBehaviour
{
    void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.velocity = Vector2.zero;
    }
}
