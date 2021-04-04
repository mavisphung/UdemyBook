using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace UdemyBook.Models
{
    public class OrderHeader
    {
        public int Id { get; set; }

        [Required]
        public string AppUserId { get; set; }
        [ForeignKey("AppUserId")]
        public ApplicationUser AppUser { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        public DateTime ShippingDate { get; set; }

        public double OrderTotal { get; set; }

        public string OrderStatus { get; set; }
        public string PaymentStatus { get; set; }

        public string TransactionId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string City { get; set; }

    }
}
