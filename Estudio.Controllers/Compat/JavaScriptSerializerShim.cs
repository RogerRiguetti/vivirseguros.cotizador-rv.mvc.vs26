using Newtonsoft.Json;

namespace System.Web.Script.Serialization
{
    // Shim para compatibilidad con JavaScriptSerializer en proyectos modernos
    public class JavaScriptSerializer
    {
        public string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public T Deserialize<T>(string input)
        {
            return JsonConvert.DeserializeObject<T>(input);
        }

        public object DeserializeObject(string input)
        {
            return JsonConvert.DeserializeObject(input);
        }
    }
}
