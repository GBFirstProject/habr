using System.Data;

namespace FirstProject.Messages
{
    public interface IModeratorRequested
    {
        public Guid Id{ get; }
        public Guid UserId{ get; }
        string Content { get; }
        string Reference { get;  }
        DateTime TimeCreated{ get; }

    
       
    }
}