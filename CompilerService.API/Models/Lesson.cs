﻿using System;
using System.Collections.Generic;

namespace CompilerService.API.Models
{
    public partial class Lesson
    {
        public Lesson()
        {
            CompleteLessons = new HashSet<CompleteLesson>();
            Notes = new HashSet<Note>();
            TheoryQuestions = new HashSet<TheoryQuestion>();
        }

        public int Id { get; set; }
        public string? Title { get; set; }
        public string? VideoUrl { get; set; }
        public int? ChapterId { get; set; }
        public string? Description { get; set; }
        public long? Duration { get; set; }
        public string? ContentLesson { get; set; }

        public virtual Chapter? Chapter { get; set; }
        public virtual ICollection<CompleteLesson> CompleteLessons { get; set; }
        public virtual ICollection<Note> Notes { get; set; }
        public virtual ICollection<TheoryQuestion> TheoryQuestions { get; set; }
    }
}
