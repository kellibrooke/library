using System;
namespace Library.Models
{
    public class Author
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Author(string authorName, int id=0)
        {
            Id = id;
            Name = authorName;
        }
    }
}
