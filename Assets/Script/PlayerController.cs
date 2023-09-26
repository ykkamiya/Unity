using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed;
    Rigidbody2D rigidbody;
    public Text collectedText; //文字列変数
    public static int collectedAmount = 0; //所持アイテム数

    public GameObject bulletPrefab;
    public float bulletSpeed;
    private float lastFire;
    public float fireDelay;  

    GameObject Head;
    HeadAnimationController HeadAnim;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        Head = transform.Find("Isaac_Head").gameObject;
        HeadAnim = Head.GetComponent<HeadAnimationController>();
    }

    // Update is called once per frame
    void Update()
    {
        //GameControllerからステータスを参照
        fireDelay = GameController.FireRate;
        speed = GameController.MoveSpeed;

        float horizontal = Input.GetAxis("Horizontal"); //入力 GetAxisは操作設定に設定されているキーの入力を取得する
        float vertical = Input.GetAxis("Vertical");

        float shootHor = Input.GetAxis("ShootHorizontal");
        float shootVert = Input.GetAxis("ShootVertical");
        if((shootHor != 0 || shootVert != 0) && Time.time > lastFire + fireDelay)
        {
            Shoot(shootHor, shootVert);
            lastFire = Time.time; //このフレーム実行時の時刻(time.time)
        }        

        rigidbody.velocity = new Vector3(horizontal * speed, vertical * speed, 0);
        collectedText.text = "Items Collected: " + collectedAmount;
    }


    void Shoot(float x, float y) //弾発射を関数定義
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation) as GameObject; //弾オブジェクトを定義
        bullet.AddComponent<Rigidbody2D>().gravityScale = 0;
        bullet.GetComponent<Rigidbody2D>().velocity = new Vector3(
            (x < 0) ? Mathf.Floor(x) * bulletSpeed : Mathf.Ceil(x) * bulletSpeed,
            (y < 0) ? Mathf.Floor(y) * bulletSpeed : Mathf.Ceil(y) * bulletSpeed,
            0
        );
        HeadAnim.animShoot.SetTrigger("Shooting");
    }
}
