namespace Dawem.Models.Dtos.Others
{
    public class UploadResult
    {
        public virtual string FileName { get; set; }
        public virtual string FolderName { get; set; }
        public virtual int Type { get; set; }
        public virtual string Path { get; set; }
    }
}
