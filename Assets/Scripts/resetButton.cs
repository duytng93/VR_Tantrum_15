using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class resetButton : MonoBehaviour
{
    private SceneController sceneController;
    private UserPrefs userPrefs;
    private Boolean confirmed;
    public TextMeshProUGUI warningMessage;
    public SimulationController simControl;
    // Start is called before the first frame update

    private void Start()
    {
        // Find the SceneController and UserPrefs objects and get their scripts
        sceneController = GameObject.Find("SceneController").GetComponent<SceneController>();
        userPrefs = GameObject.Find("UserPrefs").GetComponent<UserPrefs>();
        confirmed = false;
        warningMessage.enabled = false;
        
    }
    // Update is called once per frame
    public void resetSimulation()
    {
        if (!confirmed)
        {
            confirmed = true;
            warningMessage.enabled = true;
            StartCoroutine(resetConfirmAfter3s());
        }
        else {
           
            if(simControl != null)
                simControl.resetTimer();

            // Load the necessary scenes
            //SceneManager.LoadScene("ChildScene");
            sceneController.UnloadScene(Enums.SceneNames.ChildScene);
            sceneController.LoadScene(Enums.SceneNames.ChildScene);
            /*sceneController.UnloadScene(Enums.SceneNames.EndSceneWin);
            sceneController.UnloadScene(Enums.SceneNames.EndSceneLose);
            sceneController.UnloadScene(Enums.SceneNames.EndSceneLoseSpanish);
            sceneController.UnloadScene(Enums.SceneNames.EndSceneWinSpanish);*/
            //sceneController.LoadScene(Enums.SceneNames.StartScene);
            //sceneController.ToggleBackgroundScene(1); // load Home, unload others


            // Set the defaults
            /*userPrefs.SetScenario(Enums.Scenarios.Home);
            userPrefs.SetChildAvatar(Enums.ChildAvatars.Hispanic);
            userPrefs.SetLanguage(Enums.Languages.English);*/

            
        }
        
    }

    IEnumerator resetConfirmAfter3s() {
        yield return new WaitForSeconds(3f);
        confirmed = false;
        warningMessage.enabled = false;
    }

    
}
