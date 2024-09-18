using System;
using System.Collections.Generic;

namespace Estudio.Repository.Helpers
{
    public class Response
    {
        private bool _isOk;
        private string _message;
        private Object _object;
        private IEnumerable<string> _errors;
        private List<object> _additionalObjects; // Nueva propiedad para almacenar objetos adicionales

        public Response()
        {
            IsOk = true;
            Message = "Success";
            _additionalObjects = new List<object>(); // Inicializar la lista de objetos adicionales
        }

        public bool IsOk { get { return _isOk; } set { _isOk = value; } }
        public string Message { get { return _message; } set { _message = value; } }
        public List<object> AdditionalObjects { get { return _additionalObjects; } }
        public Object Object { get { return _object; } set { _object = value; } }
        public IEnumerable<string> Errors { get { return _errors; } set { _errors = value; } }
        

        // Método para agregar un objeto adicional a la lista
        public void AddAdditionalObject(object additionalObject)
        {
            _additionalObjects.Add(additionalObject);
        }
    }
}
