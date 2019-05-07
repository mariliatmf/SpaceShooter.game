using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyController : MonoBehaviour
{
    public float speed;
    public float changeTimer;
    public bool directionSwitch;
    float maxTimer;
    public int hp;
    public bool canShoot;
    public Transform shootingPosition;
    public GameObject bullet;
    float shootTimer;
    float maxShootTimer;
    public float shootPower;
    Rigidbody rig;
    public GameObject particleEffect;
    public MapLimits Limits;
    public int scoreReward;
    public GameObject powerUp;
    public GameObject powerDown;

    // Start is called before the first frame update
    void Start()
    {
        shootTimer = Random.Range(1f,5f);
        maxShootTimer = shootTimer;
        maxTimer = changeTimer;
        rig = GetComponent<Rigidbody>();
        //directionSwitch = false;
    }

    // Update is called once per frame
    void Update()
    {
        switchTimer();
        Movement();
        if(transform.position.x == Limits.maximumX)
        {
            switchDir(directionSwitch);
        }
        if(transform.position.x == Limits.minimumX)
        {
            switchDir(directionSwitch);
        }
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, Limits.minimumX, Limits.maximumX),
            Mathf.Clamp(transform.position.y, Limits.minimumY, Limits.maximumY), 0.0f);

        if (canShoot)
        {
            shootTimer -= Time.deltaTime;
            if (shootTimer <= 0)
            {
                shootingPosition.transform.position = transform.position - new Vector3(0.0f, 0.6f, 0.0f);

                GameObject newBullet = Instantiate(bullet, shootingPosition.position, bullet.transform.rotation);
                //newBullet.GetComponent<Rigidbody>().AddForce(Vector3.up * (-shootPower));

                //GameObject newBullet = Instantiate(bullet, shootingPosition.transform.position, transform.rotation);
                newBullet.GetComponent<Rigidbody>().velocity = Vector3.up * -shootPower;
                shootTimer = maxShootTimer;
            }
        }
    }

    void Movement()
    {
        if (directionSwitch)
        {
            //rig.velocity = Vector3.up * -speed;
            rig.velocity = new Vector3(speed * Time.deltaTime, -speed * Time.deltaTime, 0); //down and right
        }
        else
        {
            rig.velocity = new Vector3(-speed * Time.deltaTime, -speed * Time.deltaTime, 0); //down and left
        }
    }

    void switchTimer()
    {
        changeTimer -= Time.deltaTime;
        if (changeTimer < 0)
        {
            /* if (directionSwitch)
            {
                directionSwitch = false;
            }
            else
            {
                directionSwitch = true;
            } */
            switchDir(directionSwitch);
            changeTimer = maxTimer;
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "enemyBullet")
        {
            return;
        }
        if (col.gameObject.tag == "friendlyBullet")
        {
            Destroy(col.gameObject);
            Instantiate(particleEffect, transform.position, transform.rotation);
            hp--;
            if(hp <= 0)
            {
                int randomNumber = Random.Range(0, 100);
                if(randomNumber < 30)
                {
                    Instantiate(powerUp, transform.position, powerUp.transform.rotation);
                }
                if (randomNumber > 80)
                {
                    Instantiate(powerDown, transform.position, powerDown.transform.rotation);
                }
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCharacter>().score += scoreReward;
                Destroy(gameObject);
            }
        }
        if(col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<PlayerCharacter>().hp--;
            Instantiate(particleEffect, transform.position, transform.rotation);
            hp--;
            if(hp <= 0)
            {
                col.gameObject.GetComponent<PlayerCharacter>().score += scoreReward;
                Destroy(gameObject);
            }
        }
        /*if (col.gameObject.tag == "powerUp")
        {
            switchDir(directionSwitch);
        }
        if (col.gameObject.tag == "powerDown")
        {
            switchDir(directionSwitch);
        }*/
    }

    bool switchDir(bool dir)
    {
        if(dir)
        {
            directionSwitch = false;
        }
        else
        {
            directionSwitch = true;
        }
        return directionSwitch;
    }
}
