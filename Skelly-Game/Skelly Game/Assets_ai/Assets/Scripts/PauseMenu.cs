using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PuseMenue : MonoBehaviour
{
   [SerializeField] GameObject PauseMenu ;


   
   public void Pause()
   {
        PauseMenu.SetActive(true);
        Time.timeScale = 0.0f;
   }

   public void resume()
   {
        PauseMenu.SetActive(false);
        Time.timeScale = 1f;
   }
   public void Back()
   {
        SceneManager.LoadScene(4);
   }
}
