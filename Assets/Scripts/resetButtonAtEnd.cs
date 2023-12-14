using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class resetButtonAtEnd : MonoBehaviour
{
    private SceneController sceneController;
    private UserPrefs userPrefs;
    private bool confirmed;
    public TextMeshProUGUI warningMessage;
    // Start is called before the first frame update
    void Start()
    {
        // Find the SceneController and UserPrefs objects and get their scripts
        sceneController = GameObject.Find("SceneController").GetComponent<SceneController>();
        userPrefs = GameObject.Find("UserPrefs").GetComponent<UserPrefs>();
        confirmed = false;
        warningMessage.enabled = false;
    }

    public void resetSimulation()
    {
            // Load the necessary scenes
            sceneController.UnloadScene(Enums.SceneNames.ChildScene);
            sceneController.UnloadScene(Enums.SceneNames.EndSceneWin);
            sceneController.UnloadScene(Enums.SceneNames.EndSceneLose);
            sceneController.UnloadScene(Enums.SceneNames.EndSceneLoseSpanish);
            sceneController.UnloadScene(Enums.SceneNames.EndSceneWinSpanish);
            sceneController.LoadScene(Enums.SceneNames.StartScene);
            sceneController.ToggleBackgroundScene(1); // load Home, unload others


            // Set the defaults
            userPrefs.SetScenario(Enums.Scenarios.Home);
            userPrefs.SetChildAvatar(Enums.ChildAvatars.Hispanic);
            userPrefs.SetLanguage(Enums.Languages.English);


    }
}
