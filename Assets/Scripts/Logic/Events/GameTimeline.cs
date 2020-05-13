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
    public int ROUND_ANNOUNCEMENT_DURATION = 10;
    public int ADVENTURER_ANNOUNCEMENT_DURATION = 30;
    public int PLAY_ITEMS_DURATION = 30;
    public int SELL_ITEMS_DURATION = 60;
    public int CHOOSE_ITEMS_DURATION = 60;
    public int REPORT_WINNER_DURATION = 10;

    public bool DEV_MODE = true;

    private int numRounds;
    public GameTimeline(int numPlayers)
    {
        if (DEV_MODE)
        {
            initializeRoundsToDevTime();
        }
        playingTimeline = false;
        if (numPlayers < 3)
        {
            Debug.LogError("Too few players in GameTimeline.  We shouldn't be here.");
            return;
        }
        numRounds = roundsByPlayer[GameManager.Instance.playerCount()];
        timeline = generateTimeline(numPlayers);
    }

    private void initializeRoundsToDevTime()
    {
        ROUND_ANNOUNCEMENT_DURATION = 2;
        ADVENTURER_ANNOUNCEMENT_DURATION = 5;
        PLAY_ITEMS_DURATION = 30;
        SELL_ITEMS_DURATION = 5;
        CHOOSE_ITEMS_DURATION = 10;
        REPORT_WINNER_DURATION = 10;
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
            // Put this in reverse order for testing so I don'th ave to be the adventurer.
            for (int j = GameManager.Instance.playerCount() - 1; j >= 0 ; j--)
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
        playingTimeline = true;
        currentEvent = timeline.Dequeue();
        Debug.Log("TIMELINE CURRENT EVENT START: " + currentEvent.ToString());
        currentEvent.onStart();
    }

    public void endCurrentEvent()
    {
        Debug.Log("TIMELINE CURRENT EVENT END: " + currentEvent.ToString());
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
