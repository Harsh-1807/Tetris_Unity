using UnityEngine;

public class TetrisBlock : MonoBehaviour
{
    public Vector3 rotationPoint;
    public float fallTime = 0.8f;
    public static int height = 20;
    public static int width = 10;
    private static Transform[,] grid = new Transform[width, height];

    private float previousTime;
    private bool isGameOver = false;

    private AudioManager audioManager;

    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        if (audioManager == null)
        {
            Debug.LogError("AudioManager not found in the scene.");
        }
    }

    void Update()
    {
        if (!isGameOver)
        {
            HandleMovement();
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
    }

    void HandleMovement()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Move(new Vector3(-1, 0, 0));
            audioManager.PlayMoveSound();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Move(new Vector3(1, 0, 0));
            audioManager.PlayMoveSound();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Rotate();
            audioManager.PlayRotateSound();
        }

        if (Time.time - previousTime > (Input.GetKey(KeyCode.DownArrow) ? fallTime / 10 : fallTime))
        {
            Fall();
        }
    }

    void Move(Vector3 movement)
    {
        transform.position += movement;
        if (!ValidMove())
        {
            transform.position -= movement;
        }
    }

    void Rotate()
    {
        transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), 90);
        if (!ValidMove())
        {
            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -90);
        }
    }

    void Fall()
    {
        transform.position += new Vector3(0, -1, 0);
        if (!ValidMove())
        {
            transform.position -= new Vector3(0, -1, 0);
            AddToGrid();
            CheckForLines();
            audioManager.PlayDropSound();

            if (!isGameOver)
            {
                FindObjectOfType<SpawnTetromino>().NewTetromino();
                this.enabled = false;
            }
        }
        previousTime = Time.time;
    }

    void CheckForLines()
    {
        int lines = 0;
        for (int i = height - 1; i >= 0; i--)
        {
            if (HasLine(i))
            {
                DeleteLine(i);
                RowDown(i);
                lines++;
            }
        }

        if (lines > 0)
        {
            ScoreManager.AddScore(lines);
            // audioManager.PlayLineClearSound(); 
        }
    }

    bool HasLine(int i)
    {
        for (int j = 0; j < width; j++)
        {
            if (grid[j, i] == null)
                return false;
        }
        return true;
    }

    void DeleteLine(int i)
    {
        for (int j = 0; j < width; j++)
        {
            Destroy(grid[j, i].gameObject);
            grid[j, i] = null;
        }
    }

    void RowDown(int i)
    {
        for (int y = i; y < height; y++)
        {
            for (int j = 0; j < width; j++)
            {
                if (grid[j, y] != null)
                {
                    grid[j, y - 1] = grid[j, y];
                    grid[j, y] = null;
                    grid[j, y - 1].transform.position -= new Vector3(0, 1, 0);
                }
            }
        }
    }

    void AddToGrid()
    {
        foreach (Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);

            if (roundedY >= height - 3)
            {
                GameOver();
            }

            grid[roundedX, roundedY] = children;
        }
    }

    void GameOver()
    {
        isGameOver = true;
        Debug.Log("Game Over! Press 'R' to Restart.");
    }

    void RestartGame()
    {
        audioManager.PlayNewGameSound();
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (grid[x, y] != null)
                {
                    Destroy(grid[x, y].gameObject);
                    grid[x, y] = null;
                }
            }
        }

        ScoreManager.ResetScore();
        fallTime = 0.8f;
        isGameOver = false;
        FindObjectOfType<SpawnTetromino>().NewTetromino();
    }

    public bool ValidMove()
    {
        foreach (Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);

            if (roundedX < 0 || roundedX >= width || roundedY < 0 || roundedY >= height)
            {
                return false;
            }

            if (grid[roundedX, roundedY] != null)
                return false;
        }

        return true;
    }

    void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = 50;
        style.normal.textColor = Color.white;

        if (!isGameOver)
        {
            GUI.Label(new Rect(10, 10, 200, 30), $"Score: {ScoreManager.Score}", style);
            GUI.Label(new Rect(10, 40, 200, 30), $"Level: {ScoreManager.Level}", style);
            GUI.Label(new Rect(10, 70, 200, 30), $"Lines: {ScoreManager.LinesCleared}", style);
        }
        else
        {
            style.fontSize = 36;
            style.alignment = TextAnchor.MiddleCenter;

            GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 60, 200, 40), "Game Over!", style);
            GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 20, 200, 40), $"Final Score: {ScoreManager.Score}", style);
            GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2 + 20, 200, 40), "Press 'R' to Restart", style);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(0, height - 5, 0), new Vector3(width, height - 5, 0));
    }
}