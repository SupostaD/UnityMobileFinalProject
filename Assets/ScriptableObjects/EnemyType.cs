using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/EnemyType")]
public class EnemyType : ScriptableObject
{
    public string enemyName;
    public GameObject prefab; 
    public int health;
    public int speed;
    public int damage;
}
