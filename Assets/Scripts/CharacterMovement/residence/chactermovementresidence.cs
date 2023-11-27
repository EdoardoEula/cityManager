using UnityEngine;
using System.Collections;

public class CharacterMovementresidence : MonoBehaviour
{
    public Transform[] waypointsresidence;
    public float speed = 3.0f;
    public Transform target;
    public float runSpeed = 5.0f;

    private Animator animator;
    private int currentWaypoint = 0;
    private bool isMoving = false;
    private bool isRunning = false;
    private bool isStanding = true;

    // Additional variable to check if the object should stay still
    private bool shouldStayStill = false;

    void Start()
    {
        // Ottieni il riferimento all'Animator
        animator = GetComponent<Animator>();
        // Inizia con lo standing
        animator.SetBool("isStanding", true);
        animator.SetBool("isRunning", false);
        // Aspetta per un certo periodo (ad esempio, 3 secondi) e poi passa a IsMoving
        StartCoroutine(StartMovingAfterDelay(3f));
    }

    IEnumerator StartMovingAfterDelay(float delay)
    {
        // Aspetta per il periodo specificato
        yield return new WaitForSeconds(delay);

        // Fai diventare IsMoving true
        isMoving = true;
        animator.SetBool("isMoving", true);
    }
    void Update()
    {
        if (isMoving && !shouldStayStill)
        {
            MoveAlongPath();
        }

        // Aggiorna il parametro "IsMoving" nell'Animator
        animator.SetBool("IsMoving", isMoving);
        
        // Aggiorna il parametro "IsStanding" nell'Animator
        animator.SetBool("IsStanding", isStanding);
        // Aggiorna il parametro "IsMoving" nell'Animator
        animator.SetBool("IsMoving", isMoving);

        // Aggiorna il parametro "IsRunning" nell'Animator solo se il personaggio sta correndo
        if (isRunning)
        {
            animator.SetBool("IsRunning", true);
        }
    }

    void MoveAlongPath()
    {
        if (currentWaypoint < waypointsresidence.Length)
        {
            // Move towards the current waypoint
            transform.position = Vector3.MoveTowards(transform.position, waypointsresidence[currentWaypoint].position,
                speed * Time.deltaTime);

            // Check if the character has reached the current waypoint
            if (Vector3.Distance(transform.position, waypointsresidence[currentWaypoint].position) < 0.1f)
            {
                // Check for turns at specific waypoints
                HandleTurns();

                // Move to the next waypoint
                currentWaypoint++;
            }
        }
    }

    void HandleTurns()
    {
        // Check if the character has reached a waypoint that requires a turn
        if (currentWaypoint == 7) // Waypoint12 is at index 11 (0-based)
        {
            // Turn right when reaching Waypoint12
            transform.rotation = Quaternion.Euler(0f, -30f, 0f);
        }
        else if (currentWaypoint == 8) // Waypoint21 is at index 20 (0-based)
        {
            // Turn right when reaching Waypoint21
            transform.rotation = Quaternion.Euler(0f, -20f, 0f);
        }
        else if (currentWaypoint == 9) // Waypoint21 is at index 20 (0-based)
        {
            // Turn right when reaching Waypoint21
            transform.rotation = Quaternion.Euler(0f, 30f, 0f);
        }
        else if (currentWaypoint == 16) // Waypoint21 is at index 20 (0-based)
        {
            // Turn right when reaching Waypoint21
            transform.rotation = Quaternion.Euler(0f, 80f, 0f);
        }
        else if (currentWaypoint == 18) // Waypoint21 is at index 20 (0-based)
        {
            // Turn right when reaching Waypoint21
            transform.rotation = Quaternion.Euler(0f, 90f, 0f);
        }
        else if (currentWaypoint == 20) // Waypoint21 is at index 20 (0-based)
        {
            // Turn right when reaching Waypoint21
            transform.rotation = Quaternion.Euler(0f, 120f, 0f);
        }
        else if (currentWaypoint == 21) // Waypoint21 is at index 20 (0-based)
        {
            // Turn right when reaching Waypoint21
            transform.rotation = Quaternion.Euler(0f, 150f, 0f);
        }
        else if (currentWaypoint == 22) // Waypoint27 is at index 26 (0-based)
        {
            // Turn right when reaching Waypoint27
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
        else if (currentWaypoint == 27) // Waypoint38 is at index 37 (0-based)
        {
            // Turn right when reaching Waypoint38
            transform.rotation = Quaternion.Euler(0f, 120f, 0f); // Reset rotation to face forward

        }
        else if (currentWaypoint == 28) // Waypoint38 is at index 37 (0-based)
        {
            // Turn right when reaching Waypoint38
            transform.rotation = Quaternion.Euler(0f, 100f, 0f); // Reset rotation to face forward
            currentWaypoint = 0;
            transform.rotation = Quaternion.Euler(0f, -75f, 0f); // Reset rotation to face forward
        }
    }
    

    // Method to set whether the object should stay still
    public void SetStayStill(bool shouldStay)
    {
        shouldStayStill = shouldStay;
        // If the character should stay still, stop moving and running
        if (shouldStayStill)
        {
            isMoving = false;
            isRunning = false;
            isStanding = true;
            // Reset the waypoint index to restart the path
            currentWaypoint = 0;
        }
    }
    
    public void CallCharacterresidence()
    {
        // Ferma il personaggio in movimento
        isRunning = true;
        animator.SetBool("isRunning", true);
        // Fai correre il personaggio verso il target (personaggio pulsante)
        StartCoroutine(RunToTarget());
    }

    IEnumerator RunToTarget()
    {
        Vector3 initialPosition = transform.position;
        Vector3 targetPosition = target.position;

        float initialRunSpeed = runSpeed; // Memorizza la velocità iniziale di corsa

        while (Vector3.Distance(transform.position, targetPosition) > 6.5f)
        {
            // Calcola la direzione verso il target
            Vector3 direction = (targetPosition - transform.position).normalized;

            // Aumenta la velocità in base alla distanza rimanente
            runSpeed = Mathf.Lerp(initialRunSpeed, initialRunSpeed * 4f, 1f - Vector3.Distance(transform.position, targetPosition) / initialRunSpeed);
            
            // Ruota gradualmente il personaggio nella direzione del movimento
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f); // Puoi regolare la velocità di rotazione modificando il valore 5f
            
            // Sovrascrivi questa parte del tuo codice
            Ray ray = new Ray(transform.position, direction);
            RaycastHit hit;
            // Modifica il LayerMask per includere solo il layer "Edifici"
            int layerMask = 1 << LayerMask.NameToLayer("Edificio");

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                // Se c'è un ostacolo, calcola una direzione alternativa
                Vector3 obstacleAvoidanceDirection = Vector3.Cross(Vector3.up, direction.normalized);

                // Usa la direzione alternativa per continuare il movimento
                transform.position = Vector3.MoveTowards(transform.position, transform.position + obstacleAvoidanceDirection, runSpeed * Time.deltaTime);
            }
            else
            {
                // Se non ci sono ostacoli, continua con la direzione originale
                transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, runSpeed * Time.deltaTime);
            }


            
            // Muovi il personaggio
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, runSpeed * Time.deltaTime);

            // Aggiorna il parametro "IsMoving" nell'Animator
            animator.SetBool("isMoving", true);

            yield return null;
        }


        // Una volta raggiunto il target, ferma il personaggio
        isMoving = false;
        isRunning = false;
        animator.SetBool("isRunning", false);
        
        // Aggiorna il parametro "IsMoving" nell'Animator
        animator.SetBool("isMoving", false);
        // Ripristina lo standing
        isStanding = true;
    }
}
