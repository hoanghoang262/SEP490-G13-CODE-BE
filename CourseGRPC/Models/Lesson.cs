﻿using System;
using System.Collections.Generic;

namespace CourseGRPC.Models
{
    public partial class Lesson
    {
        public Lesson()
        {
            Questions = new HashSet<Question>();
        }

        public int Id { get; set; }
        public string? Title { get; set; }
        public string? VideoUrl { get; set; }
        public int? ChapterId { get; set; }
        public string? Description { get; set; }
        public bool? IsCompleted { get; set; }
        public long? Duration { get; set; }

        public virtual Chapter? Chapter { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
    }
}