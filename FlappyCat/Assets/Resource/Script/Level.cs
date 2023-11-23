using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    private static Level my_Instance;
    public static Level GetInstance { get { return my_Instance; } }

    private const float CAMERA_ORTHO_SIZE = 50f;
    private const float PIPE_BODY_WIDTH = 7.8f;
    private const float PIPE_HEAD_HEIGHT = 3.75f;
    private const float PIPE_MOVE_SPEED = 30f;
    private const float PIPE_DESTORY_POS = -100f;
    private const float PIPE_SPAWN_POS = 100f;
    private const float PLANET_DESTORY_POS = -160f;
    private const float PLANET_SPAWN_POS = 160f;
    private const float BIRD_X_POSITION = 0F;

    private List<Pipe> pipeList;
    private List<Transform> planetList;
    private float planetSpawnTimer;
    private int pipeNum;
    private int pipePassNum;
    private float pipeSpawnTimer;
    private float pipeSpawnTimerMax;
    private float gapSize;
    private levelStateEnum levelState;

    private void Awake()
    {
        my_Instance = this;
        
        pipeList = new List<Pipe>();
        pipeSpawnTimerMax = 1.5f;
        gapSize = 50f;
        setDifficulty(difficultyEnum.Easy);
        levelState=levelStateEnum.WaitingToFly;
    }
    private void Start()
    {
        Bird.GetInstance.OnDied += bird_OnDied;
        Bird.GetInstance.OnStartFly += bird_OnStartFlying;
        spawnInitialPlanets();
    }

    private void bird_OnDied(object sender, System.EventArgs e)
    {
        //CMDebug.TextPopupMouse("BirdDead!");
        levelState = levelStateEnum.BirdDead;       
    }

    private void bird_OnStartFlying(object sender, System.EventArgs e)
    {
        levelState=levelStateEnum.BirdFlying;
    }

    private void Update()
    {
        if(levelState == levelStateEnum.BirdFlying)
        {
            handlePipeMovement();
            handlePipeSpawn();
            handlePlanets();
        }      
    }

    private enum difficultyEnum
    {
        Easy,Medium,Hard,Impossible
    }
    private enum levelStateEnum
    {
       WaitingToFly,BirdFlying,BirdDead
    }
    private difficultyEnum getDfficulty()
    {
        if (pipeNum >= 300) return difficultyEnum.Impossible;
        if (pipeNum >= 20) return difficultyEnum.Hard;
        if (pipeNum >=10) return difficultyEnum.Medium;      
        return difficultyEnum.Easy;
    }

    private void spawnInitialPlanets()
    {
        planetList = new List<Transform>();
        Transform planetTransform;
        float planetY = 30f;
        planetTransform = Instantiate(GameAssets.GetInstance.SpaceObj,new Vector3(0,planetY,0),Quaternion.identity);
        planetList.Add(planetTransform);
    }

/*    IEnumerator IHandlePlanets()
    {
        yield return new WaitForSeconds(6f);
        handlePlanets();
    }*/
    private void handlePlanets()
    {
        planetSpawnTimer -=Time.deltaTime;
        if(planetSpawnTimer < 0)
        {
            float planetSpawnTimerMax = 10f;
            planetSpawnTimer = planetSpawnTimerMax;
            float planetY = 30f;
            Transform planetTransform = Instantiate(GameAssets.GetInstance.SpaceObj, new Vector3(PLANET_SPAWN_POS, planetY, 0), Quaternion.identity);
            planetList.Add(planetTransform);
        }
        for(int i = 0; i < planetList.Count; i++)
        {
            Transform planetTransform = planetList[i];           
            planetTransform.position += new Vector3(-1, 0, 0) * PIPE_MOVE_SPEED * Time.deltaTime * 0.7f;

                if (planetTransform.position.x < PLANET_DESTORY_POS)
                {
                    Destroy(planetTransform.gameObject);
                    planetList.RemoveAt(i);
                    i--;
                }           
        }
        
    }

    private void setDifficulty(difficultyEnum dif)
    {
        switch (dif)
        {
            case difficultyEnum.Easy:
                gapSize = Random.Range(30f,50f);
                pipeSpawnTimerMax = 1.5f;
                break;
            case difficultyEnum.Medium:
                gapSize = Random.Range(25f, 40f);
                pipeSpawnTimerMax = 1.25f;
                break;
            case difficultyEnum.Hard:
                gapSize = Random.Range(20f, 30f);
                pipeSpawnTimerMax = 1f;
                break;
            case difficultyEnum.Impossible:
                gapSize = Random.Range(15f, 25f);
                pipeSpawnTimerMax = 0.85f;
                break;
        }
    }
    private void handlePipeSpawn()
    {
        pipeSpawnTimer -=Time.deltaTime;
        if(pipeSpawnTimer < 0)
        {
            pipeSpawnTimer +=pipeSpawnTimerMax;
            float heightEdgeLimit = 10f;
            float totalHeight = CAMERA_ORTHO_SIZE * 2f;
            float minHeight = gapSize * 0.5f + heightEdgeLimit;
            float maxHeight = totalHeight - gapSize * 0.5f - heightEdgeLimit;
            float height = Random.Range(minHeight,maxHeight);
            CreateGapPipes(height, gapSize, PIPE_SPAWN_POS);
        }
    }

    private void handlePipeMovement()
    {
        for(int i = 0; i < pipeList.Count; i++)
        {
            Pipe pipe = pipeList[i];
            bool isRightOfBird = pipe.GetXPos() > BIRD_X_POSITION;
            pipe.Move();
            if(isRightOfBird && pipe.GetXPos() <= BIRD_X_POSITION && pipe.IsBottom())
            {
                pipePassNum++;
                SoundManager.GetInstance.PlaySound(SoundManager.audioClipEnum.score);
            }
            if(pipe.GetXPos() < PIPE_DESTORY_POS)
            {
                pipe.DestorySelf();
                pipeList.Remove(pipe);
                i--;
            }
        }
    }

    private void CreateGapPipes(float gapY, float gapSize, float xPos)
    {
        createPipe(gapY - gapSize * 0.5f, xPos, true); // >50 is bottom pip is higher
        createPipe(CAMERA_ORTHO_SIZE * 2f - gapY - gapSize * 0.5f, xPos, false); //<50 is top pipe is higher
        pipeNum++;
        setDifficulty(getDfficulty());
    }

    private void createPipe(float height, float xPos, bool isBottom)
    { 
        //set up Pipe Head
        Transform pipeHead = Instantiate(GameAssets.GetInstance.PipeHead);
        float pipeHead_yPos;
        if (isBottom)
        {
            pipeHead_yPos = -CAMERA_ORTHO_SIZE + height - PIPE_HEAD_HEIGHT * 0.5f;
            pipeHead.position = new Vector2(xPos, pipeHead_yPos);
            pipeHead.localScale = new Vector2(1,-1);
        }

        else
        {
            pipeHead_yPos = CAMERA_ORTHO_SIZE - height + PIPE_HEAD_HEIGHT * 0.5f;
            pipeHead.position = new Vector2(xPos, pipeHead_yPos);
        }       

        //set up Pipe Body
        Transform pipeBody = Instantiate(GameAssets.GetInstance.PipeBody);
        float pipeBody_yPos;
        if (isBottom)
            pipeBody_yPos = -CAMERA_ORTHO_SIZE;
        else
        {
            pipeBody_yPos = CAMERA_ORTHO_SIZE;
            pipeBody.localScale = new Vector2(1, -1);
        }
        pipeBody.position = new Vector2(xPos, pipeBody_yPos);

        //Pipe Body Sprite Size
        SpriteRenderer pipeBodySpriteRenderer = pipeBody.GetComponent<SpriteRenderer>();
        pipeBodySpriteRenderer.size = new Vector2(PIPE_BODY_WIDTH, height);


        //Pipe Body Collider Size & Offset
        BoxCollider2D pipeBodyCollider = pipeBody.GetComponent<BoxCollider2D>();
        pipeBodyCollider.size = new Vector2(PIPE_BODY_WIDTH, height);
        pipeBodyCollider.offset = new Vector2(0f , height * 0.5f);

        Pipe pipe = new Pipe(pipeHead, pipeBody,isBottom);
        pipeList.Add(pipe);
    }
    public  int GetPipePassNum()
    {
        return pipePassNum;
    }
    public int GetPipeNum()
    {
        return pipeNum;
    }
    //Represent a single pipe
    private class Pipe
    {
        private Transform pipeHeadTransform;
        private Transform pipeBodyTransform;
        private bool isBottom;

        public Pipe(Transform pipeHeadTransform,Transform pipeBodyTransform,bool isBottom)
        {
            this.pipeHeadTransform = pipeHeadTransform;
            this.pipeBodyTransform = pipeBodyTransform;
            this.isBottom = isBottom;
        }

        public void Move()
        {
            pipeHeadTransform.position += new Vector3(-1, 0, 0) * PIPE_MOVE_SPEED * Time.deltaTime;
            pipeBodyTransform.position += new Vector3(-1, 0, 0) * PIPE_MOVE_SPEED * Time.deltaTime;
        }

        public float GetXPos()
        {
            return pipeHeadTransform.position.x;
        }

        public void DestorySelf()
        {
            Destroy(pipeHeadTransform.gameObject);
            Destroy(pipeBodyTransform.gameObject);          
        }

        public bool IsBottom()
        {
            return isBottom;
        }
    }
}
