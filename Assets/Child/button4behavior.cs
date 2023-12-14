using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class button4behavior : MonoBehaviour
{
    private UserPrefs userPrefs;

    public Button yourButton;
    public AudioSource audioSource;
    public AudioClip Youbetternottouchthosetoys;
    public AudioClip Pleasestopwhining;
    public AudioClip Stopcrying;
    public AudioClip Pleasestopcrying;
    public AudioClip Whyarentyoulisteningtome;
    public AudioClip Youseemlikeyouneedahug;

    public AudioClip Esperoquenotoquesesosjuguetes;
    public AudioClip Porfavorparadequejarte;
    public AudioClip Paradellorar;
    public AudioClip Porfavorparadellorar;
    public AudioClip Parecequenecesitasunabrazo;
    public AudioClip spanish_Whyarentyoulisteningtome;

    public AudioClip StartVoice;
    public AudioClip StartVoiceSpanish;
    private ChildState childState;
    public TextMeshProUGUI tmpText;

    public int previousTantrumLevel; // to keep track of change in tantrumLevel
    public float tantrumLevelInV2;
    public int tantrumLevel;
    public float amount;
    private float introTimer;
    private bool atStart;
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
        if(atStart && tatrumchildbehavior.childIsTalking)
            introTimer += Time.deltaTime;
        tantrumLevelInV2 = childState.tantrumLevel;
        tantrumLevel = Mathf.CeilToInt(tantrumLevelInV2 / 20);
       

        if (atStart && introTimer < introLength || button1behavior.adultIsSpeaking || childState.tantrumLevel == 0 || tatrumchildbehavior.gameLost) // if the child is saying the greeting -> not show the button
        {                                                       // if the player've just clicked this button and the audio is play -> disable button so they can't click it again
            yourButton.interactable = false;
            //tmpText.text = "";
        }
        else if(!button1behavior.adultIsSpeaking)
        {
            UpdateTextAndAudioClip(tantrumLevel);
            yourButton.interactable = true;
            atStart=false;
        }
    }

    void UpdateTextAndAudioClip(int tantrumLevel)
    {
        switch (tantrumLevel)
        {
            case 0:
                tmpText.text = userPrefs.IsEnglishSpeaker() ? "You better not touch those toys" : "Espero que no toques esos juguetes";
                audioSource.clip = userPrefs.IsEnglishSpeaker() ? Youbetternottouchthosetoys : Esperoquenotoquesesosjuguetes;
                amount = 10.0f;
                break;
            case 1:
                tmpText.text = userPrefs.IsEnglishSpeaker() ? "Please stop whining now" : "Por favor deja de quejarte";
                audioSource.clip = userPrefs.IsEnglishSpeaker() ? Pleasestopwhining : Porfavorparadequejarte;
                amount = 10.0f;
                break;
            case 2:
                tmpText.text = userPrefs.IsEnglishSpeaker() ? "Stop crying right now" : "Para de llorar";
                audioSource.clip = userPrefs.IsEnglishSpeaker() ? Stopcrying : Paradellorar;
                amount = 10.0f;
                break;
            case 3:
                tmpText.text = userPrefs.IsEnglishSpeaker() ? "Please stop crying" : "Por favor deja de llorar";
                audioSource.clip = userPrefs.IsEnglishSpeaker() ? Pleasestopcrying : Porfavorparadellorar;
                amount = 10.0f;
                break;
            case 4:
                tmpText.text = userPrefs.IsEnglishSpeaker() ? "Why aren't you listening to me?" : "Por que? no me estas haciendo caso?";
                audioSource.clip = userPrefs.IsEnglishSpeaker() ? Whyarentyoulisteningtome : spanish_Whyarentyoulisteningtome;
                amount = 10.0f;
                break;
            case 5:
                tmpText.text = userPrefs.IsEnglishSpeaker() ? "You seem like you need a hug" : "Parece que necesitas un abrazo";
                audioSource.clip = userPrefs.IsEnglishSpeaker() ? Youseemlikeyouneedahug : Parecequenecesitasunabrazo;
                amount = 10.0f;
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
