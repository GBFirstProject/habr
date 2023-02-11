namespace FirstProject.ArticlesAPI.Models.DTO
{
    public class ResponseDTO
    {
        public bool IsSuccess { get; set; }
        public object Result { get; set; } = null!;
        public string DisplayMessage { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
