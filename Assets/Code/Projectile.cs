using UnityEngine;

public class Projectile : MonoBehaviour
{
    private int m_remainingBounces = 5;
    private float m_lifeTime = 0.0f;

    [SerializeField]
    private float m_minimumLifeTime = 0.15f;

    public delegate void ProjectileCreatedEvent(GameObject _projectile);
    public static event ProjectileCreatedEvent ProjectileCreated;

    public delegate void ProjectDestroyedEvent(GameObject _projectile);
    public static event ProjectDestroyedEvent ProjectileDestroyed;

    private void Awake()
    {
        Debug.Log(gameObject + " Projectile Created");
    }

    private void Start()
    {
        ProjectileCreated(gameObject);
        UIManager.Instance.SetTxtRemainBounces("Remaining Bounces: " + RemainingBounces.ToString());
    }

    private void Update()
    {
        m_lifeTime += Time.deltaTime;
        PrematureDestroy();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Collidable")
        {
            if (RemainingBounces <= 0)
            {
                ProjectileDestroyed(gameObject);
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Hit collidable object");

                m_remainingBounces--;

                UIManager.Instance.SetTxtRemainBounces("Remaining Bounces: " + RemainingBounces.ToString());

                Debug.Log("Playing projectiel collision sound");
                AudioManager.Instance.Stop("ProjectileBounce");
                AudioManager.Instance.Play("ProjectileBounce");
            }
        }
        else if (collision.collider.tag == "Target")
        {
            ProjectileDestroyed(gameObject);
            Destroy(gameObject);
        }
    }

    public int RemainingBounces
    {
        get
        {
            return m_remainingBounces;
        }

        private set
        {
            m_remainingBounces = value;
        }
    }

    private void PrematureDestroy()
    {
        if (InputManager.Instance.DestroyProjectile && m_lifeTime >= m_minimumLifeTime)
        {
            Debug.Log("Destroy command accepted");
            ProjectileDestroyed(gameObject);
            Destroy(gameObject);
        }
    }
}
