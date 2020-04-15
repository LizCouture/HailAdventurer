using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimeline
{ 
    public Queue<GameEvent> timeline;
    public bool playingTimeline;
    
    public Dictionary<int, int> roundsByPlayer = new Dictionary<int, int>()
                                                {   {3, 4 },
                                                    {4, 4 },
                                                    {5, 3 },
                                                    {6, 2 },
                                                    {7, 2 },
                                                    {8, 2 } };
    public const int ROUND_ANNOUNCEMENT_DURATION = 10;
    public const int ADVENTURER_ANNOUNCEMENT_DURATION = 30;
    public const int PLAY_ITEMS_DURATION = 30;
    public const int SELL_ITEMS_DURATION = 60;
    public const int CHOOSE_ITEMS_DURATION = 60;
    public const int REPORT_WINNER_DURATION = 10;

    private int numRounds;
    public GameTimeline(int numPlayers)
    {
        playingTimeline = false;
        if (numPlayers < 3)
        {
            Debug.LogError("Too few players in GameTimeline.  We shouldn't be here.");
            return;
        }
        numRounds = roundsByPlayer[GameManager.Instance.Players.Count];
        timeline = generateTimeline(numPlayers);
    }

    /*
     *  generateTimeline- Creates the correct sequence of game events for the number of players.
     *  input- int numPlayers (between 3-8)
     *  output- List<GameEvent> describing the correct timeline for the game.
     * 
     *   Eventually, we might want to only include some game events conditionally, like if players
     *   have decided to skip an intro or tutorial.
     *   
     *   But for now a game will consist of an intro, a skippable tutorial, and then a number of rounds
     *   of RevealAdventurer, 
     * */
    private Queue<GameEvent> generateTimeline(int numPlayers)
    {
        // Intro
        Queue<GameEvent> tl = new Queue<GameEvent>();
        tl.Enqueue(new IntroGameEvent(this, false));

        // Tutorial
        tl.Enqueue(new TutorialGameEvent(this, false));
        
        // Rounds of RoundAnnouncement, AdventurerAnnouncement, Cards Played, Cards Presented, Cards Selected, Points Awardsd
        // Rounds determined by number of players.
        for (int i = 0; i < numRounds; i++)
        {
            tl.Enqueue(new RoundAnnouncementEvent(this, true, i, ROUND_ANNOUNCEMENT_DURATION));
            // TODO:  Shuffle order of players maybe?
            // Each player gets a chance to be the adventurer every round.
            for (int j = 0; j < GameManager.Instance.Players.Count; j++)
            {
                tl.Enqueue(new AdventurerAnnouncementEvent(this, true, j, ADVENTURER_ANNOUNCEMENT_DURATION));
                tl.Enqueue(new PlayItemsEvent(this, true, PLAY_ITEMS_DURATION));

                // Each player who isn't the adventurer gets a chance to sell.
                for (int k = 0; k < GameManager.Instance.Players.Count; k++)
                {
                    if (k != j) {
                        tl.Enqueue(new SellItemsEvent(this, true, k, SELL_ITEMS_DURATION));
                    }
                }
                tl.Enqueue(new ChooseItemsEvent(this, true, j, CHOOSE_ITEMS_DURATION));
                tl.Enqueue(new ReportAdventureWinnerEvent(this, true, REPORT_WINNER_DURATION));
            }
        }

        // Final Score
        tl.Enqueue(new GameOverEvent(this, false));

        return tl;
    }

    public void nextInQueue()
    {
        playingTimeline = true;
        timeline.Dequeue().onStart();
    }

}
