using UnityEngine;
//this is a static class that does not need to be placed on GameObjects

public class StaticScript {}

public static class Tags
{
    public const string SpawnPoint = "SpawnPoint";
    public const string Ring = "Ring";
    public const string Enemy = "Enemy";
    public const string Player = "Player";
    public const string Bullet = "Bullet";
    public const string Target = "Target";
    public const string Wall = "Wall";
    public const string Surface = "Surface";
    public const string Bomb = "Bomb";
    public const string Consumable = "Consumable";
    
}

public static class Events
{
    public const string SpawnEnemy = "SpawnEnemy";
    public const string SpawnHardEnemy = "SpawnHardEnemy";
    public const string PauseGame = "PauseGame";
    public const string unPauseGame = "unPauseGame";
    public const string SpawnConsumable = "SpawnConsumable";
    public const string EnemyModifier = "EnemyModifier";
    public const string UIUpdate = "UIUpdate";
    public const string WonGame = "WonGame";
    public const string InvalidPosition = "InvalidPosition";
    public const string usingBomb = "usingBomb";
    public const string usingGun = "usingGun";
    public const string EnemyDied = "EnemyDied";
    public const string GameOver = "GameOver";
}

public static class Scenes
{
    public const string MainMenu = "MainMenu";
    public const string Gameplay = "Gameplay";
    public const string MusicSelection = "MusicSelection";
    public const string Feedback = "Feedback";
}