namespace Dawem.Models.Dtos.Dawem.Settings
{
    public class UpdateSettingDTO
    {
        public int Id { get; set; }
        public int SettingType { get; set; }
        public dynamic Value { get; set; }
    }
}
