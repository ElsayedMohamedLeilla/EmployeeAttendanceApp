using Dawem.Domain.Entities.Summons;
using Dawem.Enums.Generals;
using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Attendances
{
    [Table(nameof(EmployeeAttendanceCheck) + LeillaKeys.S)]
    public class EmployeeAttendanceCheck : BaseEntity
    {
        #region Forign Key
        public int EmployeeAttendanceId { get; set; }
        [ForeignKey(nameof(EmployeeAttendanceId))]
        public EmployeeAttendance EmployeeAttendance { get; set; }

        public int? SummonId { get; set; }
        [ForeignKey(nameof(SummonId))]
        public Summon Summon { get; set; }

        #endregion
        public TimeOnly Time { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string IpAddress { get; set; }
        public FingerPrintType FingerPrintType { get; set; }
        public RecognitionWay RecognitionWay { get; set; }

    }
}
