using System.Collections;
using UnityEngine;

public class Wavemanager : MonoBehaviour
{
    //随时跟踪玩家，确保怪物只在玩家周围生成
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private LivingEntity PlayerEntity;

    //seconds between two waves.
    public static float timeBetweenWaves = 2.0f;
    public int wavenumber;
    public static event System.Action<int> OnNewWave;    //事件
    public static event System.Action HealTips;

    //这里的waves是Wave的合集
    public Wave[] waves;        //每个Wave包含enemy，count以及TimeBetweenSpawn
    private Wave _currentWave;
    private int _currentWaveIndex;       //当前波数索引(这个并不能代表波数，因为每次升级这个索引都会被清空，我们需要创建一个新的数字来记录波数)
    private bool _isWaveValid = true;
    private int _enemyRemainAliveCount;
    public float _upgrade;

    [SerializeField] private GameObject BigGirlAnimPrefab;
    [SerializeField] private GameObject[] SlimeBodyAnimPrefab;


    private void Start()
    {


        if (spawnPoints.Length == 0)
        {
            Debug.LogError("Can't find spawn points");
            return;
        }
        if (PlayerEntity == null) 
        {
            Debug.LogError("Can't find Player!!Bro!");
            return;
        }

        PlayerEntity.OnDeath += OnPlayerDeath;
        //GameEvents.GameOver += OnPlayerDeath;
        //这里要使用协程
        StartCoroutine(NextWaveCoroutine());
    }


    private IEnumerator NextWaveCoroutine()
    {
        _currentWaveIndex++;
        //在upgrade时不加判断的话这里的waveNumber会累加两次
        if (_isWaveValid)
        {
            OnNewWave?.Invoke(++wavenumber);
            HealTips?.Invoke();
        }

        yield return new WaitForSeconds(timeBetweenWaves);

        if (_currentWaveIndex - 1 < waves.Length)
        {
            _currentWave = waves[_currentWaveIndex - 1];
            _enemyRemainAliveCount = _currentWave.count;
            for (int i = 0; i < _currentWave.count; i++)
            {
                if(PlayerEntity == null)
                {
                    yield break;
                }
                //随机巢穴出现Monster。
                int spawnIndex = Random.Range(0, spawnPoints.Length);

                //Debug.Log("巢穴数"+spawnPoints.Length);
                /*BigGirl biggirl = Instantiate(_currentWave.biggirl, spawnPoints[spawnIndex].position, Quaternion.identity);
                GameObject girlAnimInstance = Instantiate(BigGirlAnimPrefab, spawnPoints[spawnIndex].position, Quaternion.identity);
                biggirl.transform.SetParent(girlAnimInstance.transform);
                biggirl.parentTransform = girlAnimInstance.transform;
                biggirl.Setup(PlayerEntity.transform, _upgrade);
                biggirl.OnDeath += OnEnemyDeath;*/

                /*Beetle beetle = Instantiate(_currentWave.beetle, spawnPoints[spawnIndex].position, Quaternion.identity);
                beetle.Setup(PlayerEntity.transform, _upgrade);
                beetle.OnDeath += OnEnemyDeath;*/
                Slime slime = Instantiate(_currentWave.slimeBody, spawnPoints[spawnIndex].position, Quaternion.identity);
                //随机取怪物
                GameObject slimeAnim = Instantiate(SlimeBodyAnimPrefab[Random.Range(0, 8)], spawnPoints[spawnIndex].position, Quaternion.identity);
                //Slime slime = ObjectPool.Instance.GetObject(_currentWave.slimeBody.gameObject).GetComponent<Slime>();
                //GameObject slimeAnim = ObjectPool.Instance.GetObject(SlimeBodyAnimPrefab[Random.Range(0, 8)]);
                //slime.transform.position = spawnPoints[spawnIndex].position;
                //slimeAnim.transform.position = spawnPoints[spawnIndex].position;


                //GameObject slimeAnim = Instantiate(SlimeBodyAnimPrefab[2], spawnPoints[spawnIndex].position, Quaternion.identity);
                slime.transform.SetParent(slimeAnim.transform);
                slime.parentTransform = slimeAnim.transform;
                slime.Setup(PlayerEntity.transform, _upgrade);
                slime.OnDeath += OnEnemyDeath;






                yield return new WaitForSeconds(_currentWave.timeBetweenSpawn);     //喵甚
            }
            _isWaveValid = true;
        }
        else
        {
            _isWaveValid = false;
            _currentWaveIndex = 0;      //无限循环
            _upgrade += 0.8f;
            StartCoroutine(NextWaveCoroutine());

        }


    }


    //再LivingEntity血量为0时会执行OnDeath事件订阅的所有函数，这里的OnEnemyDeath就是其中一个订阅函数
    //函数作用：当一波中的一个怪物死亡时，将_enemyRemainAliveCount减少，如果减到零了，开启下一波的协程。
    private void OnEnemyDeath()
    {
        _enemyRemainAliveCount--;
        if(_enemyRemainAliveCount==0)
        {
            StartCoroutine(NextWaveCoroutine());
        }
    }
    private void OnPlayerDeath()
    {
        StopCoroutine(NextWaveCoroutine());
    }

}
