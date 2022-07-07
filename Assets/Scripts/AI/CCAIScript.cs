using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCAIScript : MonoBehaviour
{
	public enum SpawnState { COUNTING, SPAWNING, WAITING };

	public CommandCenterScript ccScript;
	public EnemyDetectorModuleScript enemyDetect;

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
	public float backUpSpawnDelay = 5f;
	private float waveCountdown;
	public float WaveCountdown
	{
		get { return waveCountdown; }
	}

	public SpawnState state;

	//private

    private void Start()
    {
		StartCoroutine(nameof(MasterRoutine));
		StartCoroutine(nameof(DeployBackupRoutine));
    }

    IEnumerator MasterRoutine()
	{
		Debug.Log("Master Routine Start");
		while (true)
		{
			yield return StartCoroutine(nameof(CountingRoutine));

			yield return StartCoroutine(nameof(SpawningRoutine));

			yield return StartCoroutine(nameof(WaitingRoutine));

			yield return null;

		}
		Debug.Log("Master Routine End");
	}

	IEnumerator CountingRoutine()
	{
		Debug.Log("Counting Routine Start");
		state = SpawnState.COUNTING;
		yield return new WaitForSeconds(timeBetweenWaves);
		Debug.Log("Counting Routine End");
	}

	IEnumerator SpawningRoutine()
	{
		Debug.Log("Spawning Routine Start");
		state = SpawnState.SPAWNING;
		Wave wave = waves[nextWave];

		for (int i = 0; i < wave.count; i++)
		{
			if (SpawnEnemy(wave))
			{

			}
			else
				i--;
			yield return new WaitForSeconds(wave.delayBetweenSpwns);
		}

		Debug.Log("Spawning Routine End");
	}

	IEnumerator WaitingRoutine()
	{
		Debug.Log("Waiting Routine Start");
		state = SpawnState.WAITING;

		bool res = true;

        while (res)
        {
			res = CheckTanksAlive();
			yield return new WaitForSeconds(1);//Search with 1s gap
        }

		IncrementWave();

		Debug.Log("Waiting Routine End");
	}

	IEnumerator DeployBackupRoutine()
	{
        while (true)
        {
			int res = enemyDetect.GetIndexWhereEnemyIsAt();//Get where enemy has invaded 
			if (res != -1)
			{
				int[] arr = GetAppropriateSpawnIndex(res);//Get list of appropriate spawn indexes
				int deployIndex = arr[Random.Range(0, arr.Length)];//get final deploy index
				SpawnEnemy2(deployIndex, waves[nextWave]);//deploy!
			}

			yield return new WaitForSeconds(Random.Range(-5f, 15f));
        }
	}

	//-------------------
	//Utility functions
	//-------------------

	bool SpawnEnemy(Wave wave)
	{
		Debug.Log("Spawning Enemy: ");
		Debug.Log("Spawn Enemy");

		int a = UnityEngine.Random.Range(0, 3);
		int b = UnityEngine.Random.Range(0, wave.tankIndexes.Length);

		if (ccScript.IsTankSpwnAreaAvailable(a))
		{
			ccScript.DeployTank(a, b + 1);
			return true;
		}
		return false;
	}

	void SpawnEnemy2(int pos, Wave wave)
	{
		int b = UnityEngine.Random.Range(0, wave.tankIndexes.Length);

		if (ccScript.IsTankSpwnAreaAvailable(pos))
		{
			ccScript.DeployTank(pos, b + 1);
		}
	}

	bool CheckTanksAlive()
	{
		return ccScript.currDeployedTanks.Count > 0;
	}

	void IncrementWave()
	{
		nextWave = (nextWave + 1) % waves.Length;
	}

	//Get spawn indexes based on where enemy is located.
	int[] GetAppropriateSpawnIndex(int targetIndex)
	{
		int[] res = null;

        switch (targetIndex)
        {
			case 0:
				res = new int[2];
				res[0] = 0;res[1] = 1;
				break;
			case 1:
				res = new int[3];
				res[0] = 0; res[1] = 1; res[2] = 2;
				break;
			case 2:
				res = new int[2];
				res[0] = 1; res[1] = 2;
				break;
			default:
                break;
        }

		return res;
    }

}
