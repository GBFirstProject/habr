namespace FirstProject.Messages
{/// <summary>
 /// Класс сообщения запроса модератора
 /// </summary>
    public class ModeratorRequested : IModeratorRequested
    {/// <summary>
     /// Идентификатор сообщения
     /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Идентификатор отправителя
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        ///Сообщение
        /// </summary>
        public string Content { get; set; } = string.Empty;
        /// <summary>
        /// Ссылка на комментарий
        /// </summary>
        public string Reference { get; set; } = string.Empty;
        /// <summary>
        /// Дата запроса
        /// </summary>
        public DateTime TimeCreated { get; set; }

    }
}