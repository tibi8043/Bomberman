﻿namespace Bombazo.Presistance {
    public class FileValidationException : Exception{

        public FileValidationException(string message) : base(message) {

        }
        public FileValidationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
