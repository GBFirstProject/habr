using FirstProject.CommentsAPI.Data.Models.DTO;
using System.Collections.Concurrent;

namespace FirstProject.CommentsAPI.Utils
{
    public class CommentsCache
    {
        private readonly ConcurrentDictionary<string, List<CommentJsonDTO>> _comments;
        // key - {articleId}-{count}-{index}

        public CommentsCache()
        {
            _comments = new();
        }

        public List<CommentJsonDTO>? Get(string key)
        {
            _comments.TryGetValue(key, out var result);
            return result;
        }

        public void Add(string key, List<CommentJsonDTO> json)
        {
            if (_comments.ContainsKey(key))
            {
                _comments.TryRemove(key, out _);
            }
            _comments.TryAdd(key, json);
        }

        public void Drop(Guid articleId)
        {
            var entries = _comments.Where(s => s.Key.Contains(articleId.ToString()));
            foreach (var entry in entries)
            {
                _comments.TryRemove(entry.Key, out _);
            }
        }
    }
}
