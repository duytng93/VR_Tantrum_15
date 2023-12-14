using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class button1behavior : MonoBehaviour
{
    private UserPrefs userPrefs;

    public Button yourButton;
    public AudioSource audioSource;
    public AudioClip Thankyouforstayingcalm;
    public AudioClip Thankyoufortryingtoquietdown;
    public AudioClip Thankyoufortakingdeepbreath;
    public AudioClip Thankyoufortryingtocalmdown;
    public AudioClip Thankyouforkeepingyourarmsandfeettoyourself;

    public AudioClip Graciasporquedartecalmado;
    public AudioClip Graciasporintentarcalmarse;
    public AudioClip Permanezcatranquilo;
    public AudioClip Graciaspormantenertusbrazosypiesatuslados;
    public AudioClip Graciasporintentarrespiraprofundo;

    public AudioClip StartVoice;
    public AudioClip StartVoiceSpanish;

    private ChildState childState;
    public TextMeshProUGUI tmpText;


    public int previousTantrumLevel; // to keep track of change in tantrumLevel
    public float tantrumLevelInV2;
    public int tantrumLevel;
    public float amount;
    private bool atStart;
    private float introTimer;
    private float introLength;
    public static bool adultIsSpeaking;

    void Start()
    {
        userPrefs = GameObject.Find("UserPrefs").GetComponent<UserPrefs>();
        tmpText = yourButton.GetComponentInChildren<TextMeshProUGUI>();
        
        if (tmpText == null)
        {
            Debug.LogError("TextMeshProUGUI component not found on the button's children.");
        }

        

        switch (userPrefs.GetChildAvatar()) {
            case Enums.ChildAvatars.Hispanic:
                childState = GameObject.Find("TKBoyA").GetComponent<ChildController>().childState;
                break;
            case Enums.ChildAvatars.Black:
                childState = GameObject.Find("TKBoyB").GetComponent<ChildController>().childState;
                break;
            case Enums.ChildAvatars.White:
                childState = GameObject.Find("TKBoyC").GetComponent<ChildController>().childState;
                break;
            case Enums.ChildAvatars.Asian:
                childState = GameObject.Find("TKBoyD").GetComponent<ChildController>().childState;
                break;
        }

        tantrumLevelInV2 = childState.tantrumLevel;
        tantrumLevel = Mathf.CeilToInt(tantrumLevelInV2 / 20);
        UpdateTextAndAudioClip(tantrumLevel); // Initial setup of text and audio clip
        yourButton.onClick.AddListener(OnButtonClick);
        introTimer = 0f;
        introLength = userPrefs.IsEnglishSpeaker() ? StartVoice.length : StartVoiceSpanish.length;
        atStart = true;
    }

    void Update()
    {
        if (atStart && tatrumchildbehavior.childIsTalking)
            introTimer += Time.deltaTime;
        tantrumLevelInV2 = childState.tantrumLevel;
        tantrumLevel = Mathf.CeilToInt(tantrumLevelInV2 / 20);


        if (atStart && introTimer < introLength || adultIsSpeaking || childState.tantrumLevel == 0 || tatrumchildbehavior.gameLost)  // if we at start the child is saying the greeting -> disable the button
        {                                                                        // if the player've just clicked this button and the audio is play -> disable button so they can't click it again
            yourButton.interactable = false;
            //tmpText.text = "";
        }
        else if (!adultIsSpeaking)
        { // let the parent finish talking first, otherwise their audio is cut off during tantrum level change
            UpdateTextAndAudioClip(tantrumLevel);
            yourButton.interactable = true;
            atStart = false;
            
        }

    }

    void UpdateTextAndAudioClip(int tantrumLevel)
    {
        //if (!true)
        //{

        //    switch (tantrumLevel)
        //    {
        //        case 0:
        //            tmpText.text = "Gracias por quedarte calmado";
        //            audioSource.clip = Graciasporquedartecalmado;
        //            amount = -10.0f;
        //            break;
        //        case 1:
        //            tmpText.text = "Gracias por intentar calmarse";
        //            audioSource.clip = Graciasporintentarcalmarse;
        //            amount = -10.0f;
        //            break;
        //        case 2:
        //            tmpText.text = "Gracias por intentar calmarse";
        //            audioSource.clip = Graciasporintentarcalmarse;
        //            amount = -10.0f;
        //            break;
        //        case 3:
        //            tmpText.text = "Permanezca tranquilo";
        //            audioSource.clip = Permanezcatranquilo;
        //            amount = 0.0f;
        //            break;
        //        case 4:
        //            tmpText.text = "Gracias por intentar calmarse";
        //            audioSource.clip = Graciasporintentarcalmarse;
        //            amount = -10.0f;
        //            break;
        //        case 5:
        //            tmpText.text = "Gracias por mantener tus brazos y pies a tus lados";
        //            audioSource.clip = Graciaspormantenertusbrazosypiesatuslados;
        //            amount = -10.0f;
        //            break;
        //        default:
        //            tmpText.text = "";
        //            audioSource.clip = null;
        //            amount = 0.0f;
        //            break;
        //    }
        //}

        //if (false)
        {
            switch (tantrumLevel)
            {
                case 0:
                    tmpText.text = userPrefs.IsEnglishSpeaker() ? "Thank you for staying calm" : "Gracias por quedarte calmado";
                    audioSource.clip = userPrefs.IsEnglishSpeaker() ? Thankyouforstayingcalm : Graciasporquedartecalmado;
                    amount = -10.0f;
                    break;
                case 1:
                    tmpText.text = userPrefs.IsEnglishSpeaker() ? "Thank you for trying to quiet down" : "Gracias por intentar calmarse";
                    audioSource.clip = userPrefs.IsEnglishSpeaker() ? Thankyoufortryingtoquietdown : Graciasporintentarcalmarse;
                    amount = -10.0f;
                    break;
                case 2:
                    tmpText.text = userPrefs.IsEnglishSpeaker() ? "Thank you for trying to quiet down" : "Gracias por intentar calmarse";
                    audioSource.clip = userPrefs.IsEnglishSpeaker() ? Thankyoufortryingtoquietdown : Graciasporintentarcalmarse;
                    amount = -10.0f;
                    break;
                case 3:
                    tmpText.text = userPrefs.IsEnglishSpeaker() ? "Thank you for trying to take deep breath" : "Gracias por intentar respira profundo";
                    audioSource.clip = userPrefs.IsEnglishSpeaker() ? Thankyoufortakingdeepbreath : Graciasporintentarrespiraprofundo;
                    amount = -10.0f;
                    break;
                case 4:
                    tmpText.text = userPrefs.IsEnglishSpeaker() ? "Thank you for trying to calm down" : "Gracias por intentar calmarse";
                    audioSource.clip = userPrefs.IsEnglishSpeaker() ? Thankyoufortryingtocalmdown : Graciasporintentarcalmarse;
                    amount = -10.0f;
                    break;
                case 5:
                    tmpText.text = userPrefs.IsEnglishSpeaker() ? "Thank you for keeping your arms and feet to yourself" : "Gracias por mantener tus brazos y pies a tus lados";
                    audioSource.clip = userPrefs.IsEnglishSpeaker() ? Thankyouforkeepingyourarmsandfeettoyourself : Graciaspormantenertusbrazosypiesatuslados;
                    amount = -10.0f;
                    break;
                default:
                    tmpText.text = "";
                    audioSource.clip = null;
                    amount = 0.0f;
                    break;
            }
        }
    }

    void OnButtonClick()
    {
        
        adultIsSpeaking = true;
        StartCoroutine(PlayAudioAndChangeTantrumLevel());
    }

    System.Collections.IEnumerator PlayAudioAndChangeTantrumLevel()
    {

        if (audioSource.clip != null)
        {
            if(!tatrumchildbehavior.childIsTalking)
                childState.ChangeTantrumLevel(amount);
            audioSource.Play();
            yield return new WaitForSeconds(audioSource.clip.length);
            adultIsSpeaking = false;
            /*if (!tatrumchildbehavior.childIsTalking)
            {
                childState.ChangeTantrumLevel(amount);
            }*/
        }
        else
        {
            Debug.LogWarning("No AudioClip assigned.");
        }

        
        
            
    }

}
