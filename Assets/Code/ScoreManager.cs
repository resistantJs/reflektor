using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    private int m_score = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public int Score
    {
        get
        {
            return Score;
        }

        set
        {
            if (value > 0)
            {
                m_score = value;
                UIManager.instance.TxtScore.text = "Score: " + m_score;
            }
        }
    }
}
