namespace Bombazo.Presistance.FileManager {
    public class FileManagerException : Exception {
        public FileManagerException() {

        }
        public FileManagerException(string message) : base(message) {

        }
        public FileManagerException(string message, Exception innerException) : base(message, innerException) { }
    }
}
