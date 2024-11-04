using Dawem.Enums.Generals;

namespace Dawem.Models.Dtos.Dawem.Attendances
{
    public class FingerprintModel
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public FingerPrintType Type { get; set; }
        public RecognitionWay RecognitionWay { get; set; }
        public string FingerprintMobileCode { get; set; }
        public bool FromExcel { get; set; } = false;
    }
}
