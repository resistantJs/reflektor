﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelect : MonoBehaviour {

	public void LoadSelectedLevel(int _buildIndex)
    {
        GameManager.Instance.LevelSelectedLevel(_buildIndex);
    }
}
