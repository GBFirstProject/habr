namespace FirstProject.Messages
{/// <summary>
 /// Класс сообщения запроса модератора
 /// </summary>
    public class ModeratorRequested : MessageBase
    {
        /// <summary>
        /// Идентификатор отправителя
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        ///Сообщение
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// Ссылка на комментарий
        /// </summary>
        public string Reference { get; set; }
    
        public ModeratorRequested(string username, string content, string reference)
        {
            Username = username;
            Content = content;
            Reference = reference;
            CreatedAt = DateTime.UtcNow;
        }

        public ModeratorRequested()
        {
            
        }
    }
}