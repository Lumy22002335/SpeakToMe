using UnityEngine;

[CreateAssetMenu(fileName = "Spirit", menuName = "Spirit")]
public class SpiritSO : ScriptableObject
{
    [SerializeField] private string spiritName;
    public string Name => spiritName;

    [SerializeField] private ElementType element;
    public ElementType Element => element;

    [SerializeField] private MoodType mood;
    public MoodType Mood => mood;

    [SerializeField] private DeathAge age;
    public DeathAge Age => age;

    [SerializeField] private DeathPlace placeOfDeath;
    public DeathPlace PlaceOfDeath => placeOfDeath;
}
