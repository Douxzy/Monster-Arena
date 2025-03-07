using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private AudioSource audioSource;
    private Vector2 m_Input;
    private bool isActive = false;

    public Animator animator;
    [SerializeField]
    private float moveSpeed = 5f; // Vitesse de déplacement

    private Rigidbody2D m_Rigidbody;

    [SerializeField]
    private GameObject Sword;

    private bool canUseSword = true;

    [SerializeField]
    private Transform LifeBar;

    [SerializeField]
    private int att  = 100;

    [SerializeField]
    private int HP = 100;


    [SerializeField]
    private TextMeshProUGUI textMeshPro;

    [SerializeField]
    private AudioSource Death;

    private Vector3 InitializationLocalScale;
    void Start()
    {
        transform.position = new Vector2(0.1f, -6f);
        audioSource = GetComponent<AudioSource>();
        InitializationLocalScale = LifeBar.localScale;

        Sword.SetActive(false);
        // Récupérer le composant Rigidbody2D une seule fois au démarrage
        m_Rigidbody = GetComponent<Rigidbody2D>();
    }

    

    void Update()
    {
        // Récupérer l'entrée utilisateur
        m_Input.x = Input.GetAxisRaw("Horizontal");
        m_Input.y = Input.GetAxisRaw("Vertical");

        // Animer utilisateur

        animator.SetFloat("Horizontal", m_Input.x);
        animator.SetFloat("Vertical", m_Input.y);
        animator.SetFloat("Speed", m_Input.magnitude);

        // Déplacer le Rigidbody2D à la nouvelle position
        m_Rigidbody.MovePosition((Vector2)m_Rigidbody.position + m_Input * moveSpeed * Time.deltaTime);
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (canUseSword)
            {
                canUseSword = false;
                StartCoroutine(AttackDelay());
            }
        }

        // Vérifier les touches pour modifier l'offset en temps réel
        Vector3 swordOffset = new Vector3(0f, 0f, 0f);
        if (Input.GetKeyDown(KeyCode.D))
        {
            swordOffset.x = 0.6f;
            swordOffset.y = 0f;
            UpdateSwordOffset(swordOffset);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            swordOffset.x = -0.6f;
            swordOffset.y = 0f;
            UpdateSwordOffset(swordOffset);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            swordOffset.x = 0f;
            swordOffset.y = -0.6f;
            UpdateSwordOffset(swordOffset);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            swordOffset.x = 0f;
            swordOffset.y = 0.6f;
            UpdateSwordOffset(swordOffset);
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            if(canUseSword) {
                canUseSword = false;
                StartCoroutine(AttackDelay());
            }
        }
    }

    void UpdateSwordOffset(Vector3 offset)
    {
        Sword swordScript = Sword.GetComponent<Sword>();
        if (swordScript != null)
        {
            swordScript.setOffset(offset);
        }
    }

    IEnumerator AttackDelay()
    {
        Sword.SetActive(true);
        animator.SetTrigger("Attack");
        audioSource.Play();

        yield return new WaitForSeconds(0.1f);

        Sword.SetActive(false);

        yield return new WaitForSeconds(0.1f);

        canUseSword = true;
    }


    void SetHp(int damage)
    {
        int predictHp = this.HP - damage;

        if (predictHp <= 0)
        {
            this.HP = 0;
        }
        else
        {
            this.HP = predictHp;
        }
        Vector3 scale = LifeBar.localScale;
        scale.x = InitializationLocalScale.x * HP / 100;
        LifeBar.localScale = scale;
        textMeshPro.text = HP.ToString();
        if(HP == 0)
        {
            GameOver();
        }
    }
    public int GetAttack()
    {
        return this.att;
    }

    IEnumerator GameOverRoutine()
    {
        Debug.Log("Game Over !");
        moveSpeed = 0;
        this.enabled = false;

        if (!isActive) {
            Death.Play();
            isActive = true;
        }

        animator.SetTrigger("IsDead");

        if (m_Rigidbody != null)
        {
            m_Rigidbody.velocity = Vector2.zero;
            m_Rigidbody.isKinematic = true;
        }

        yield return new WaitForSeconds(2f); // Attendre 2 secondes avant de charger la scène
        
        SceneManager.LoadScene("GameOver");
    }

    void GameOver()
    {
        StartCoroutine(GameOverRoutine());
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collision détectée avec : " + other.gameObject.name);

        if (other.CompareTag("attackCircle")) // Remplace par le bon Tag
        {
            Debug.Log("Le joueur a été touché par une attaque !");

            circle_attack attackScript = other.GetComponent<circle_attack>();

            if (attackScript != null)
            {
                SetHp(attackScript.GetDamage());
                Debug.Log("Le joueur a perdu " + attackScript.GetDamage() + " HP.");
            }
            else
            {
                Debug.LogError("⚠️ Aucun script circle_attack trouvé sur " + other.gameObject.name);
            }
        } else if (other.CompareTag("bigAttack")) // Remplace par le bon Tag
        {
            Debug.Log("Le joueur a été touché par une attaque !");

            BigAttack BigAttackScript = other.GetComponent<BigAttack>();

            if (BigAttackScript != null)
            {
                SetHp(BigAttackScript.GetDamage());
                Debug.Log("Le joueur a perdu " + BigAttackScript.GetDamage() + " HP.");
            }
            else
            {
                Debug.LogError("⚠️ Aucun script BigAttack trouvé sur " + other.gameObject.name);
            }
        }
    }
}
