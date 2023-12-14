using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class button3behavior : MonoBehaviour
{
    private UserPrefs userPrefs;

    public Button yourButton;
    public AudioSource audioSource;
    public AudioClip StartVoice;
    public AudioClip StartVoiceSpanish;
    public AudioClip Youknowyoucannotplayrightnow;
    public AudioClip Iwishyouwoulduseyourbigkidvoice;
    public AudioClip Ohyouaregoingtocrynow;
    public AudioClip Ineedyoutoquietdown;
    public AudioClip Youneedtoquietdownrightnow;
    public AudioClip Youneedtocalmyourselfdownnow;

    public AudioClip Sabesquenopuedesjugarahoramismo;
    public AudioClip Ojaláusarastuvozdeniñograndej;
    public AudioClip Vasallorarahora;
    public AudioClip Necesitoquetecalmes;
    public AudioClip Tienesquecalmarteahoramismo;
    public AudioClip Necesitascalmarteahora;

    private ChildState childState;
    public TextMeshProUGUI tmpText;

    public int previousTantrumLevel; // to keep track of change in tantrumLevel
    public float tantrumLevelInV2;
    public int tantrumLevel;
    public float amount;
    private bool atStart;
    private float introTimer;
    private float introLength;
    void Start()
    {
        tmpText = yourButton.GetComponentInChildren<TextMeshProUGUI>();
        userPrefs = GameObject.Find("UserPrefs").GetComponent<UserPrefs>();

        if (tmpText == null)
        {
            Debug.LogError("TextMeshProUGUI component not found on the button's children.");
        }

        switch (userPrefs.GetChildAvatar())
        {
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
        

        if (atStart && introTimer < introLength || button1behavior.adultIsSpeaking || childState.tantrumLevel == 0 || tatrumchildbehavior.gameLost)  // if we at start the child is saying the greeting -> disable the button
        {                                                                        // if the player've just clicked this button and the audio is play -> disable button so they can't click it again
            yourButton.interactable = false;
            //tmpText.text = "";
        }
        else if(!button1behavior.adultIsSpeaking)
        { // let the parent finish talking first, otherwise their audio is cut off during tantrum level change
            UpdateTextAndAudioClip(tantrumLevel);
            yourButton.interactable = true;
            atStart = false;
        }
    }

    void UpdateTextAndAudioClip(int tantrumLevel)
    {
        switch (tantrumLevel)
        {
            case 0:
                tmpText.text = userPrefs.IsEnglishSpeaker() ? "You know you cannot play right now" : "Ya sabes que no puedes jugar ahora";
                audioSource.clip = userPrefs.IsEnglishSpeaker() ? Youknowyoucannotplayrightnow : Sabesquenopuedesjugarahoramismo;
                amount = 5.0f;
                break;
            case 1:
                tmpText.text = userPrefs.IsEnglishSpeaker() ? "I wish you would use your big kid voice" : "Ojalá usaras tu voz de niño grande";
                audioSource.clip = userPrefs.IsEnglishSpeaker() ? Iwishyouwoulduseyourbigkidvoice : Ojaláusarastuvozdeniñograndej;
                amount = 5.0f;
                break;
            case 2:
                tmpText.text = userPrefs.IsEnglishSpeaker() ? "Oh, you are going to cry now" : "Vas a llorar ahora";
                audioSource.clip = userPrefs.IsEnglishSpeaker() ? Ohyouaregoingtocrynow : Vasallorarahora;
                amount = 5.0f;
                break;
            case 3:
                tmpText.text = userPrefs.IsEnglishSpeaker() ? "I need you to quiet down" : "Necesito que te calmes";
                audioSource.clip = userPrefs.IsEnglishSpeaker() ? Ineedyoutoquietdown : Necesitoquetecalmes;
                amount = 5.0f;
                break;
            case 4:
                tmpText.text = userPrefs.IsEnglishSpeaker() ? "You need to quiet down right now" : "Tienes que calmarte ahora mismo";
                audioSource.clip = userPrefs.IsEnglishSpeaker() ? Youneedtoquietdownrightnow : Tienesquecalmarteahoramismo;
                amount = 5.0f;
                break;
            case 5:
                tmpText.text = userPrefs.IsEnglishSpeaker() ? "You need to calm yourself down now" : "Necesitas calmarte ahora";
                audioSource.clip = userPrefs.IsEnglishSpeaker() ? Youneedtocalmyourselfdownnow : Necesitascalmarteahora;
                amount = 5.0f;
                break;
            default:
                tmpText.text = "";
                audioSource.clip = null;
                amount = 0.0f;
                break;
        }
    }

    void OnButtonClick()
    {
        button1behavior.adultIsSpeaking = true;
        StartCoroutine(PlayAudioAndChangeTantrumLevel());
    }

    System.Collections.IEnumerator PlayAudioAndChangeTantrumLevel()
    {
        if (audioSource.clip != null)
        {
            childState.ChangeTantrumLevel(amount);
            audioSource.Play();
            yield return new WaitForSeconds(audioSource.clip.length);
            button1behavior.adultIsSpeaking = false;
            tatrumchildbehavior.negativeStatementSelected = true;
        }
        else
        {
            Debug.LogWarning("No AudioClip assigned.");
        }

        
    }

}
