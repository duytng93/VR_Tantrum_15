using UnityEngine;

public class SliderFollowCamera : MonoBehaviour
{
    private Transform mainCameraTrans;
    private GameObject[] attention_tantrum_Canvas;
    private GameObject speakPanel;
    private GameObject playerObject;
    private GameObject winOrLoseCanvas;
    // Start is called before the first frame update
    void Start()
    {
        mainCameraTrans = GameObject.Find("Main Camera").transform;
        attention_tantrum_Canvas = GameObject.FindGameObjectsWithTag("ChildTantrumAttentionCanvas");
        speakPanel = GameObject.Find("SpeakPanel");
        playerObject = GameObject.Find("PlayerObject");
        winOrLoseCanvas = GameObject.Find("WinOrLoseCanvas");
    }

    // Update is called once per frame
    void LateUpdate()
    {

        if (tatrumchildbehavior.simluationOnGoing)
        {
            foreach (GameObject canvas in attention_tantrum_Canvas)
            {
                canvas.transform.position = mainCameraTrans.position + mainCameraTrans.rotation * new Vector3(-0.7f, 0, 2) + new Vector3(0, 0.2f, 0.3f);
                canvas.transform.rotation = Quaternion.LookRotation(canvas.transform.position - mainCameraTrans.position);
                //prevent the meters canvas from going into the floor
                if (canvas.transform.position.y < -0.5f)
                    canvas.transform.position = new Vector3(canvas.transform.position.x, -0.5f, canvas.transform.position.z);
                //prevent the meters canvas from going into the ceiling
                else if (canvas.transform.position.y > 0.5f)
                    canvas.transform.position = new Vector3(canvas.transform.position.x, 0.5f, canvas.transform.position.z);
            }

            speakPanel.transform.position = mainCameraTrans.position + mainCameraTrans.rotation * new Vector3(0.6f, 0, 2) + new Vector3(0, -0.5f, 0.3f);
            speakPanel.transform.rotation = Quaternion.LookRotation(speakPanel.transform.position - (playerObject.transform.position + mainCameraTrans.position) / 2f);
            // prevent the speak panel from going into the floor
            if (speakPanel.transform.position.y < -1.1f)
                speakPanel.transform.position = new Vector3(speakPanel.transform.position.x, -1.1f, speakPanel.transform.position.z);
            // prevent the speak panel from going into the ceiling
            else if (speakPanel.transform.position.y > 0.1f)
                speakPanel.transform.position = new Vector3(speakPanel.transform.position.x, 0.1f, speakPanel.transform.position.z);

            //hide the winorlose status when the simulation is on going
            winOrLoseCanvas.transform.position = new Vector3(0f, 3, 0);
            
        }
        else { // hide the canvases and speak panel, show winorlose at the end
            foreach (GameObject canvas in attention_tantrum_Canvas)
            {
                canvas.transform.position = new Vector3(0f, 3, 0);
            }
            speakPanel.transform.position = new Vector3(0f, 3, 0);
            
            winOrLoseCanvas.transform.position = mainCameraTrans.position + mainCameraTrans.rotation * new Vector3(0f, 0, 2) + new Vector3(0, 0.2f, 0f);
            winOrLoseCanvas.transform.rotation = Quaternion.LookRotation(winOrLoseCanvas.transform.position - mainCameraTrans.position);
            
        }
    }
}
