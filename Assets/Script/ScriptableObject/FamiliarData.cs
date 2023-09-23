using UnityEngine;

[CreateAssetMenu(fileName = "Familiar.asset", menuName = "Familiars/FamiliarObject")] //アセット設定項目を定義
public class FamiliarData : ScriptableObject
{
    public string familiarType;

    public float speed;
    public float fireDelay;
    public GameObject bulletPrefab;


}
