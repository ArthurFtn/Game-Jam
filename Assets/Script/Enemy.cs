using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 1f;
    public int health = 100;
    
    public virtual void Move()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void Update()
    {
        Move();
    }
}
