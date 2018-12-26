using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    private readonly int m_maxRemainingBounces = 5;

    private int m_remainingBounces = 5;
    private float m_lifeTime = 0.0f;

    [SerializeField]
    private float m_minimumLifeTime = 0.15f;

    [SerializeField]
    private float m_noBouncesDestroyDelay = 3f;

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
        UIManager.Instance.SetTxtRemainBounces(RemainingBounces, true);
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

                DecrementBounces();

                TriggerObstacleEffect(collision);

                UIManager.Instance.SetTxtRemainBounces(RemainingBounces, true);

                Debug.Log("Playing projectile collision sound");

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

    private void DecrementBounces()
    {
        m_remainingBounces--;
    }

    private void TriggerObstacleEffect(Collision _collision)
    {
        if (_collision.collider.gameObject.GetComponent(typeof(IObstacle)) as IObstacle != null)
        {
            IObstacle _obstacleScript = _collision.collider.gameObject.GetComponent(typeof(IObstacle)) as IObstacle;

            _obstacleScript.TriggerObstacleEffect(gameObject);

            Debug.Log("Hit object was an IObstacle");
        }
        else
        {
            Debug.Log("Hit object was not an IObstacle");
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

        StartCoroutine(DestroyAfterDelay(_delay));
    }

    private IEnumerator DestroyAfterDelay(float _delay)
    {
        yield return new WaitForSecondsRealtime(_delay);
        Destroy(gameObject);
    }

    public void DestroyProjectileExternal(float _delay)
    {
        DestroyProjectile(_delay);
    }

    public void RestoreBounces(int _amount)
    {
        if (_amount > 0)
        {
            if (!(m_remainingBounces + _amount <= m_remainingBounces))
            {
                m_remainingBounces = m_maxRemainingBounces;
            }
        }
    }
}
