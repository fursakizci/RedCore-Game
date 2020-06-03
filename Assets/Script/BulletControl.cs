using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControl : MonoBehaviour
{

    MaceControl mace;
    Rigidbody2D physics;
    void Start()
    {
        mace = GameObject.FindGameObjectWithTag("enemyTag").GetComponent<MaceControl>();
        physics = GetComponent<Rigidbody2D>();
        physics.AddForce(mace.GetDirection() * 1000);
        Destroy(gameObject, 3);
    }

    
}
