﻿using System;
using System.Linq;
using System.Collections.Generic;

using System.ComponentModel.DataAnnotations;

namespace MusicHub.Data.Models
{
    public class Album
    {
        public Album()
        {
            this.Songs = new HashSet<Song>();
        }

        public int Id { get; set; }

        [Required]
        [MaxLength(40)]
        public string Name { get; set; }

        [Required]
        public DateTime ReleaseDate { get; set; }
        public decimal Price => this.Songs.Sum(s => s.Price);
        public int? ProducerId { get; set; }
        public virtual Producer Producer { get; set; }
        public virtual ICollection<Song> Songs { get; set; }
    }
}