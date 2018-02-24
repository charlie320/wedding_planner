using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WeddingPlanner.Models

{
    public class WeddingConfirmation : BaseEntity {
        [Key]
        public int ConfirmationId { get; set; }

        public int GuestId {get; set;}
        public User Guest {get; set;}

        public int WeddingId {get; set;}
        public Wedding Wedding {get; set;}

    }
}