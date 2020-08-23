using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class MazeUIController : MonoBehaviour
{
    public Image infirIMG, serptIMG, eclypIMG, drakeIMG, crossIMG;
    public GameObject infirRunePanel, serptRunePanel, eclypRunePanel, drakeRunePanel, crossPanel, gameMenu, optionsMenu, confirmMenu, player;
    public Text infirStory, serptStory, eclypStory, drakeStory, crossStory;
    private int storyIndex;
    private bool hadCrossBefore = false;
    private string[] story;

    // Start is called before the first frame update
    void Start()
    {
        story = new string[6];
        story[0] = "As you pick up the rune, a voice speaks from the very air. It is little more than a whisper on the breeze, but you can make it out nonetheless. 'So, not just a treasure hunter after all, You are blood of my blood-that-once-was. Do you know what you have just taken? Were you sent by Charon? Do you know what doom you have been cursed to?'\n\n'Who’s there!?' You demand, but the voice is gone and only silence remains.";
        story[1] = "You find another Rune, and as you pick it up, you hear the voice again. ‘Once, long ago, I too lived, and died, and stood before Charon. I offered him the toll, but instead of taking it, he told me of this Labyrinth. Have you guessed the truth of the Labyrinth yet? Have you guessed why a Ferryman of Hades needs a Labyrinth to trap souls?’\n\n’Who is this!? Are you the Ghost? The Ancestor Spirit?” but again the voice is gone.’";
        story[2] = "This time, as you take the Rune, you are prepared for the voice. ‘Am I the Dark Spirit? Yes and no. I am what the Ghost once was, and the Ghost is what I once was. But I grow weak, and the Ghost grows strong. I have forgotten much, but I remember this: Stop the Dark Spirit from escaping at any cost!’\n\n’Tell me more about the Labyrinth! Tell me how to escape!’ You shout, well aware that you will receive no answer until you find another Rune.";
        story[3] = "‘Tell me about the Labyrinth, and tell me what I must do.’ You declare, just before taking the last Rune.\n\n’Do not trust Charon. This is his Labyrinth, and he is responsible for the tortures that shaped the Dark Spirit my other half has become! The Runes are me, and tied to our bloodline. Bring them to Charon and find out what he wants, but remember; he cannot act upon the Runes himself, he needs to use you as his agent. Defy him! Overcome him! Do not trust him as I once did!’\n\nThe voice fades as before. You have all four Runes now. Time to seek out Charon again.";
        story[4] = "As you collect the crucifix before you, a sense of peace fills you.\n\nWhatever your beleifs, you realize that this is no mere religious symbol. It is imbued with faith and hope of those who came here before. Charon sent you for the runes, but did not mention this.\n\nAfter a short debate you decide to take it with you, just in case.";
        story[5] = "You found the crucifix again! You thought it was lost! Joyously, you pick it up, hoping you will not need to use it again…";
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.GAME.infir) infirIMG.enabled = true;
        if (!GameManager.GAME.infir) infirIMG.enabled = false;
        if (GameManager.GAME.serpt) serptIMG.enabled = true;
        if (!GameManager.GAME.serpt) serptIMG.enabled = false;
        if (GameManager.GAME.eclyp) eclypIMG.enabled = true;
        if (!GameManager.GAME.eclyp) eclypIMG.enabled = false;
        if (GameManager.GAME.drake) drakeIMG.enabled = true;
        if (!GameManager.GAME.drake) drakeIMG.enabled = false;
        if (GameManager.GAME.cross) crossIMG.enabled = true;
        if (!GameManager.GAME.cross) crossIMG.enabled = false;

        if (Input.GetKeyUp(KeyCode.Escape) || Input.GetKeyUp(KeyCode.E) || Input.GetKeyUp(KeyCode.Space))
        {
            if (infirRunePanel.activeSelf) { infirRunePanel.SetActive(false); GameManager.GAME.UnpauseGame(player); Debug.Log(">>> INFIR PANEL UNPAUSE"); }
            if (serptRunePanel.activeSelf) { serptRunePanel.SetActive(false); GameManager.GAME.UnpauseGame(player); Debug.Log(">>> SERPT PANEL UNPAUSE"); }
            if (eclypRunePanel.activeSelf) { eclypRunePanel.SetActive(false); GameManager.GAME.UnpauseGame(player); Debug.Log(">>> ECLYP PANEL UNPAUSE"); }
            if (drakeRunePanel.activeSelf) { drakeRunePanel.SetActive(false); GameManager.GAME.UnpauseGame(player); Debug.Log(">>> DRAKE PANEL UNPAUSE"); }
            if (crossPanel.activeSelf) { crossPanel.SetActive(false); GameManager.GAME.UnpauseGame(player); Debug.Log(">>> CROSS PANEL UNPAUSE"); }

        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if(!infirRunePanel.activeSelf && !serptRunePanel.activeSelf && !eclypRunePanel.activeSelf && !drakeRunePanel.activeSelf && !crossPanel.activeSelf) //no story panels are active
            {
                if (optionsMenu.activeSelf || confirmMenu.activeSelf)
                {
                    optionsMenu.SetActive(false);
                    confirmMenu.SetActive(false);
                }
                else
                {
                    if (gameMenu.activeSelf) GameManager.GAME.UnpauseGame(player); 
                        if (!gameMenu.activeSelf) GameManager.GAME.PauseGame(player);
                    gameMenu.SetActive(!gameMenu.activeSelf);
                }
            }
        }
    }

    public string getStory(int n)
    {
        string r = story[storyIndex];
        if (n < 4) storyIndex++;
        if (n >= 4)
        {
            if(!hadCrossBefore) r = story[4];
            if (hadCrossBefore) r = story[5];
            hadCrossBefore = true;
        }
        return r;
    }

    public void UnpauseGame()
    {
        GameManager.GAME.UnpauseGame(player);
    }
}
