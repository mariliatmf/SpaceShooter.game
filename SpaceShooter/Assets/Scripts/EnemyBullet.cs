using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    float specialTimer = 2f;
    
    void Start()
    {

    }
    
    void Update()
    {
        specialTimer -= Time.deltaTime;
        if (specialTimer <= 0)
        {
            GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyController>().canShoot = true;
            Destroy(gameObject); 
        }
    }
}