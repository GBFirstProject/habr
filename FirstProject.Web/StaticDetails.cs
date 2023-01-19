namespace FirstProject.Web
{
    public static class StaticDetails
    {
        public static string ArticleAPIBase { get; set; }
        public enum ApiType { 
            GET,
            POST,
            PUT,
            DELETE
        }
    }
}
