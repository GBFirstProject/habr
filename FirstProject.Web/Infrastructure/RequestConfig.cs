namespace FirstProject.Web.Infrastructure
{
    public static class RequestConfig
    {
        public static string ArticlesAPIBase { get; set; } = String.Empty;
        public static string CommentsAPIBase { get; set; } = String.Empty;
        public static string MessagesBase { get; set; } = String.Empty;
        public static string NotificationsAPIBase { get; set; } = String.Empty;
        public static string PersonalAccountAPIBase { get; set; } = String.Empty;

        public enum ApiType
        {
            GET,
            POST,
            PUT,
            DELETE
        }
    }
}
