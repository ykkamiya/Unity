using UnityEngine;

[CreateAssetMenu(fileName = "Spawner.asset", menuName = "Spawners/Spawner")]
public class SpawnerData : ScriptableObject //クラスの設定
{
    public GameObject itemToSpawn; //GameObject型を設定->prefabとかを適用できる
    public int minSpawn; //最小スポーン数
    public int maxSpawn; //最大
}
