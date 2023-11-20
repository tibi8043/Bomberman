namespace Bombazo.Model {
    public class Field {
        private readonly FieldType _fieldType;
        public FieldType FieldType => _fieldType;

        public Field(FieldType fieldType) {
            _fieldType = fieldType;
        }

        public bool IsExplosive() {
            return _fieldType == FieldType.BOMB
                   || _fieldType == FieldType.EXPLOSION
                   || _fieldType == FieldType.PLAYERANDBOMB;
        }
    }
}