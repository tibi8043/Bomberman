using Bombazo.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombazoWPF.ViewModel
{
    public class Field : ViewModelBase
    {
        private FieldType _fieldType;
        private Position _position;

        public Position Position {
            get => _position;           
        }

        public FieldType FieldType{
            get => _fieldType;

            set {
                if (FieldType != value) {
                    _fieldType = value;
                    OnPropertyChanged();
                }
            }
        }        

        public Field(FieldType fieldType, Position position) {
            _position = position;
            _fieldType = fieldType;
        }
    }
}
