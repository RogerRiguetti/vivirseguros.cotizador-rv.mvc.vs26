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
        public Response()
        {
            IsOk = true;
            Message = "Success";
        }
        public bool IsOk { get { return _isOk; } set { _isOk = value; } }
        public string Message { get { return _message; } set { _message = value; } }
        public Object Object { get { return _object; } set { _object = value; } }
        public IEnumerable<string> Errors { get { return _errors; } set { _errors = value; } }
    }
}