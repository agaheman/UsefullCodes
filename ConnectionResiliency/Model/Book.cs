namespace ConnectionResiliency
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Book")]
    public partial class Book
    {
        public int BookId { get; set; }

        [Required]
        [StringLength(20)]
        public string Name { get; set; }

        [StringLength(10)]
        public string ISBN { get; set; }

        public short CategoryId { get; set; }

        public int AuthorPersonId { get; set; }

        public virtual Category Category { get; set; }

        public virtual Person Person { get; set; }
    }
}
