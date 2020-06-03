using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class CaracterControl : MonoBehaviour
{
    public Sprite[] waitingAnimation;
    public Sprite[] jumpingAnimation;
    public Sprite[] runningAnimation;
    float horizontal = 0;
    float standbyTime;
    bool jumpOnce = true;

    int waitingAnimationCount = 0;  
    int runningAnimationCount = 0;

    SpriteRenderer spriteRenderer;

    Vector3 vec;

    Rigidbody2D physics;
    Vector3 cameraFirstPosition;
    Vector3 cameraEndPosition;

    GameObject camera;

    public Text healthText;
    int health = 20;

    public Image deadBlackBackground;
    public Image CoinImage;
    float blackBackgroundCounter = 0;
    float returnTimeToMainMenu = 0;

    public Text coinText;

    int coinCount = 0;


    public AudioSource collectCoin;
    
    void Start()
    {

        collectCoin = GetComponent<AudioSource>();
        


        PlayerPrefs.SetInt("whichLevel", SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
        physics = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        camera = GameObject.FindGameObjectWithTag("MainCamera");
        healthText.text = "HEALTH  " + health;
        coinText.text = "X  " + coinCount + " - 20";
        cameraFirstPosition = camera.transform.position;

       

        if(SceneManager.GetActiveScene().buildIndex > PlayerPrefs.GetInt("whichLevel"))
        {
            PlayerPrefs.SetInt("whichLevel", SceneManager.GetActiveScene().buildIndex);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (jumpOnce)
            {
                vec = new Vector3(horizontal * 10, 0, 0);
                physics.velocity = vec;
                physics.AddForce(new Vector2(0, 500));
                jumpOnce = false;
            }

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        jumpOnce = true;

        if (collision.gameObject.CompareTag("GrassPlatformTag"))
        {
            transform.SetParent(collision.transform);
        }
        
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("GrassPlatformTag"))
        {
            transform.SetParent(null);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "enemyTag")
        {
            health -= 15;
            healthText.text = "HEALTH  " + health;
        }
        else if (collision.gameObject.tag == "SawTag")
        {
            health -= 10;
            healthText.text = "HEALTH  " + health;
        }
        else if (collision.gameObject.tag == "BulletTag")
        {
            health--;
            healthText.text = "HEALTH  " + health;
        }
        else if (collision.gameObject.tag == "FinishLevelTag")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else if (collision.gameObject.tag == "ChestTag")
        {
            
            health += 10;
            if(health > 20)
            {
                health = 20;
            }
            
                healthText.text = "HEALTH  " + health;
                collision.GetComponent<BoxCollider2D>().enabled = false;
                collision.GetComponent<ChestController>().enabled = true;
            
            
            
        }
        else if (collision.gameObject.tag == "CoinTag")
        {
            collectCoin.Play();
            coinCount++;
            Destroy(collision.gameObject);
            coinText.text = "X  " + coinCount + " - 20";
        }
        else if (collision.gameObject.tag == "WaterTag")
        {
            health = 0;
        }
        else if (collision.gameObject.tag == "TrapTag")
        {
            health--;
            healthText.text = "HEALTH  " + health;

        }
    }

    void FixedUpdate()
    {
        MoveCharacter();
        Animation();

        if(health <= 0)
        {
            Time.timeScale = 0.4f; //agir cekim
            healthText.enabled = false;
            CoinImage.enabled = false;
            coinText.enabled = false;
            blackBackgroundCounter += 0.03f;
            deadBlackBackground.color = new Color(0, 0, 0, blackBackgroundCounter) ;
            returnTimeToMainMenu += Time.deltaTime;
            if(returnTimeToMainMenu > 1)
            {
                SceneManager.LoadScene("MainMenu");
            }
        }
    }
    private void LateUpdate()
    {
        CameraControl();
    }

    void MoveCharacter()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vec = new Vector3(horizontal * 10, physics.velocity.y, 0);
        physics.velocity = vec;

    }

    void CameraControl()
    {
        cameraEndPosition = cameraFirstPosition + transform.position;
        camera.transform.position = Vector3.Lerp(camera.transform.position,cameraEndPosition,0.07f);
    }


    void Animation()
    {
        if (jumpOnce) {
            if (horizontal == 0)
            {
                standbyTime += Time.deltaTime;
                if (standbyTime > 0.05f)
                {

                    spriteRenderer.sprite = waitingAnimation[waitingAnimationCount++];

                    if (waitingAnimationCount == waitingAnimation.Length)
                    {
                        waitingAnimationCount = 0;
                    }
                    standbyTime = 0;
                }
            }
            else if (horizontal > 0)
            {

                standbyTime += Time.deltaTime;
                if (standbyTime > 0.01f)
                {

                    spriteRenderer.sprite = runningAnimation[runningAnimationCount++];

                    if (runningAnimationCount == runningAnimation.Length)
                    {
                        runningAnimationCount = 0;
                    }
                    standbyTime = 0;
                }
                transform.localScale = new Vector3(1, 1, 1);
            }

            else if (horizontal < 0)
            {
                standbyTime += Time.deltaTime;
                if (standbyTime > 0.01f)
                {

                    spriteRenderer.sprite = runningAnimation[runningAnimationCount++];

                    if (runningAnimationCount == runningAnimation.Length)
                    {
                        runningAnimationCount = 0;
                    }
                    standbyTime = 0;
                }
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }
        else
        {
            if (physics.velocity.y > 0)
            {
                spriteRenderer.sprite = jumpingAnimation[0];

            }
            else if(physics.velocity.y<0)
            {
                spriteRenderer.sprite = jumpingAnimation[1];
            }
            if (horizontal > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else if(horizontal < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }

        }
    

    }
}


    //void moveAnimationCharacter(Sprite[] animationArray, float moveStandByTime,int animationCount )
    //{
    //    standbyTime += Time.deltaTime;
    //    if (standbyTime > moveStandByTime)
    //    {

    //        spriteRenderer.sprite = animationArray[animationCount++];
    //        if(animationArray.Length == animationCount)
    //        {
    //            animationCount = 0;
    //        }
          
    //        standbyTime = 0;
    //    }
    //}


