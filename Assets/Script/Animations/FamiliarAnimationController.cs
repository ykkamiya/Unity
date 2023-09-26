using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FamiliarAnimationController : MonoBehaviour
{
    public Animator animShoot = null;
    // Start is called before the first frame update
    void Start()
    {
        animShoot = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 scale = transform.localScale;
        float shootHor = Input.GetAxis("ShootHorizontal");
        float shootVert = Input.GetAxis("ShootVertical");
        if(shootHor > 0)
        {
            scale.x = 1;
            animShoot.SetBool("FaceLeft", false);
            animShoot.SetBool("FaceBack", false);
            animShoot.SetBool("FaceRight", true);
        }
        else if(shootHor < 0)
        {
            scale.x = -1;
            animShoot.SetBool("FaceRight", false);
            animShoot.SetBool("FaceBack", false);
            animShoot.SetBool("FaceLeft", true);
        }
        else if(shootVert > 0)
        {
            animShoot.SetBool("FaceRight", false);
            animShoot.SetBool("FaceLeft", false);
            animShoot.SetBool("FaceBack", true);
        }
        else
        {
            animShoot.SetBool("FaceRight", false);
            animShoot.SetBool("FaceLeft", false);
            animShoot.SetBool("FaceBack", false);
        }
        transform.localScale = scale;
    }
}
