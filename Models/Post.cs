using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArtMapApi.Models
{
    public class Post
    {
        // [Key]
        public int PostId { get; set; }

        public string UserId { get; set; }

        // [Required]
        public string PhotoId { get; set; }

        // [Required]
        // [StringLength(25)]
        public string Artist { get; set; }
        // [Required]
        // [StringLength(25)]
        public string Title { get; set; }

        // [Required]
        // [DataType(DataType.Date)]
        public DateTime CreatedAt { get; set; }

        // [Required]
        // [DataType(DataType.Date)]
        public DateTime UpdatedAt { get; set; }

        // [Required]
        public decimal Lat { get; set; }

        // [Required]
        public decimal Long { get; set; }

        // [Required]
        public string PhotoURI { get; set; }

        // [Required]
        // [DefaultValue(0)]
        public int LikesCount { get; set; }

        // [Required]
        // [DefaultValue(0)]
        public int CommentsCount { get; set; }
    }
}