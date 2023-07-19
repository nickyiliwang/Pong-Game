using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BallMovement : MonoBehaviour
{
    [SerializeField]
    private float initialSpeed = 10;

    [SerializeField]
    private float speedIncrease = 0.25f;

    [SerializeField]
    private TextMeshProUGUI playerScore;

    [SerializeField]
    private TextMeshProUGUI AIScrore;

    private int hitCounter;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Invoke("StartBall", 1f);
    }

    private void FixedUpdate()
    {
        // so the ball stay in constant speed
        rb.velocity = Vector2.ClampMagnitude(
            // target
            rb.velocity,
            // limit
            initialSpeed + (speedIncrease * hitCounter)
        );
    }

    private void StartBall()
    {
        rb.velocity = new Vector2(-1, 0) * (initialSpeed + speedIncrease * hitCounter);
    }

    private void ResetBall()
    {
        rb.velocity = new Vector2(0, 0);
        transform.position = new Vector2(0, 0);
        hitCounter = 0;
        // sleep for 2 seconds
        Invoke("StartBall", 1f);
    }

    private void PlayerBounce(Transform ballObj)
    {
        hitCounter++;
        Vector2 ballPos = transform.position;
        Vector2 playerPos = ballObj.position;

        float xDirection,
            yDirection;

        if (transform.position.x > 0)
        {
            xDirection = -1;
        }
        else
        {
            xDirection = 1;
        }

        // calculates the y-direction of a vector based on the positions of the ball and player objects in a 2D space.
        // (ballPos.y - playerPos.y) calculates the vertical distance between the ball and player objects by subtracting the y-coordinate of the player object from the y-coordinate of the ball object.
        // / ballObj.GetComponent<Collider2D>().bounds.size.y divides the above difference by the height of the ball object's collider.
        // The resulting value is assigned to the variable yDirection. This value can be used to determine the direction or magnitude of movement for the ball object, depending on the context of the code.
        // the ball reflects off a paddle. The line can be used to calculate the required y-direction for the ball to bounce off the paddle at different angles based on where it hits.
        yDirection = (ballPos.y - playerPos.y) / ballObj.GetComponent<Collider2D>().bounds.size.y;

        // Dead center fix
        if (yDirection == 0)
        {
            yDirection = 0.25f;
        }

        rb.velocity =
            new Vector2(xDirection, yDirection) * (initialSpeed + (speedIncrease * hitCounter));
    }

    // Callback method called automatically when a collision occurs between two objects with rigidbodies and collider components in a 2D physics simulation. This function is specifically used in Unity's built-in Physics engine for 2D games.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player" || collision.gameObject.name == "AI")
        {
            PlayerBounce(collision.transform);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (transform.position.x > 0)
        {
            ResetBall();
            playerScore.text = (int.Parse(playerScore.text) + 1).ToString();
        }
        else if (transform.position.x < 0)
        {
            ResetBall();
            AIScrore.text = (int.Parse(AIScrore.text) + 1).ToString();
        }
    }
}
