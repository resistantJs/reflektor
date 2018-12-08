using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static InputManager m_instance = null;
    private bool m_fire = false;
    private bool m_quit = false;
    private bool m_destroyProjectile = false;
    private Vector3 m_mousePos = Vector3.zero;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    private void Update()
    {
        SetInputs();
    }

    private void SetInputs()
    {
        m_mousePos = Input.mousePosition;

        if (Input.GetButtonDown("Fire1"))
        {
            Fire = true;
        }
        else
        {
            Fire = false;
        }

        if (Input.GetButtonDown("Fire2"))
        {
            DestroyProjectile = true;
        }
        else
        {
            DestroyProjectile = false;
        }

        if (Input.GetButtonDown("Cancel"))
        {
            Quit = true;
        }
        else
        {
            Quit = false;
        }
    }

    public Vector3 MousePos
    {
        get
        {
            return m_mousePos;
        }

        private set
        {
            m_mousePos = value;
        }
    }

    public bool Fire
    {
        get
        {
            return m_fire;
        }

        set
        {
            m_fire = value;
        }
    }

    public bool Quit
    {
        get
        {
            return m_quit;
        }

        set
        {
            m_quit = value;
        }
    }

    public static InputManager Instance
    {
        get
        {
            return m_instance;
        }

        private set
        {
            m_instance = value;
        }
    }

    public bool DestroyProjectile
    {
        get
        {
            return m_destroyProjectile;
        }

        private set
        {
            m_destroyProjectile = value;
        }
    }
}
