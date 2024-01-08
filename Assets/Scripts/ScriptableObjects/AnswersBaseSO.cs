using UnityEngine;

public abstract class AnswersBaseSO : ScriptableObject 
{
    [SerializeField] private string question;
    public string Question => question;
}

public abstract class AnswersBaseSO<T> : AnswersBaseSO
{
    // Don't need a value
    public T Type;

    [SerializeField] private SerializableDictionary<T, string[]> answers;
    public SerializableDictionary<T, string[]> Answers => answers;

    [SerializeField] private SerializableDictionary<T, AudioClip[]> answersSounds;
    public SerializableDictionary<T, AudioClip[]> AnswersSounds => answersSounds;
}
