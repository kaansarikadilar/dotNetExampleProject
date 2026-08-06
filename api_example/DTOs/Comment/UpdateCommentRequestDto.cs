using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api_example.DTOs.Comment
{
    public class UpdateCommentRequestDto
    {
        [Required]
        [MinLength(2,ErrorMessage ="Title must be longer than 2 characters")]
        [MaxLength(30,ErrorMessage ="Title must be shorter than 30 characters")]
        public string Title { get; set; } = string.Empty;
        [Required]
        [MinLength(2,ErrorMessage ="Content must be longer than 2 characters")]
        [MaxLength(30,ErrorMessage ="Content must be shorter than 30 characters")]
        public string Content { get; set; } = string.Empty; 

    }
}