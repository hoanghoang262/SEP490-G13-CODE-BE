﻿using CloudinaryDotNet;
using Contract.Service.Message;
using CourseService.API.Feartures.CourseFearture.Queries.CourseQueries;
using CourseService.API.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CourseService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly Cloudinary _cloudinary;
        private readonly CourseContext context;

        public CourseController(IMediator mediator, Cloudinary cloudinary, CourseContext _context)
        {
            _mediator = mediator;
            _cloudinary = cloudinary;
            context = _context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCourses([FromQuery] GetAllCourseQuerry query)
        {
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetCourseByUser(int Id)
        {
            return Ok(await _mediator.Send(new GetCourseByUserIdQuerry { UserId = Id }));
        }

        [HttpGet]
        public async Task<IActionResult> GetCourseByCourseId(int Id)
        {
            return Ok(await _mediator.Send(new GetCourseByCourseIdQuerry { CourseId = Id }));
        }

        [HttpGet]
        public async Task<IActionResult> GetChapterById(int chapterId)
        {
            try
            {
                var query = new GetChapterByIdQuerry { ChapterId = chapterId };
                var result = await _mediator.Send(query);

                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest(Message.MSG30);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetLessonById(int lessonId)
        {
            try
            {
                var query = new GetLessonByIdQuerry { LessonId = lessonId };
                var result = await _mediator.Send(query);

                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest(Message.MSG30);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetPracticeQuestionById(int practiceQuestionId)
        {
            try
            {
                var query = new GetPracticeQuestionByIdQuerry { PracticeQuestionId = practiceQuestionId };
                var result = await _mediator.Send(query);

                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest(Message.MSG30);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CompletedLesson(int userId, int lessonId)
        {
            var completed = new CompleteLesson
            {
                LessonId = lessonId,
                UserId = userId
            };

            context.CompleteLessons.Add(completed);
            await context.SaveChangesAsync();
            return Ok(completed);
        }
    }
}
