using System.Data;

namespace FirstProject.Messages
{/// <summary>
 /// Интерфейс сообщения запроса модератора
 /// </summary>
    public interface IModeratorRequested
    {
        /// <summary>
        /// Идентификатор отправителя
        /// </summary>
        public Guid UserId{ get; }

        /// <summary>
        /// Сообщение
        /// </summary>
        string Content { get; }

        /// <summary>
        /// Ссылка на комментарий
        /// </summary>
        string Reference { get;  }

        /// <summary>
        /// Дата запроса
        /// </summary>
        DateTime TimeCreated { get; }
    
    }
}