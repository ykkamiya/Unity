using UnityEngine;

[System.Serializable] //unityのインスペクターになんか追加するらしい
public class Item
{
    public string name;
    public string description;
    public Sprite itemImage;
}

public class CollectionController : MonoBehaviour
{
    //ステータス変更要素
    public Item item;
    public float healthChange;
    public float moveSpeedChange;
    public float attackSpeedChange;
    public float bulletSizeChange;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = item.itemImage; //スプライトを取得、画像データを代入
        Destroy(GetComponent<PolygonCollider2D>()); //要素を破壊
        gameObject.AddComponent<PolygonCollider2D>(); //アイテムを取得した要素にcomponentを追加
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
            
        if(collision.tag == "Player") //タグを忘れるな！！！！
        {
            PlayerController.collectedAmount++;
            //ステータス変更変数を用いてGameControllerの動作を呼び出す
            GameController.HealPlayer(healthChange); 
            GameController.MoveSpeedChange(moveSpeedChange);
            GameController.FireRateChange(attackSpeedChange);
            GameController.BulletSizeChange(bulletSizeChange);
            Destroy(gameObject);
        }
    }

}
