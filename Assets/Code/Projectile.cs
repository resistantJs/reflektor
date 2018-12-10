using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    #region Properties

    private int m_remainingBounces = 5;
    private float m_lifeTime = 0.0f;

    [SerializeField]
    private float m_minimumLifeTime = 0.15f;

    [SerializeField]
    private float m_noBouncesDestroyDelay = 3f;

    #region Events

    public delegate void ProjectileCreatedEvent(GameObject _projectile);
    public static event ProjectileCreatedEvent ProjectileCreated;

    public delegate void ProjectDestroyedEvent(GameObject _projectile);
    public static event ProjectDestroyedEvent ProjectileDestroyed;

    #endregion

    #endregion

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
                DestroyProjectile(m_noBouncesDestroyDelay);
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
            DestroyProjectile(0);
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
            DestroyProjectile(0);
        }
    }

    private void DestroyProjectile(float _delay)
    {
        ProjectileDestroyed(gameObject);

        GetComponent<SphereCollider>().enabled = false;
        GetComponent<MeshRenderer>().enabled = false;

        StartCoroutine(DestroyAfterDelay(_delay));
    }

    private IEnumerator DestroyAfterDelay(float _delay)
    {
        yield return new WaitForSecondsRealtime(_delay);
        Destroy(gameObject);
    }
}
