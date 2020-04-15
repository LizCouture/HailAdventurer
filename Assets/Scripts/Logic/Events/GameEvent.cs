using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum TimelineEventType { None, Intro, Tutorial, RoundAnnouncement, AdventurerReveal, PlayItems, SellItems,
    ChooseItems, ReportAdventureWinner, GameOver};

public class GameEvent
{
    public TimelineEventType type;

    public bool timed;
    public int duration;

    public GameTimeline myTimeline;

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

    public GameEvent(GameTimeline timeline, TimelineEventType type, bool timed, int duration = 0)
    {
        myTimeline = timeline;
        this.type = type;
        this.timed = timed;
        this.duration = duration;
    }

    /*
     * These actions occur whenever this event starts
     */
    public virtual void onStart()
    {
        //TODO:  Don't do this.  It should be different for every action
        onEnd();
    }

    /*
     * These actions occur whenever this event ends
     */
    public virtual void onEnd()
    {
        myTimeline.nextInQueue();
    }
}
