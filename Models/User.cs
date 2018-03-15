using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace ArtMapApi.Models
{
    public class User : IdentityUser
    {

        [DataType(DataType.Date)]
        public DateTime CreatedAt { get; set; }


        [DataType(DataType.Date)]
        public DateTime UpdatedAt { get; set; }


        public string Avatar { get; set; }

        public Int64 FacebookId { get; set; }

    }
}