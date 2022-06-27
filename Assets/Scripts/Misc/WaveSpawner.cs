using System.Collections.Generic;
using System;
using UnityEngine;
using System.Collections;

public class WaveSpawner : MonoBehaviour
{
	public enum SpawnState { SPAWNING, WAITING, COUNTING };

	public CommandCenterScript ccScript;

	[System.Serializable]
	public class Wave
	{
		public string name;
		public int[] tankIndexes;
		public int count;
		public float delayBetweenSpwns;
	}

	public Wave[] waves;
	private int nextWave = 0;
	public int NextWave
	{
		get { return nextWave + 1; }
	}

	//public Transform[] spawnPoints;

	public float timeBetweenWaves = 5f;
	private float waveCountdown;
	public float WaveCountdown
	{
		get { return waveCountdown; }
	}

	public event EventHandler OnSpawnEnemy;

	private float searchCountdown = 1f;

	bool latestResponse;

	private SpawnState state = SpawnState.COUNTING;
	public SpawnState State
	{
		get { return state; }
	}

	void Start()
	{
		waveCountdown = timeBetweenWaves;
	}

	void Update()
	{
		if (state == SpawnState.WAITING)
		{
			if (!EnemyIsAlive())
			{
				WaveCompleted();
			}
			else
			{
				return;
			}
		}

		if (waveCountdown <= 0)
		{
			if (state != SpawnState.SPAWNING)
			{
				StartCoroutine(SpawnWave(waves[nextWave]));
			}
		}
		else
		{
			waveCountdown -= Time.deltaTime;
		}
	}

	void WaveCompleted()
	{
		Debug.Log("Wave Completed!");

		state = SpawnState.COUNTING;
		waveCountdown = timeBetweenWaves;

		if (nextWave + 1 > waves.Length - 1)
		{
			nextWave = 0;
			Debug.Log("ALL WAVES COMPLETE! Looping...");
		}
		else
		{
			nextWave++;
		}
	}

	bool EnemyIsAlive()
	{
		searchCountdown -= Time.deltaTime;
		if (searchCountdown <= 0f)
		{
			searchCountdown = 1f;
			//if (GameObject.FindGameObjectWithTag("Enemy") == null)
			//{
			//	return false;
			//}
			if (!CheckEnemyAlive())
				return false;
		}
		return true;
	}

	IEnumerator SpawnWave(Wave _wave)
	{
		Debug.Log("Spawning Wave: " + _wave.name);
		state = SpawnState.SPAWNING;

		for (int i = 0; i < _wave.count; i++)
		{
			if (SpawnEnemy(_wave))
			{
				
			}
			else
				i--;
			yield return new WaitForSeconds(_wave.delayBetweenSpwns);
		}

		state = SpawnState.WAITING;

		yield break;
	}

	bool SpawnEnemy(Wave wave)
	{
		latestResponse = false;
		Debug.Log("Spawning Enemy: ");

		TankSpawnInfoEventArgs spnEvent = new TankSpawnInfoEventArgs();

		spnEvent.position = UnityEngine.Random.Range(0, 3);
		int a = UnityEngine.Random.Range(0, wave.tankIndexes.Length);
		spnEvent.tankIndex = wave.tankIndexes[a];

		OnSpawnEnemy?.Invoke(this, spnEvent);

		return latestResponse;
	}

	bool CheckEnemyAlive()
	{
		return ccScript.currDeployedTanks.Count > 0;
	}

	public void OnSpawnEventResponse(bool response)
	{
		latestResponse = response;
	}

}

public class TankSpawnInfoEventArgs: EventArgs
{
	public int tankIndex;
	public int position;
}
