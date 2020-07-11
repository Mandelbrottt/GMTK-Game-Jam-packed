using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class PlayerManager : MonoBehaviour
{
	public int NumLives { get; private set; } = 3;

	public int NumSecondsToWait { get; } = 3;

	public UnityEvent onPlayerSurvived;
	public UnityEvent onPlayerInfected;

	private NPCManager m_npcManager;

	[SerializeField]
	private GameObject gameOverMenu = null;

	[SerializeField]
	private TextMeshProUGUI livesText = null;

	private void Start()
	{ 
		m_npcManager = FindObjectOfType<NPCManager>();
		
		m_npcManager.onLastInfectedDied.AddListener(PlayerWins);

		livesText.text = $"Lives: {NumLives}";
	}

	public void RegisterPlayer(Player player) {
		player.OnPlayerInfected.AddListener(PlayerInfected);
	}

	private void PlayerInfected() {
		NumLives--;

		livesText.text = $"Lives: {NumLives}";

		// Restart the level if the player dies
		// Show the game over menu if they are out of lives
		StartCoroutine(
			NumLives > 0
				? PlayerEvent(onPlayerInfected)
				: ShowGameOverMenu()
		);
	}

	private void PlayerWins() {
		StartCoroutine(PlayerEvent(onPlayerSurvived));
	}

	private IEnumerator PlayerEvent(UnityEvent handler) {
		yield return new WaitForSeconds(NumSecondsToWait);

		handler?.Invoke();
	}

	private IEnumerator ShowGameOverMenu() {
		yield return new WaitForSeconds(NumSecondsToWait);

		gameOverMenu.SetActive(true);
	}
}
