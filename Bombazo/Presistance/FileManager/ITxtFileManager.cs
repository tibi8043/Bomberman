namespace Bombazo.Presistance.FileManager {
    public interface ITxtFileManager {
        Task<string[]> LoadAsync();
    }
}
