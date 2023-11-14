using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;//シーン切り替えに使用するライブラリ

public class SceneChange200 : MonoBehaviour
{
   public void OnClick(){
           SceneManager.LoadScene("Select", LoadSceneMode.Single); 
       } 

}