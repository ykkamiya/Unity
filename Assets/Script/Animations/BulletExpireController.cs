using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletExpireController : MonoBehaviour
{
    Animator anim = null;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        StartCoroutine(DeathDelay());
    }

    IEnumerator DeathDelay()
    {
        anim.SetTrigger("isDestroy");
        anim.Update(0f);
        var state = anim.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(state.length);
        Destroy(gameObject);
    }
}
