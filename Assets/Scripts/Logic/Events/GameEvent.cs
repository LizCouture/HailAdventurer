using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


public enum TimelineEventType { None, Intro, Tutorial, RoundAnnouncement, AdventurerReveal, PlayItems, SellItems,
    ChooseItems, ReportAdventureWinner, GameOver};

public class GameEvent
{
    public TimelineEventType type;

    public bool timed;
    public int duration;


    /*
     * GameEvent-
     * A game event is a phase of gameplay that all players experience simultaneously.
     * For example, watching an intro, the adventurer being chosen, cards being played,
     * the adventurer making a decision, each of these sections of play is a game event.
     * 
     * type - What TimelineEventType is associated with this game event?
     * timed - Is this an event with a timer, or can it go on as long as the players want it to go on for?
     * duration - (optional) Default 0.  If timed, how long does the timer last?
     * 
     * */
     public GameEvent()
    {
        this.type = TimelineEventType.None;
        this.timed = false;
        this.duration = 0;
    }

    public GameEvent(TimelineEventType type, bool timed, int duration = 0)
    {
        this.type = type;
        this.timed = timed;
        this.duration = duration;
    }

    /*
     * These actions occur whenever this event starts
     * 
     * IMPORTANT:  By default, base.onStart() will end the event after 1/2 a second.
     * You should never call base.onStart() unless you're testing with an unfinished
     * timeline.
     */
    public virtual void onStart()
    {
        Task.Delay(500).ContinueWith(t=> onEnd());
    }

    /*
     * These actions occur whenever this event ends
     * This should be called by the GameManager, through the timeline.
     */
    public virtual void onEnd()
    {
        GameManager.Instance.goToNextEvent();
    }

}
