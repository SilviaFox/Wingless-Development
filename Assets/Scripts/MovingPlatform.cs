using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    
    int atPosition = 2;
    Rigidbody2D rb2d;

    [SerializeField] Transform position1;
    [SerializeField] Transform position2;

    [SerializeField] float platformSpeed;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (atPosition)
        {
            case 1:
                MoveToPos2();
            break;
            case 2:
                MoveToPos1();
            break;
        }
    }

    void MoveToPos1()
    {
        // Step consistent each frame
        float step = platformSpeed * Time.deltaTime;

        // Move platform smoothly to where its meant to be
        rb2d.MovePosition(Vector2.MoveTowards(transform.position, position1.position, step));

        if (RoundPos(transform.position) == RoundPos(position1.position))
            atPosition = 1;

    }

    void MoveToPos2()
    {
        // Step consistent each frame
        float step = platformSpeed * Time.deltaTime;

        // Move platform smoothly to where its meant to be
        rb2d.MovePosition(Vector2.MoveTowards(transform.position, position2.position, step));

        if (RoundPos(transform.position) == RoundPos(position2.position))
            atPosition = 2;

    }

    Vector2 RoundPos(Vector2 input) // get an input Vector 2
    {
        Vector2 output = new Vector2(Mathf.Round(input.x * 10), Mathf.Round(input.y * 10)); // Round input

        return output; // Output
    }
}
