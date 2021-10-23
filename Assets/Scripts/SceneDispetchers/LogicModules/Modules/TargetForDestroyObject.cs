using UnityEngine;
using UnityEngine.Events;

public class TargetForDestroyObject : MonoBehaviour, IAlive
{
    [SerializeField]
    private GameObject loot;

    [SerializeField]
    private UnityEvent afterDeadEvent;

    [SerializeField]
    private GameObject afterDeadPrefab;

    [SerializeField, Range(0, 1000)]
    private float maxHealth = 100;

    public float Health { get; set; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Blade"))
        {
            GetDamage(100);
        }
        else if(other.CompareTag("Bullet"))
        {
            GetDamage(other.GetComponent<Bullet>().damage);
        }
    }

    public virtual void DeadAction()
    {
        if(loot != null)
        {
            Vector3 dir = new Vector3(Random.Range(-0.05f, 0.05f), 2, Random.Range(-0.05f, 0.05f));
            Instantiate(loot, transform.position + dir, Quaternion.identity).GetComponent<Rigidbody>()
                .AddForce(dir, ForceMode.Impulse);
        }
        Instantiate(afterDeadPrefab, transform.position, Quaternion.identity).GetComponent<Decal>()?.Init(2);
        afterDeadEvent.Invoke();
        Destroy(gameObject, Time.deltaTime);
    }

    public void GetDamage(float value)
    {
        Health -= value;
        if (Health <= 0)
        {
            DeadAction();
        }
    }

    public void PlusHealth(float value)
    {
        Health += value;
    }

    public void Death() => DeadAction();
}
