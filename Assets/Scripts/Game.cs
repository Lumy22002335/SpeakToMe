using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private Text _textDisplay;
    [SerializeField] private VoskSpeechToText _speechToText;
    [SerializeField] private SerialController _serialController;
    [SerializeField] private float _typingSpeed = 0.01f;

    [Header("Scriptable Objects")]
    [SerializeField] private List<SpiritSO> _spirits;
    [SerializeField] private List<AnswersBaseSO> _answers;

    [Header("UI")]
    [SerializeField] private GameObject _nameUI;
    [SerializeField] private GameObject _spiritBoxGlow;
    [SerializeField] private GameObject _transcriberGlow;

    [Header("Sounds")]
    [SerializeField] private AudioClip _helloSound;
    [SerializeField] private AudioClip _sureSound;
    [SerializeField] private AudioClip _nameSound;
    [SerializeField] private AudioClip _winSound;
    [SerializeField] private AudioClip _loseSound;

    private SpiritSO _selectedSpirit;
    private WaitForSeconds _wait;
    private AudioClip[] answersSounds;

    private float _restartDelay = 0f;

    private bool _waitHello = true;
    private bool _waitYesNo = false;
    private bool _waitName = false;

    private void Awake()
    {
        _speechToText.OnTranscriptionResult += CheckTranscriptionResult;
    }

    private void Start()
    {
        _wait = new WaitForSeconds(_typingSpeed);
        PickASpirit();
    }

    private void PickASpirit()
    {
        _selectedSpirit = _spirits[Random.Range(0, _spirits.Count)];
    }

    private void CheckTranscriptionResult(string obj)
    {
        if (GetComponent<AudioSource>().isPlaying)
        {
            return;
        }

        RecognitionResult result = new RecognitionResult(obj);

        string resultText = result.Phrases[0].Text;

        if (resultText == "") { return; }

        Debug.Log(resultText);

        if (_waitHello)
        {
            if (_restartDelay > 0)
            {
                _restartDelay -= Time.deltaTime;
                return;
            }

            if (resultText.ToLower() == "hello")
            {
                _waitHello = false;
                answersSounds = new AudioClip[1] { _helloSound };
                Respond("I am Here");
            }
            return;
        }

        string answer = HandleQuestion(resultText);

        if (answer == "") { return; }

        Respond(answer);
    }

    private string HandleQuestion(string question)
    {
        if (_waitName)
        {
            for (int i = 0; i <= _spirits.Count; i++)
            {
                if (i == _spirits.Count)
                {
                    return "";
                }

                if (_spirits[i].Name.ToLower() == question.ToLower())
                {
                    break;
                }
            }

            _waitName = false;

            double similarity = StringSimilarity.CalculateSimilarity(_selectedSpirit.Name.ToLower(), question.ToLower());

            if (similarity > 65)
            {
                // Win
                answersSounds = new AudioClip[1] { _winSound };
                _waitHello = true;
                PickASpirit();
                _restartDelay = 3f;
                return "*Spirit being exorcised*";
            }
            else
            {
                // Lose
                answersSounds = new AudioClip[1] { _loseSound };
                _waitHello = true;
                PickASpirit();
                _restartDelay = 3f;
                return "Now it's your time to die!";
            }
        }

        answersSounds = null;
        AnswersBaseSO answer = GetAnswer(question);
        if (answer == null)
        {
            return "No Response.";
        }

        string[] answers = new string[1] { "No Response." };

        if (answer is DeathAgeAnswersSO)
        {
            answers = ((DeathAgeAnswersSO)answer).Answers[_selectedSpirit.Age];
            answersSounds = ((DeathAgeAnswersSO)answer).AnswersSounds[_selectedSpirit.Age];
        }
        else if (answer is DeathPlaceAnswersSO)
        {
            answers = ((DeathPlaceAnswersSO)answer).Answers[_selectedSpirit.PlaceOfDeath];
            answersSounds = ((DeathPlaceAnswersSO)answer).AnswersSounds[_selectedSpirit.PlaceOfDeath];
        }
        else if (answer is ElementAnswersSO)
        {
            answers = ((ElementAnswersSO)answer).Answers[_selectedSpirit.Element];
            answersSounds = ((ElementAnswersSO)answer).AnswersSounds[_selectedSpirit.Element];
        }
        else if (answer is MoodAnswersSO) 
        {
            answers = ((MoodAnswersSO)answer).Answers[_selectedSpirit.Mood];
            answersSounds = ((MoodAnswersSO)answer).AnswersSounds[_selectedSpirit.Mood];
        }
        else if (answer is NoTypeAnswersSO)
        {
            if (answer.Question == "I need to go")
            {
                // Exit game
                Application.Quit();
            }
            else if (answer.Question == "I know your name")
            {
                _waitYesNo = true;
                answersSounds = new AudioClip[1] { _sureSound };
                return "Are you sure?";
                // Name the spirit
                //_nameUI.SetActive(true);
                //_nameUI.GetComponent<NameUI>().Init(_selectedSpirit.Name);
            }
            else if (answer.Question == "Yes" && _waitYesNo)
            {
                _waitYesNo = false;
                answersSounds = new AudioClip[1] { _nameSound };
                _waitName = true;
                return "What's my name?";
            }
            else if (answer.Question == "No" && _waitYesNo)
            {
                _waitYesNo = false;
                answersSounds = new AudioClip[1] { _helloSound };
                return "Hello";
            }
        }

        return answers[Random.Range(0, answers.Length)];
    }

    private AnswersBaseSO GetAnswer(string question)
    {
        Dictionary<double, AnswersBaseSO> answers = new Dictionary<double, AnswersBaseSO>();

        foreach (AnswersBaseSO answer in _answers)
        {
            double similarity = StringSimilarity.CalculateSimilarity(answer.Question.ToLower(), question.ToLower());

            if (similarity > 65)
            {
                if (!answers.ContainsKey(similarity))
                    answers.Add(similarity, answer);
            }
        }
        
        if (answers.Count > 0)
        {
            if (answers.Count == 1)
            {
                return answers.FirstOrDefault().Value;
            }
            else
            {
                // Order the dictionary so that the highest similarity is first and return the first value
                return answers.OrderByDescending(x => x.Key).FirstOrDefault().Value;
            }
        }

        return null;
    }

    public void Respond(string text)
    {
        StopAllCoroutines();
        StartCoroutine(DelayResponse(text));
    }
    
    private IEnumerator DelayResponse(string text)
    {
        yield return new WaitForSeconds(Random.Range(0f, 2f)); // Delay the spirit's response by a random amount of time

        if (answersSounds != null)
        {
            GetComponent<AudioSource>().PlayOneShot(answersSounds[Random.Range(0, answersSounds.Length)]);
        }
            

        // Turn on light
        _spiritBoxGlow.SetActive(true);
        _serialController.SendSerialMessage("1");

        _textDisplay.text = "";
        foreach (char c in text)
        {
            _transcriberGlow.SetActive(!_transcriberGlow.activeSelf);
            _textDisplay.text += c;
            yield return _wait;
        }

        // Turn off light
        _spiritBoxGlow.SetActive(false);
        _transcriberGlow.SetActive(false);
        _serialController.SendSerialMessage("0");
    }
}
