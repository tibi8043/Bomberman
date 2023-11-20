namespace Bombazo.Presistence.FileManager {
    public class TxtFileManager : ITxtFileManager {
        public async Task<string[]> LoadAsync(string path) {
            try {
                return await File.ReadAllLinesAsync(path);
            }
            catch (Exception exc) {
                throw new FileManagerException("Hibás bemeneti fájl!", exc);
            }
        }
    }
}