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

	[SerializeField]
	private GameObject m_gameOverMenu;

	private void Start()
	{ 
		m_npcManager = FindObjectOfType<NPCManager>();
		
		m_npcManager.onLastInfectedDied.AddListener(PlayerWins);

		// TEMPORARY
		//onPlayerSurvived.AddListener(() => { SceneManager.LoadScene("Menu"); });
		//onPlayerInfected.AddListener(() => { SceneManager.LoadScene("Gameplay"); });
	}

	public void RegisterPlayer(Player player) {
		player.OnPlayerInfected.AddListener(PlayerInfected);
	}

	private void PlayerInfected() {
		numLives--;

		if (numLives > 0) {
			StartCoroutine(PlayerEvent(onPlayerInfected));
		} else {
			m_gameOverMenu.SetActive(true);
		}
	}

	private void PlayerWins() {
		StartCoroutine(PlayerEvent(onPlayerSurvived));
	}

	private IEnumerator PlayerEvent(UnityEvent handler) {
		yield return new WaitForSeconds(numSecondsToWait);

		handler?.Invoke();
	}
}
