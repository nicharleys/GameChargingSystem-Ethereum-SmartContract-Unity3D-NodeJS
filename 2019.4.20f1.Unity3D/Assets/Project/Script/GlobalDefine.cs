public interface ISave {
    void SaveData();
}

public delegate void SaveDataDelegate();

public class GameState {
    public static bool forcePause;
}