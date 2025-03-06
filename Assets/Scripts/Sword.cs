using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{

    [SerializeField]
    private Transform Player;

    
    [SerializeField]
    private Vector3 OffsetSword;

    [SerializeField]
    private GameObject boss;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
         transform.position = Player.position + OffsetSword;
    }
}
