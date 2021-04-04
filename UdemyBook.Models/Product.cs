using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace UdemyBook.Models
{
    public class Product
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }
        public string ImageUrl { get; set; }

        public DateTime updatedDay { get; set; }

        [Required]
        [MaxLength(50)]
        public string Language { get; set; }

        [Required]
        [Range(1, 10000)]
        public double Price { get; set; }

        [Required]
        public int AuthorID { get; set; }

        [ForeignKey("AuthorID")]
        public Author Author { get; set; }

        [Required]
        public int CategoryID { get; set; }

        [ForeignKey("CategoryID")]
        public Category Category { get; set; }

    }
}
