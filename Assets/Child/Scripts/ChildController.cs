
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChildController : MonoBehaviour
{
    public ChildState childState;
    public SimulationController simulationController;
    public FaceCheck faceCheck;
    public ChildMovementController movementController;
    public EyeTrackingObjectFocuser eyeTrackingObjectFocuser;
    public ChildAnimationController childAnimationController;
    //Refs for GUI
    public FloatingBar attentionBar, tantrumBar;
    //Behaviors which respond to attention and tantrum level
    public List<AttentionLevelRegion> attentionRegions;
    public List<TantrumRegion> tantrumRegions;
    public bool eyeTrackingUpdatesAttentionLevel = false;


    //boxed breathing UI
    public GameObject boxedbreather;
    public RectTransform panelRectTransform;
    private float boxedbreatherTimer;
    

    private float updateFrequencyPerSecond;

    //Begin the coroutine when this wakes up.

    public GameObject[] focusStatusTextMesh;
    private UserPrefs userPrefs;
    private void Start()
    {
        focusStatusTextMesh = GameObject.FindGameObjectsWithTag("FocusStatus");
        userPrefs = GameObject.Find("UserPrefs").GetComponent<UserPrefs>();
        boxedbreatherTimer = 0;
        updateFrequencyPerSecond = Time.deltaTime;
        simulationController = GameObject.Find("SimulationController").GetComponent<SimulationController>();
    }
    

    void Update()
    {
        
        if(faceCheck.IsFacingPlayer())
        {
            childState.receivingAttention = true;
            updateFocusStatus(userPrefs.IsEnglishSpeaker() ? "You're focusing on the child" : "Est�s prestando atenci�n al ni�o");
        }
        else
        {
            childState.receivingAttention = false;
            updateFocusStatus(userPrefs.IsEnglishSpeaker() ? "You're ignoring the child" : "Est�s ignorando al ni�o");
        }

        //if tantrunlevel > 4 display boxed breather Ui
        if (childState.tantrumLevel > 60)
        {
            boxedbreather.SetActive(true);
            panelRectTransform.offsetMin = new Vector2(360,0); // expand back ground panel
        }

        if (boxedbreather.activeSelf)
            boxedbreatherTimer += Time.deltaTime;

        if (boxedbreatherTimer > 16f && childState.tantrumLevel < 60)
        {
            boxedbreather.SetActive(false);
            panelRectTransform.offsetMin = new Vector2 (360,190); // collapse background panel
            boxedbreatherTimer = 0; //reset timer
        }


        //Integrate attention level into tantrum level
        foreach (var region in attentionRegions)
        {
            //If we are not in this region's bounds, skip it
            if (!isWithinRange(region.activatePercentageLoBound, region.activatePercentageHiBound, childState.attentionLevel))
                continue;

            //Update attention level
            float attentionLevelChangePerSecond = 0;
            if (tatrumchildbehavior.simluationOnGoing) {
                if (childState.receivingAttention)
                {
                    attentionLevelChangePerSecond = region.attentionLevelAddedPerSecond;
                }
                else
                {
                    attentionLevelChangePerSecond = -region.attentionLevelLostPerSecond;
                }
            }
            

            if (isWithinRange(0, 100, childState.attentionLevel + (attentionLevelChangePerSecond * updateFrequencyPerSecond)))
            {
                childState.attentionLevel += attentionLevelChangePerSecond * updateFrequencyPerSecond;
            }
            else
            {
                childState.attentionLevel = (childState.attentionLevel > 50) ? 100 : 0;
            }

            //If the attention level is too low, increase tantrum level
            if (childState.tantrumLevel < 25 && childState.attentionLevel <= 40)
            {
                childState.tantrumLevel += 3 * updateFrequencyPerSecond;
            }
            

            if (childState.receivingAttention)
            {
                if (childState.tantrumLevel > 0 && tatrumchildbehavior.childIsTalking)
                    childState.tantrumLevel += region.tantrumLevelIncreasePerSecond * updateFrequencyPerSecond;
                else if (childState.tantrumLevel > 0 && !tatrumchildbehavior.childIsTalking)
                    childState.tantrumLevel -= region.tantrumLevelDecreasePerSecond * updateFrequencyPerSecond;
            }
            else
            {
                if (childState.tantrumLevel > 0 && tatrumchildbehavior.childIsTalking && !tatrumchildbehavior.negativeStatementSelected)
                    childState.tantrumLevel -= region.tantrumLevelDecreasePerSecond * updateFrequencyPerSecond;
                else if (childState.tantrumLevel > 0 && !tatrumchildbehavior.childIsTalking)
                    childState.tantrumLevel += 1 * updateFrequencyPerSecond;
            }
            

            if (childState.tantrumLevel > 100)
                childState.tantrumLevel = 100;
            else if (childState.tantrumLevel < 0)
                childState.tantrumLevel = 0;

            //Update AttentionBar color
            attentionBar.barImage.color = region.barColor;

            //Do not check other regions
            break;
        }

        //Respond to tantrum level
        foreach (var region in tantrumRegions)
        {
            //If we are not in this region's bounds, skip it
            if (!isWithinRange(region.activatePercentageLoBound, region.activatePercentageHiBound, childState.tantrumLevel))
                continue;

            tantrumBar.barImage.color = region.barColor;

            //Do not check other regions
            break;
        }

        //Update the GUI
        if (attentionBar != null)
            attentionBar.UpdateBar(childState.attentionLevel, 100);
        if (tantrumBar != null)
            tantrumBar.UpdateBar(childState.tantrumLevel, 100);


        // Update the tantrum tracker
       
        if (childState.tantrumLevel == 0 && tatrumchildbehavior.simluationOnGoing) {
            Debug.Log("incrementTantrumTimeAtZero");
            simulationController.incrementTantrumTimeAtZero();
        }
        else if (childState.tantrumLevel > 80 && tatrumchildbehavior.simluationOnGoing) {
            simulationController.incrementTantrumTimeAboveEighty();
        }
    }

    private bool isWithinRange(float lo, float hi, float newValue)
    {
        return (newValue >= lo && newValue <= hi);
    }

    public List<ChildBehaviorState> GetValidBehaviors()
    {
        List<ChildBehaviorState> result = new();

        foreach (var region in tantrumRegions)
        {
            //If we are not in this region's bounds, skip it
            if (!isWithinRange(region.activatePercentageLoBound, region.activatePercentageHiBound, childState.tantrumLevel))
                continue;

            foreach (ChildBehaviorState behavior in region.validBehaviors)
                result.Add(behavior);
        }

        return result;
    }


    public void updateFocusStatus(string message) {
        foreach (GameObject focusStatusLabel in focusStatusTextMesh)
        {
            focusStatusLabel.GetComponent<TextMeshProUGUI>().text = message;
        }
    }
}
