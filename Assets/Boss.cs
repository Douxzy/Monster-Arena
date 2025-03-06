using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss : MonoBehaviour
{
    private Rigidbody2D Rigidbody;
    private AudioSource audioSource;
    
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject warning_circle_generator;
    [SerializeField] private bool isAttacking = false;
    [SerializeField] private bool isDead = false; // Nouvelle variable pour suivre l'état de mort

    [SerializeField] private int HPONE = 500;
    [SerializeField] private int HPTWO = 1500;
    [SerializeField] private int phase = 0;

    [SerializeField] private float minRadius = 2f;
    [SerializeField] private float maxRadius = 5f;
    [SerializeField] private int circleCount = 10;
    [SerializeField] private float attackCooldown = 3f;
 
    [SerializeField] private float DetectionRange = 10f;
    [SerializeField] public float moveSpeed = 20f;
    [SerializeField] private bool canMove = true;

    [SerializeField] public Animator animator;

    void Start()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");

        if (warning_circle_generator == null)
        {
            Debug.LogError("warning_circle_generator n'est pas assigné dans l'inspecteur !");
            return;
        }
    }

    IEnumerator GenerateCircles()
    {
        if (isAttacking || isDead) yield break; // Ne pas attaquer si le boss est mort
        isAttacking = true;

        Debug.Log("Début de l'attaque !");

        for (int i = 0; i < circleCount; i++)
        {
            if (isDead) yield break; // Arrêter la génération si le boss est mort

            Vector2 spawnPosition = GetValidSpawnPosition();

            if (warning_circle_generator != null)
            {
                GameObject warning_circle = Instantiate(warning_circle_generator, spawnPosition, Quaternion.identity);
            }
            else
            {
                Debug.Log("Le prefab du cercle d'avertissement est NULL !");
            }

            yield return new WaitForSeconds(attackCooldown);
        }

        isAttacking = false;
    }

    Vector2 GetValidSpawnPosition()
    {
        for (int attempt = 0; attempt < 10; attempt++)
        {
            Vector2 direction = Random.insideUnitCircle.normalized;
            if (direction == Vector2.zero) continue;

            float distance = Random.Range(minRadius, maxRadius);
            Vector2 randomPosition = (Vector2)transform.position + (direction * distance);
            return randomPosition;
        }

        return (Vector2)transform.position + (Random.insideUnitCircle.normalized * minRadius);
    }

    void Update()
    {
        if (player == null || isDead) return; // Si le boss est mort, on arrête

        Vector2 playerPosition = player.transform.position;
        float distanceToPlayer = Vector2.Distance(transform.position, playerPosition);

        if (distanceToPlayer < maxRadius)
        {
            StartCoroutine(GenerateCircles());
        }
        if (distanceToPlayer < DetectionRange && canMove)
        {
            MoveToPlayer((playerPosition - (Vector2)transform.position).normalized);
        }
    }

    void MoveToPlayer(Vector2 playerDirection)
    {
        if (isDead) return; // Ne pas bouger si le boss est mort
        Rigidbody.MovePosition((Vector2)Rigidbody.position + playerDirection * moveSpeed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Sword") && !isDead) // Ne pas appliquer de dégâts si le boss est mort
        {
            audioSource.Play();
            getHit();
        }
    }

    public void getHit()
    {
        if (isDead) return; // Si le boss est déjà en train de mourir, on ne fait rien

        int playerAtt = player.GetComponent<Player>().GetAttack();

        if (phase == 0)
        {
            if (!willHaveHp(playerAtt, HPONE))
            {
                HPONE = 0;  
                phase += 1;
                attackCooldown = 0.02f;
                circleCount = 30;
                return;
            }
            HPONE -= playerAtt;
        }
        else if (phase == 1)
        {
            if (!willHaveHp(playerAtt, HPTWO))
            {
                HPTWO = 0;
                phase += 1;
                isDead = true; // Marquer le boss comme mort
                StartCoroutine(deathBoss());
                return;
            }
            HPTWO -= playerAtt;
        }
    }

    IEnumerator deathBoss()
    {
        canMove = false;
        isAttacking = false;

        if (Rigidbody != null)
        {
            Rigidbody.velocity = Vector2.zero;
            Rigidbody.isKinematic = true;
        }

        animator.SetBool("Dead", true);
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
        SceneManager.LoadScene("Credit");
    }

    bool willHaveHp(int att, int hp)
    {
        return hp - att > 0;
    }
}