using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{
    // Start is called before the first frame update
    public float delay = 4.0f; // Delay in seconds before switching scenes

    void Start()
    {
        StartCoroutine(SwitchSceneAfterDelay());
    }

    IEnumerator SwitchSceneAfterDelay()
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        // Load the second scene (scene index 1)
        SceneManager.LoadScene(1);
        UnityEngine.Debug.Log("Current Scene Name: " + SceneManager.GetActiveScene().name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
