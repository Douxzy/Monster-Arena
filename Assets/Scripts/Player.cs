using System;
using System.Collections;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private Vector2 m_Input;

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
    private int HP = 100;


    [SerializeField]
    private TextMeshProUGUI textMeshPro;

    private Vector3 InitializationLocalScale;
    void Start()
    {

        InitializationLocalScale = LifeBar.localScale;

        Sword.SetActive(false);
        // Récupérer le composant Rigidbody2D une seule fois au démarrage
        m_Rigidbody = GetComponent<Rigidbody2D>();
    }

    

    void Update()
    {

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
        
        // Récupérer l'entrée utilisateur
        m_Input.x = Input.GetAxisRaw("Horizontal");
        m_Input.y = Input.GetAxisRaw("Vertical");

        // Animer utilisateur

        animator.SetFloat("Horizontal", m_Input.x);
        animator.SetFloat("Vertical", m_Input.y);
        animator.SetFloat("Speed", m_Input.magnitude);

        // Déplacer le Rigidbody2D à la nouvelle position
        m_Rigidbody.MovePosition((Vector2)m_Rigidbody.position + m_Input * moveSpeed * Time.deltaTime);
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
        Debug.Log("CC");
        animator.SetBool("Attack", true);

        yield return new WaitForSeconds(0.5f);

        animator.SetBool("Attack", false);
        Sword.SetActive(false);

        yield return new WaitForSeconds(0.5f);

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
    IEnumerator GameOverRoutine()
    {
        Debug.Log("Game Over !");
        moveSpeed = 0;
        this.enabled = false;
        
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
        }
    }
}
