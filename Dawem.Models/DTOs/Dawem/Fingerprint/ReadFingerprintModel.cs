using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Models.Dtos.Dawem.Fingerprint
{
    public class ReadFingerprintModel
    {
        public string SN { get; set; }
        public string Table { get; set; }
        public string INFO { get; set; }
        public string Stamp { get; set; }
        public string Options { get; set; }
        public string Pushver { get; set; }
        public string Language { get; set; }
        public string PushCommkey { get; set; }
        public Stream RequestBody { get; set; }
        public string RequestBodyString { get; set; }
        [NotMapped] // for fingerprint log
        public string LogType { get; set; }
        [NotMapped] // for fingerprint log
        public Exception Exception { get; set; }
    }
}
