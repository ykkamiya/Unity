using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyWalk : MonoBehaviour
{
    private Animator anim = null;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 scale = transform.localScale;
        float horizontal = Input.GetAxis("Horizontal"); //入力 GetAxisは操作設定に設定されているキーの入力を取得する
        float vertical = Input.GetAxis("Vertical");

        if(horizontal < 0)
        {
            if(scale.x > 0)
            {
                scale.x *= -1;
            }
            anim.SetBool("WalkFront", false);  
            anim.SetBool("WalkRight", true);
        }
        else if(horizontal > 0)
        {
            if(scale.x < 0)
            {
                scale.x *= -1;
            }
            anim.SetBool("WalkFront", false);  
            anim.SetBool("WalkLeft", true);
        }
        else if(vertical != 0)
        {
            if(horizontal == 0)
            {
                anim.SetBool("WalkRight", false);
                anim.SetBool("WalkLeft", false);
            }
            anim.SetBool("WalkFront", true);  
        }
        else
        {
            anim.SetBool("WalkRight", false);
            anim.SetBool("WalkLeft", false);
            anim.SetBool("WalkFront", false);
        }
        transform.localScale = scale;
        
    }
}
