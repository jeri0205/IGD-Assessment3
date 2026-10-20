using UnityEngine;

public class MovementScript : MonoBehaviour
{
    public AudioSource movementAudio;
    private Vector2[] checkpoints;

    private Animator animator;
    Vector2 currentPosition;
    private int currentCheckpoint = 0;
    Vector2 goalPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        checkpoints = new Vector2[4];
        checkpoints[0] = new Vector2(6f, -1f);
        checkpoints[1] = new Vector2(6f, -5f);
        checkpoints[2] = new Vector2(1f, -5f);
        checkpoints[3] = new Vector2(1f, -1f);
        
        animator = GetComponent<Animator>();
        
        currentPosition = transform.position;
        
        movementAudio.loop = true;
        movementAudio.Play();

    }

    // Update is called once per frame
    void Update()
    {
        currentPosition = transform.position;
        goalPosition = checkpoints[currentCheckpoint];

        Vector2 direction = goalPosition - currentPosition;
        direction = direction.normalized;
        Vector2 newPosition = currentPosition + direction * Time.deltaTime * 2f;

        //When goal position is reached
        if (Vector2.Distance(newPosition, goalPosition) < 0.01f)
        {
            newPosition = goalPosition;
            currentCheckpoint += 1;
            if (currentCheckpoint >= checkpoints.Length)
            {
                currentCheckpoint = 0;
            }
            playAnimation(currentCheckpoint);
        }
        
        transform.position = newPosition;
        
    }

    private void playAnimation(int index)
    {
        if (index == 0)
        {
            animator.Play("gooseWalkRight");
        }
        else if (index == 1)
        {
            animator.Play("gooseWalkDown");
        }
        else if (index == 2)
        {
            animator.Play("gooseWalkLeft");
        }
        else if (index == 3)
        {
            animator.Play("gooseWalkUp");
        }
    }
}
