using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MiniGolfGameController : NetworkBehaviour
{
    // yes, im that lazy...
    private static MiniGolfGameController _snigelton;
    public static MiniGolfGameController Instance 
    { 
        get
		{
			if (!_snigelton)
			{
                _snigelton = FindObjectOfType<MiniGolfGameController>();

                if (!_snigelton)
                {
                    Debug.LogError("No game controller");
                }
            }
            return _snigelton;
        } 
    }

    public List<MiniGolfPlayerController> players = new List<MiniGolfPlayerController>();
    public int activePlayer = -1;

	public void Start()
	{
        _snigelton = this;
    }

	public void AddPlayer(MiniGolfPlayerController player)
    {
        players.Add(player);
        if (activePlayer == -1)
        {
            activePlayer = 0;
            players[activePlayer].CanHazControl();
        }
    }

    public void PlayerPlayed(uint playerId)
	{
        var prevPlayerIdx = players.Select((o, i) => new { o, i }).First(o => o.o.netId == playerId).i;
        activePlayer = (prevPlayerIdx + 1) % players.Count;

        players[activePlayer].CanHazControl();
    }
}
