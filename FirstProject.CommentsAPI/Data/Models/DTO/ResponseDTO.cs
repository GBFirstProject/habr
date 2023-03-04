namespace FirstProject.CommentsAPI.Data.Models.DTO
{
    public class ResponseDTO
    {
        public bool IsSuccess { get; set; }
        public object Result { get; set; } = null!;
        public string DisplayMessage { get; set; } = string.Empty;
        public List<string> ErrorMessages { get; } = new();
    }
}
