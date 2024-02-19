﻿using CloudinaryDotNet.Actions;
using CourseService.API.Common.Mapping;
using CourseService.API.Common.ModelDTO;
using CourseService.API.Models;
using EventBus.Message.IntegrationEvent.PublishEvent;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ModerationService.API.Common.PublishEvent;

namespace CourseService.API.Feartures.CourseFearture.Command.CreateCourse
{
    public class SyncLessonCommand : IRequest<IActionResult>, IMapFrom<LessonEvent>
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? VideoUrl { get; set; }
        public int? ChapterId { get; set; }
        public string? Description { get; set; }
        public long? Duration { get; set; }
        public bool? IsCompleted { get; set; }
        public class asyncLessonHandler : IRequestHandler<SyncLessonCommand, IActionResult>
        {
            private readonly CourseContext _context;
            private readonly CloudinaryService _cloudinaryService;

            public asyncLessonHandler(CourseContext context, CloudinaryService cloudinaryService)
            {
                _context = context;
                _cloudinaryService = cloudinaryService;
            }
            public async Task<IActionResult> Handle(SyncLessonCommand request, CancellationToken cancellationToken)
            {
                var lesson = await _context.Lessons.FindAsync(request.Id);
                if (lesson == null)
                {
                    var newLesson = new Lesson
                    {
                        Id=request.Id,
                        Title = request.Title,
                        VideoUrl = request.VideoUrl,
                        ChapterId = request.ChapterId,
                        Description = request.Description,
                        Duration = request.Duration,
                        IsCompleted=request.IsCompleted

                    };

                    _context.Lessons.Add(newLesson);
                    await _context.SaveChangesAsync(cancellationToken);

                }
                else
                {

                    lesson.Title = request.Title;
                    lesson.VideoUrl = request.VideoUrl;
                    lesson.ChapterId = request.ChapterId;
                    lesson.Description = request.Description;
                    lesson.Duration = request.Duration;
                    lesson.IsCompleted = request.IsCompleted;

                    await _context.SaveChangesAsync(cancellationToken);
                }
              


             
                return new OkObjectResult("done") ;
            }
        }
    }
   
}