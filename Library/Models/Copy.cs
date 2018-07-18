using System;
namespace Library.Models
{
    public class Copy
    {
        public int Id { get; set; }
        public int Total { get; set; }

        public Copy(int copies, int id=0)
        {
            Id = id;
            Total = copies;
        }
    }
}
