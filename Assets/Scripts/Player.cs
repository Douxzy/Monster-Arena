using System;
using System.Collections;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

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

    IEnumerator AttackDelay()
    {
        Sword.SetActive(true);
        Debug.Log("CC");

        yield return new WaitForSeconds(1);
        
        Sword.SetActive(false);

        yield return new WaitForSeconds(1);


        canUseSword = true;
    }

    void Update()
    {

        if(Input.GetKeyDown(KeyCode.Space)) {
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
    void GameOver()
    {

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