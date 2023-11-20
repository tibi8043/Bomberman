namespace Bombazo.Presistance.FileManager {
    public class TxtFileManager : ITxtFileManager {
        private string? _path;

        public TxtFileManager() { }

        public TxtFileManager(string path) {
            _path = path;
        }
        public async Task<string[]> LoadAsync() {
            if (_path != null) {
                try {
                    return await File.ReadAllLinesAsync(_path);
                }
                catch (Exception exc) {
                    throw new FileManagerException("Hibás bemeneti fájl!", exc);
                }
            }
            return Array.Empty<string>();
        }
    }
}
