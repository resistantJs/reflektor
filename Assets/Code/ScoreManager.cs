using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public GameObject txtScore;

    private int m_score = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void AddToScore(int _amount)
    {
        if (_amount > 0)
        {
            m_score += _amount;
        }

        txtScore.GetComponent<Text>().text = "Score: " + m_score;
        
    }
}
