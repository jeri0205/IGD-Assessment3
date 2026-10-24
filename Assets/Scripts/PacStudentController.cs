using UnityEngine;

public class PacStudentController : MonoBehaviour
{

    [SerializeField] private LevelGrid levelGrid;
    [SerializeField] private Animator animator;
    [SerializeField] private float tilesPerSecond = 5f;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip movingSound;
    [SerializeField] private AudioClip eatingSound;
    [SerializeField] private ParticleSystem walkingParticles;
    

    private Vector2Int lastInput;
    private Vector2Int currentInput;
    private bool isLerping;
    private Vector2Int currentCell;
    private Vector2Int targetCell;
    private Vector3 startWorldPosition;
    private Vector3 endWorldPosition;
    private float elapsedTime;
    private float lerpDuration;
    private bool canMove = true;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentCell = levelGrid.worldToGrid(transform.position);
        currentInput = Vector2Int.zero;
        lastInput = Vector2Int.zero;
        animator.speed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            lastInput = new Vector2Int(0, -1);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            lastInput = new Vector2Int(0, 1);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            lastInput = new Vector2Int(-1, 0);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            lastInput = new Vector2Int(1, 0);
        }


        if (isLerping)
        {
            
            if (currentInput == Vector2Int.up)
            {
                animator.SetInteger("Direction", 0);
            }    
            else if (currentInput == Vector2Int.down)
            {
                animator.SetInteger("Direction", 2);
            }   
            else if (currentInput == Vector2Int.left)
            {
                animator.SetInteger("Direction", 3);
            }
            else if (currentInput == Vector2Int.right)
            {
                animator.SetInteger("Direction", 1);
            }
                

            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / lerpDuration);
            transform.position = Vector3.Lerp(startWorldPosition, endWorldPosition, t);
            
            

            if (t >= 1f)
            {
                currentCell = targetCell;
                transform.position = endWorldPosition;
                animator.speed = 0;
                isLerping = false;
            }
        }

        if(canMove && !isLerping)
        {
            if(lastInput == Vector2Int.zero && currentInput == Vector2Int.zero)
            {
                animator.speed = 0;
                audioSource.Stop();
                walkingParticles.Stop();
                return;
            }

            targetCell = lastInput + currentCell;
            if(levelGrid.canEnterPlayer(targetCell))
            {
                currentInput = lastInput;
                playAudio(levelGrid.getTile(targetCell));
                if(!walkingParticles.isPlaying)
                {
                    walkingParticles.Play();
                }
                lerpToTarget();

                animator.speed = 1;
            }

            else
            {
                targetCell = currentInput + currentCell;
                if(levelGrid.canEnterPlayer(targetCell))
                {
                    playAudio(levelGrid.getTile(targetCell));
                    if(!walkingParticles.isPlaying)
                    {
                        walkingParticles.Play();
                    }
                    lerpToTarget();
                    animator.speed = 1;

                }
                else
                {
                    animator.speed = 0;
                    audioSource.Stop();
                    walkingParticles.Stop();
                }
                
            }
        }

        
    }

    void lerpToTarget()
    {
        targetCell = currentCell + currentInput;
        startWorldPosition = transform.position;
        endWorldPosition = levelGrid.gridToWorld(targetCell);
        elapsedTime = 0f;
        lerpDuration = 1f / tilesPerSecond;
        isLerping = true;
    }

    void playAudio(int tile)
    {
        AudioClip clipToPlay = 
        tile == LevelGrid.pellet || tile == LevelGrid.powerPellet ? eatingSound : movingSound;
        if (!audioSource.isPlaying || audioSource.clip != clipToPlay)
        {
            audioSource.clip = clipToPlay;
            audioSource.Play();
        }

    }




}
