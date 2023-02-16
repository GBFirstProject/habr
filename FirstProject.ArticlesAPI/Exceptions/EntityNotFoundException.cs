namespace FirstProject.ArticlesAPI.Exceptions
{
    public class DataNotFoundException : Exception
    {
        public DataNotFoundException(string name, object key) : base($"Запись \"{name}\" ({key} не найдена)")
        {
        }
    }
}
