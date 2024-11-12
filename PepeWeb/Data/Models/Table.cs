﻿using System.ComponentModel.DataAnnotations;

namespace PepeWeb.Data.Models
{
    public class Table
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
    }
}
