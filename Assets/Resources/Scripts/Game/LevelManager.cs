using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour { 
	public List<GameObject> levels;
	public int startingLevelIndex = 0;
	
	public UnityEvent onPreLevelLoadEvent;
	public UnityEvent onPostLevelLoadEvent;

	private int m_currentLevelIndex;
	private GameObject m_currentLevel;
	
	private void Start() {
		LoadLevelPrefabs("Prefabs/Levels");
		
		var playerManager = FindObjectOfType<PlayerManager>();
		playerManager.onPlayerInfected.AddListener(PlayerInfectedEvent);
		playerManager.onPlayerSurvived.AddListener(PlayerSurvivedEvent);

		// Call GenerateNewLevel() with reset to avoid having to rewrite the function in Start()
		m_currentLevelIndex = startingLevelIndex;
		GenerateNewLevel(reset: true);
	}

	private void GenerateNewLevel(bool reset) {
		onPreLevelLoadEvent?.Invoke();
		
		Destroy(m_currentLevel);

		if (reset) {
			m_currentLevel = Instantiate(levels[m_currentLevelIndex]);
		} else {
			m_currentLevelIndex = levels.Count > 1 ? Random.Range(1, levels.Count) : 0;
			m_currentLevel = Instantiate(levels[m_currentLevelIndex]);
		}

		onPostLevelLoadEvent?.Invoke();
	}

	private void PlayerInfectedEvent() {
		GenerateNewLevel(reset: true);
	}
	
	private void PlayerSurvivedEvent() {
		GenerateNewLevel(reset: false);
	}

	public void RetryButtonClickedEvent() {
		StartCoroutine(LoadSceneAsync("Gameplay"));
	}
	
	public void MenuButtonClickedEvent() {
		StartCoroutine(LoadSceneAsync("Menu"));
	}

	private IEnumerator LoadSceneAsync(string scene) {
		var asyncLoad = SceneManager.LoadSceneAsync(scene);

		while (!asyncLoad.isDone)
		{
			yield return null;
		}
	}

	private void LoadLevelPrefabs(string path) {
		var res = Resources.LoadAll<GameObject>(path);

		foreach (var obj in res) {
			levels.Add(obj);
		}
	}
}
