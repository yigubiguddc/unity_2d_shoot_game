using System.Collections;
using UnityEngine;

public class Wavemanager : MonoBehaviour
{
    //��ʱ������ң�ȷ������ֻ�������Χ����
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private LivingEntity PlayerEntity;

    //seconds between two waves.
    public static float timeBetweenWaves = 2.0f;
    public int wavenumber;
    public static event System.Action<int> OnNewWave;    //�¼�
    public static event System.Action HealTips;

    //�����waves��Wave�ĺϼ�
    public Wave[] waves;        //ÿ��Wave����enemy��count�Լ�TimeBetweenSpawn
    private Wave _currentWave;
    private int _currentWaveIndex;       //��ǰ��������(��������ܴ���������Ϊÿ����������������ᱻ��գ�������Ҫ����һ���µ���������¼����)
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
        //����Ҫʹ��Э��
        StartCoroutine(NextWaveCoroutine());
    }


    private IEnumerator NextWaveCoroutine()
    {
        _currentWaveIndex++;
        //��upgradeʱ�����жϵĻ������waveNumber���ۼ�����
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
                //�����Ѩ����Monster��
                int spawnIndex = Random.Range(0, spawnPoints.Length);

                //Debug.Log("��Ѩ��"+spawnPoints.Length);
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
                //���ȡ����
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






                yield return new WaitForSeconds(_currentWave.timeBetweenSpawn);     //����
            }
            _isWaveValid = true;
        }
        else
        {
            _isWaveValid = false;
            _currentWaveIndex = 0;      //����ѭ��
            _upgrade += 0.8f;
            StartCoroutine(NextWaveCoroutine());

        }


    }


    //��LivingEntityѪ��Ϊ0ʱ��ִ��OnDeath�¼����ĵ����к����������OnEnemyDeath��������һ�����ĺ���
    //�������ã���һ���е�һ����������ʱ����_enemyRemainAliveCount���٣�����������ˣ�������һ����Э�̡�
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
