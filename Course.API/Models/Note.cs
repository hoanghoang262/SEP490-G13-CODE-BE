﻿using System;
using System.Collections.Generic;

namespace CourseService.API.Models
{
    public partial class Note
    {
        public int Id { get; set; }
        public int LessonId { get; set; }
        public int UserId { get; set; }
        public string? ContentNote { get; set; }
        public int? VideoLink { get; set; }

        public virtual Lesson Lesson { get; set; } = null!;
    }
}
