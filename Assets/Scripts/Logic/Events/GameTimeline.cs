using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimeline
{ 
    public Queue<GameEvent> timeline;
    public GameEvent currentEvent;
    public bool playingTimeline;
    
    public Dictionary<int, int> roundsByPlayer = new Dictionary<int, int>()
                                                {   {3, 2 },
                                                    {4, 2 },
                                                    {5, 2 },
                                                    {6, 1 },
                                                    {7, 1 },
                                                    {8, 1 } };
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
        numRounds = roundsByPlayer[GameManager.Instance.playerCount()];
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
        tl.Enqueue(new IntroGameEvent(false));

        // Tutorial
        tl.Enqueue(new TutorialGameEvent(false));
        
        // Rounds of RoundAnnouncement, AdventurerAnnouncement, Cards Played, Cards Presented, Cards Selected, Points Awardsd
        // Rounds determined by number of players.
        for (int i = 0; i < numRounds; i++)
        {
            tl.Enqueue(new RoundAnnouncementEvent(true, i, ROUND_ANNOUNCEMENT_DURATION));
            // TODO:  Shuffle order of players maybe?
            // Each player gets a chance to be the adventurer every round.
            for (int j = 0; j < GameManager.Instance.playerCount(); j++)
            {
                tl.Enqueue(new AdventurerAnnouncementEvent(true, j, ADVENTURER_ANNOUNCEMENT_DURATION));
                tl.Enqueue(new PlayItemsEvent(true, PLAY_ITEMS_DURATION));

                // Each player who isn't the adventurer gets a chance to sell.
                for (int k = 0; k < GameManager.Instance.playerCount(); k++)
                {
                    if (k != j) {
                        tl.Enqueue(new SellItemsEvent(true, k, SELL_ITEMS_DURATION));
                    }
                }
                tl.Enqueue(new ChooseItemsEvent(true, j, CHOOSE_ITEMS_DURATION));
                tl.Enqueue(new ReportAdventureWinnerEvent(true, REPORT_WINNER_DURATION));
            }
        }

        // Final Score
        tl.Enqueue(new GameOverEvent(false));

        return tl;
    }

    public void nextInQueue()
    {
        Debug.Log("GameTimeline.nextInQueue");
        playingTimeline = true;
        currentEvent = timeline.Dequeue();
        currentEvent.onStart();
    }

    public void endCurrentEvent()
    {
        Debug.Log("timeline end current event");
        currentEvent.onEnd();
    }

    public int queueLength()
    {
        return timeline.Count;
    }

    public override string ToString()
    {
        string ret = "";
        foreach (var i in timeline.ToArray()) {
            ret += i.ToString() + ", ";
        }
        return ret;
    }

}
