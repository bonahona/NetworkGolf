using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MiniGolfNetworkManager : NetworkManager
{
    public const string DefaultServerName = "A Server";

    [Header("Canvas UI")]

    [Tooltip("Assign Main Panel so it can be turned on from Player:OnStartClient")]
    public RectTransform mainPanel;

    [Tooltip("Assign Players Panel for instantiating PlayerUI as child")]
    public RectTransform playersPanel;
    public MiniGolfGameController GameControllerPrefab;
    MiniGolfGameController _gameController;


    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        Transform startPos = GetStartPosition();
        GameObject player = startPos != null
            ? Instantiate(playerPrefab, startPos.position, startPos.rotation)
            : Instantiate(playerPrefab);
        NetworkServer.AddPlayerForConnection(conn, player);
        _gameController.AddPlayer(player.GetComponent<MiniGolfPlayerController>());
    }

	public override void OnStartServer()
	{
		base.OnStartServer();
        _gameController = Instantiate(GameControllerPrefab);
        DontDestroyOnLoad(_gameController);
        NetworkServer.Spawn(_gameController.gameObject);

        ServerListService.PostServer();
    }

    public override void OnStopServer()
    {
        base.OnStopServer();

        ServerListService.DeleteServer();
    }
}
