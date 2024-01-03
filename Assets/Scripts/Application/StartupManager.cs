using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Application {
    public class StartupManager : MonoBehaviour
    {
        private void Start() {
            SceneManager.LoadScene("Simulation", LoadSceneMode.Single);
        }
    }
}

