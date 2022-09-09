﻿using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iHome.Core.Models.Database
{
    public class TUserRoom
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; } = "";
        public int RoomId { get; set; }
        [ForeignKey("RoomId")]
        public virtual TRoom Room { get; set; }
    }
}
