using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEvolutionMaster : MonoBehaviour
{
    public static UIEvolutionMaster Instance;

    public AudioClip evolutionMusic;
    public AudioClip evolutionSuccessSound;
    public AudioClip evolutionStartSound;
    public AudioClip evolutionShineSound;

    public GameObject evolutionLinesPrefab;
    public GameObject evolutionLightsPrefab;
    public GameObject evolutionGlowPrefab;
    public GameObject evolutionGlowFinalPrefab;
    public GameObject evolutionWaveFinalPrefab;

    public TransitionBase evolutionPanel;
    public Image originalPokemonImage;
    public Image evolvedPokemonImage;
    public Transform evolutionLinesContainer;
    public Transform evolutionLightsContainer;
    public Transform evolutionGlowContainer;
    public Transform evolutionFinalWaveContainer;
    public Flowchart flowchart;

    private PokemonAnimationController originalPokemon;
    private PokemonAnimationController evolvedPokemon;

    private bool isEvolving = false;


    private void Awake()
    {
        if (Instance)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public static UIEvolutionMaster GetInstance()
    {
        return Instance;
    }

    public void InitiateEvolution(PokemonCaughtData pokemon, PokemonBaseData evolution, List<PokemonMoveLearn> learnedMoves=null)
    {
        if (learnedMoves == null)
        {
            learnedMoves = new List<PokemonMoveLearn>();
        }
        isEvolving = true;
        evolvedPokemonImage.gameObject.SetActive(false);
        evolutionPanel.FadeIn();
        UIPauseMenuMaster.GetInstance().HideOpener();
        AudioMaster.GetInstance().PlayMusic(null);
        PokemonAnimationController animator = pokemon.GetPokemonBaseData().GetAnimatorController();
        originalPokemon = Instantiate(animator);

        Dictionary<string, string> variables = new Dictionary<string, string>();
        variables.Add("pokemon", pokemon.GetName());
        InteractionsMaster.GetInstance().AddEvent(new InteractionEventFlowchart(flowchart, "Start", variables));

        StartCoroutine(InitiateEvolutionSequence(pokemon, evolution, learnedMoves));
    }

    IEnumerator InitiateEvolutionSequence(PokemonCaughtData pokemon, PokemonBaseData evolution, List<PokemonMoveLearn> learnedMoves = null)
    {
        float evoGlowTime = 2f;
        float evolutionTransitionDuration = 15f;
        float finalGlowDuration = 2f;
        float finalWaveDuration = 2f;
        float transitionDurationFinal = evolutionTransitionDuration - finalGlowDuration;
        StartCoroutine(CreateLines(0.25f));
        AudioMaster.GetInstance().PlayMusic(null);
        yield return new WaitForSeconds(1);
        AudioMaster.GetInstance().PlaySfx(evolutionStartSound);
        yield return new WaitForSeconds(2f);
        AudioMaster.GetInstance().PlaySfx(pokemon.GetCry());
        yield return new WaitForSeconds(0.2f);
        GameObject finalWave = Instantiate(evolutionWaveFinalPrefab, evolutionFinalWaveContainer);
        Destroy(finalWave, finalWaveDuration);
        yield return new WaitForSeconds(evoGlowTime);
        AudioMaster.GetInstance().PlayMusic(evolutionMusic);
        GameObject evoGlow = Instantiate(evolutionGlowPrefab, evolutionGlowContainer);
        Destroy(evoGlow, evolutionTransitionDuration);
        StartCoroutine(CreateLights(0.25f, evolutionTransitionDuration + finalGlowDuration/2f));
        yield return new WaitForSeconds(1f);
        // Load original pokemon transition
        originalPokemonImage.color = Color.white;
        TransitionSize transition = originalPokemonImage.gameObject.AddComponent<TransitionSize>();
        transition.FadeIn();
        transition.initialSize = Vector3.one;
        transition.finalSize = new Vector3(0.5f, 0.5f, 1);
        transition.pingPong = true;
        transition.speed = 2f;
        yield return new WaitForSeconds(transitionDurationFinal / 3f);
        transition.speed = 4f;
        yield return new WaitForSeconds(transitionDurationFinal / 3f);
        transition.speed = 6f;
        yield return new WaitForSeconds(transitionDurationFinal / 3f);
        // Show final glow
        GameObject finalGlow = Instantiate(evolutionGlowFinalPrefab, evolutionGlowContainer);
        Destroy(finalGlow, finalGlowDuration);
        yield return new WaitForSeconds(1f);
        AudioMaster.GetInstance().PlaySfx(evolutionShineSound);
        yield return new WaitForSeconds(0.5f);
        // Change sprites to evolution
        evolvedPokemon = Instantiate(evolution.GetAnimatorController());
        originalPokemonImage.gameObject.SetActive(false);
        evolvedPokemonImage.gameObject.SetActive(true);
        AudioMaster.GetInstance().PlayMusic(null);
        yield return new WaitForSeconds(1f);
        finalWave= Instantiate(evolutionWaveFinalPrefab, evolutionFinalWaveContainer);
        Destroy(finalWave, finalWaveDuration);
        AudioMaster.GetInstance().PlaySfx(evolution.GetCry());
        yield return new WaitForSeconds(2f);
        AudioMaster.GetInstance().PlaySfx(evolutionSuccessSound);
        Dictionary<string, string> variables = new Dictionary<string, string>();
        variables.Add("pokemon", pokemon.GetName());
        variables.Add("evolution", evolution.species);
        InteractionsMaster.GetInstance().AddEvent(new InteractionEventFlowchart(flowchart, "Evolution", variables));
        foreach (PokemonMoveLearn ml in learnedMoves)
        {
            Dictionary<string, string> moveVar = new Dictionary<string, string>();
            moveVar.Add("pokemon", pokemon.GetName());
            moveVar.Add("move", ml.move.moveName);
            InteractionsMaster.GetInstance().AddEvent(new InteractionEventFlowchart(flowchart, "Move Learned", moveVar));
        }
        if (learnedMoves.Count > 0)
        {
            InteractionsMaster.GetInstance().AddEvent(new InteractionEventFlowchart(flowchart, "Move Equip"));
        }
        InteractionsMaster.GetInstance().AddEvent(new InteractionEventFinishEvolution());
    }

    IEnumerator CreateLights(float interval, float duration)
    {
        while (duration > 0f)
        {
            GameObject lights = Instantiate(evolutionLightsPrefab, evolutionLightsContainer);
            lights.transform.eulerAngles = new Vector3(0f, 0f, Random.Range(0f, 359f));
            float waitAmount = interval * (0.5f + Random.value * 0.5f);
            duration -= waitAmount;
            Destroy(lights, 1f);
            yield return new WaitForSeconds(waitAmount);
        }
    }
    IEnumerator CreateLines(float interval)
    {
        while (isEvolving)
        {
            GameObject lines = Instantiate(evolutionLinesPrefab, evolutionLinesContainer);
            Destroy(lines, 2f);
            yield return new WaitForSeconds(interval);
        }
    }

    public float FinishEvolution()
    {
        // Close Pokemon Screen and reset all
        isEvolving = false;
        evolvedPokemonImage.gameObject.SetActive(false);
        Destroy(originalPokemon.gameObject);
        Destroy(evolvedPokemon.gameObject);
        evolutionPanel.FadeOut();
        WorldMapMaster.GetInstance().PlayCurrentPlaceMusic();
        TransitionMaster.GetInstance().SetDialogueToScene();
        UIPauseMenuMaster.GetInstance().ShowOpener();
        InteractionsMaster.GetInstance().ExecuteNext(1f);
        return 1f;
    }


    private void Update()
    {
        if (originalPokemon)
        {
            originalPokemonImage.sprite = originalPokemon.GetCurrentSprite();
        }
        if (evolvedPokemon)
        {
            evolvedPokemonImage.sprite = evolvedPokemon.GetCurrentSprite();
        }
    }
}
