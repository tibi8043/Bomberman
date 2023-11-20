namespace Bombazo.Presistence.FileManager {
    public interface ITxtFileManager {
        public Task<string[]> LoadAsync(string path);
    }
}