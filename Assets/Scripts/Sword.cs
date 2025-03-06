using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{

    [SerializeField]
    private Transform Player;

    
    
    private Vector3 OffsetSword = new Vector3(0f,0f,0f);

    [SerializeField]
    private GameObject boss;

    public void setOffset(Vector3 offset)
    {
        OffsetSword = offset;
        Debug.Log("OffsetSword mis à jour : " + OffsetSword);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        

        Debug.Log(OffsetSword);
        transform.position = Player.position + OffsetSword;
    }


    
}
