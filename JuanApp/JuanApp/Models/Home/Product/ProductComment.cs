using System.ComponentModel.DataAnnotations;

namespace JuanApp.Models.Home.Product
{
    public class ProductComment : BaseEntity
    {
        [Required(ErrorMessage = "Comment is required")]
        public string Text { get; set; }
        public int Rate { get; set; }
        public Product Product { get; set; }
        public int ProductId { get; set; }
        public AppUser AppUser { get; set; }
        public string AppUserId { get; set; }
        public CommentStatus Status { get; set; } = CommentStatus.Pending;
    }
    public enum CommentStatus
    {
        Pending,
        Approved,
        Rejected 
    }
}
