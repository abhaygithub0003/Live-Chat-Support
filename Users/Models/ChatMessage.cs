using System;
using System.ComponentModel.DataAnnotations;

namespace Users.Models
{
    public class ChatMessage
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string FromUser { get; set; }
        [Required]
        public string ToUser { get; set; }
        [Required]
        public string Message { get; set; }
        [Required]
        public DateTime Timestamp { get; set; }
    }
}
