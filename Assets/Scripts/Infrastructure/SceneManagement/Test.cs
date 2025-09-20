using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Infrastructure.SceneManagement
{
    public class Test : MonoBehaviour
    {
        private void Awake()
        {
            SceneManager.LoadSceneAsync("MainMenu");
        }
    }
}