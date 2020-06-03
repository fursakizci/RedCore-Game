using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinContol : MonoBehaviour
{
    public Sprite[] animationSquar;
    SpriteRenderer spriteRenderer;
    float time = 0;
    int animationSquarCount;

   
    void Start()
    {
        

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if (time > 0.03f)
        {
            spriteRenderer.sprite = animationSquar[animationSquarCount++];

            if (animationSquar.Length == animationSquarCount)
            {
                animationSquarCount = 0;
            }
            time = 0;
        }
    }
    

}
