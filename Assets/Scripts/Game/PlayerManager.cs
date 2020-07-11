using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
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

	private void Start()
	{ 
		m_npcManager = FindObjectOfType<NPCManager>();
		
		m_npcManager.OnLastInfectedDied.AddListener(PlayerWins);
	}

	public void RegisterPlayer(Player player) {
		player.OnPlayerInfected.AddListener(PlayerInfected);
	}

	private void PlayerInfected() {
		numLives--;

		if (numLives > 0) {
			StartCoroutine(PlayerEvent(onPlayerInfected));
		} else {
			var gameOverPanel = GameObject.Find("GameOverMenu");
			gameOverPanel.SetActive(true);
		}
	}

	private void PlayerWins() {
		StartCoroutine(PlayerEvent(onPlayerSurvived));
	}

	private IEnumerator PlayerEvent(UnityEvent handler) {
		yield return new WaitForSeconds(numSecondsToWait);

		handler?.Invoke();
	}
	
	private IEnumerator LoadMenuAsync()
	{
		var asyncWait = SceneManager.LoadSceneAsync("Menu");

		while (!asyncWait.isDone)
		{
			yield return null;
		}
	}

}
