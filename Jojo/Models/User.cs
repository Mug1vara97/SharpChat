﻿using System.ComponentModel.DataAnnotations;

namespace Jojo.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public bool IsEmailVerified { get; set; }
        public string? ProfileDescription { get; set; }
        public byte[]? ProfilePhoto { get; set; }
        public string? ProfilePhotoContentType { get; set; }
        public string? BackgroundColor { get; set; }

    }
}