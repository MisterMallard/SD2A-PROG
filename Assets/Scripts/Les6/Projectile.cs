using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField, Min(0f)] private float speed = 8f;
    private Transform target;
    [SerializeField] private GameObject hitPS;

    [SerializeField] private AudioClip hitSFX;

    // WRITE ONLY (Tower)
    public Transform Target
    {
        set { target = value; }
    }

    void Update()
    {
        if(target == null){
            Destroy(gameObject);
            return;
        }

        Vector3 dir = (target.position - transform.position).normalized;
        transform.position += dir * speed * Time.deltaTime;

        float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);

        if (Vector2.Distance(transform.position,target.position) < 0.15f){
            Enemy e = target.GetComponent<Enemy>();
            e.Health -= 1;
            if (e.Health <= 0)
            {
                CoinManager.Instance.UpdateCoins(1);
                Destroy(target.gameObject);
            }
            
            Instantiate(hitPS, transform.position, Quaternion.identity);
            AudioManager.Instance.PlaySFX(hitSFX);
            Destroy(gameObject); 
        }
    }
}
