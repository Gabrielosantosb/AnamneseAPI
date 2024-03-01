﻿using System.ComponentModel.DataAnnotations.Schema;

namespace CatalogAPI.Models
{
    [Table("pacients")]
    public class PacientModel : BaseModel
    {
        [Column("email")]
        public string Email { get; set; }

        [Column("phone")]
        public string Phone { get; set; }
        [Column("adress")]
        public string Adress { get; set; }

        [Column("profession")]
        public string Profession { get; set; }

        [Column("UF")]
        public string Uf { get; set; }


        [Column("birth")]
        public string Birth { get; set; }


        [Column("gender")]
        public string Gender { get; set; }

        [ForeignKey("DoctorId")]        
        public int DoctorId { get; set; }

        //public virtual UserModel Doctor { get; set; }
    }
}