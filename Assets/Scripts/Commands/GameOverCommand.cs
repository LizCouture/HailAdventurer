using UnityEngine;
using System.Collections;

public class GameOverCommand : Command{

    private Player loser;

    public GameOverCommand(Player loser)
    {
        this.loser = loser;
    }

    public override void StartCommandExecution()
    {
        //looser.PArea.Portrait.Explode();
    }
}
