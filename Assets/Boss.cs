using System.Collections;
using UnityEngine;

public class Boss : MonoBehaviour
{
    private Rigidbody2D Rigidbody;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject warning_circle_generator; // Assigner dans l'inspecteur !
    [SerializeField] private bool isAttacking = false;
    [SerializeField] private float att = 4f; // Dégâts infligés par l'attaque
    [SerializeField] private float minRadius = 1f; // Distance minimum du boss
    [SerializeField] private float maxRadius = 5f; // Distance maximum du boss
    [SerializeField] private int circleCount = 20;
    [SerializeField] private float attackCooldown = 0.5f;
    [SerializeField] private float BaseAttackCooldown = 2f;
    [SerializeField] private float BaseAttackRange = 2f; // Distance à laquelle le boss peut attaquer le joueur
    [SerializeField] private float DetectionRange = 10f;
    [SerializeField] public float moveSpeed = 20f;
    [SerializeField] private bool canMove = true;
    [SerializeField] private bool canAttack = true; // Pour gérer le cooldown entre deux attaques
    [SerializeField] private float warningCircleLifetime = 20f;
    [SerializeField] private float attackCircleLifetime = 4f;
    [SerializeField] private float attackCircleAttack = 10f;

    void Start()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player"); // Trouve le joueur si non assigné

        if (warning_circle_generator == null)
        {
            Debug.LogError("warning_circle_generator n'est pas assigné dans l'inspecteur !");
            return; // Arrêter si le prefab n'est pas assigné
        }
    }

    IEnumerator GenerateCircles()
{
    if (isAttacking) yield break;
    isAttacking = true;

    Debug.Log("Début de l'attaque !");
    
    for (int i = 0; i < circleCount; i++)
    {
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

    isAttacking = false; // ✅ Maintenant bien placé, après la boucle for
}


 Vector2 GetValidSpawnPosition()
{
    for (int attempt = 0; attempt < 10; attempt++) // Essayer 10 fois de trouver une position valide
    {

        
        Vector2 direction = Random.insideUnitCircle.normalized; // Générer une direction aléatoire

        if (direction == Vector2.zero) // Éviter le cas (0,0)
            continue; 

        float distance = Random.Range(minRadius, maxRadius); // Sélectionner une distance aléatoire
        Vector2 randomPosition = (Vector2)transform.position + (direction * distance); // Calcul de la position

        return randomPosition; // Retourner la première position valide trouvée
    }

    // Si après 10 tentatives aucune position correcte n'a été trouvée, en générer une dernière de secours
    return (Vector2)transform.position + (Random.insideUnitCircle.normalized * minRadius);
}

    void Update()
    {
        if (player == null) return; // Si le joueur n'existe pas, on arrête

        // Vérifier si le joueur est à portée d'attaque
        Vector2 playerPosition = player.transform.position;
        Vector2 playerDirection = (playerPosition - (Vector2)transform.position).normalized;
        // Faire tourner le boss vers le joueur
        float angle = Mathf.Atan2(playerDirection.y, playerDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        float distanceToPlayer = Vector2.Distance(transform.position, playerPosition); // Utilisation de player.transform.position

        if (distanceToPlayer < maxRadius){
            StartCoroutine(GenerateCircles());
        }
        if (distanceToPlayer < BaseAttackRange)
        {
            AttackPlayer();
        }
        if(distanceToPlayer < DetectionRange && canMove ){
            MoveToPlayer(playerDirection);
        }

    }
    void MoveToPlayer(Vector2 playerDirection)
    {
        Rigidbody.MovePosition((Vector2)Rigidbody.position +playerDirection * moveSpeed * Time.deltaTime);
    }
    void AttackPlayer()
    {
        // Infliger des dégâts au joueur

        Debug.Log("Le boss a infligé " + att + " dégâts au joueur.");

        // Démarrer le cooldown pour éviter les attaques trop fréquentes
        StartCoroutine(AttackCooldown());
    }

    IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(BaseAttackCooldown);
        canAttack = true;
    }
}


