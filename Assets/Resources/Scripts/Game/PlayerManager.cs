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
	public int numLives = 3;

	public int numSecondsToWait = 3;

	public UnityEvent onPlayerSurvived;
	public UnityEvent onPlayerInfected;

	private NPCManager m_npcManager;

	[SerializeField]
	private GameObject gameOverMenu = null;

	[SerializeField]
	private TextMeshProUGUI livesText = null;

	private bool m_hasPlayerDied = false;

	private void Start()
	{ 
		m_npcManager = FindObjectOfType<NPCManager>();
		
		m_npcManager.onLastInfectedDied.AddListener(PlayerWins);

		livesText.text = $"Lives: {numLives}";
	}

	public void RegisterPlayer(Player player) {
		m_hasPlayerDied = false;
		player.OnPlayerInfected.AddListener(PlayerInfected);
	}

	private void PlayerInfected() {
		numLives--;
		m_hasPlayerDied = true;

		livesText.text = $"Lives: {numLives}";

		// Restart the level if the player dies
		// Show the game over menu if they are out of lives
		StartCoroutine(
			numLives > 0
				? PlayerEvent(onPlayerInfected)
				: ShowGameOverMenu()
		);
	}

	private void PlayerWins() { 
		if (!m_hasPlayerDied)
			StartCoroutine(PlayerEvent(onPlayerSurvived));
	}

	private IEnumerator PlayerEvent(UnityEvent handler) {
		yield return new WaitForSeconds(numSecondsToWait);

		handler?.Invoke();
	}

	private IEnumerator ShowGameOverMenu() {
		yield return new WaitForSeconds(numSecondsToWait);

		gameOverMenu.SetActive(true);
	}
}
