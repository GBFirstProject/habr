namespace FirstProject.Messages
{/// <summary>
 /// Класс сообщения запроса модератора
 /// </summary>
    public class ModeratorRequested
    { 
        /// <summary>
        /// Идентификатор отправителя
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        ///Сообщение
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// Ссылка на комментарий
        /// </summary>
        public string Reference { get; set; }
        /// <summary>
        /// Дата запроса
        /// </summary>
        public DateTime CreatedAt { get; set; }

        public ModeratorRequested(Guid userId, string content, string reference, DateTime creaatedAt)
        {
            UserId= userId;
            Content= content;
            Reference = reference;
            CreatedAt = creaatedAt;
        }



    }
}