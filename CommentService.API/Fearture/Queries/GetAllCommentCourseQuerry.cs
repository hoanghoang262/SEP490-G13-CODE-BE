﻿using CommentService.API.Models;
using ForumService.API.Common.DTO;
using GrpcServices;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ForumService.API.Fearture.Queries
{
    public class GetAllCommentCourseQuerry : IRequest<List<CommentDTO>>
    {
        public int CoursesId { get; set; }
        public class GetAllCommentCourseQuerryHandler : IRequestHandler<GetAllCommentCourseQuerry, List<CommentDTO>>
        {
            private readonly GetUserInfoGrpcService _service;
            private readonly CommentContext _context;
            public GetAllCommentCourseQuerryHandler(GetUserInfoGrpcService service, CommentContext context)
            {
                _service = service;
                _context = context;
            }
            public async Task<List<CommentDTO>> Handle(GetAllCommentCourseQuerry request, CancellationToken cancellationToken)
            {

                var querry = await _context.Comments.Include(c => c.Replies).Where(c => c.CourseId != null&& c.CourseId.Equals(request.CoursesId)).ToListAsync();
                if(querry == null)
                {
                    return null;
                }
                List<CommentDTO> course = new List<CommentDTO>();
                foreach (var c in querry)
                {
                    var id = c.UserId;
                    var userInfo = await _service.SendUserId(id);

                    List<ReplyDTO> replies = new List<ReplyDTO>();
                    foreach (var reply in c.Replies)
                    {
                        var replyUserInfo = await _service.SendUserId(reply.UserId);
                        replies.Add(new ReplyDTO
                        {
                            CommentId = reply.CommentId,
                            ReplyContent = reply.ReplyContent,
                            UserId = reply.UserId,
                            Id = reply.Id,
                            UserName = replyUserInfo.Name
                        });
                    }

                    course.Add(new CommentDTO
                    {
                        UserId = c.UserId,
                        UserName = userInfo.Name,
                        CommentContent = c.CommentContent,
                        Date = c.Date,
                        Picture = userInfo.Picture,
                        Id = userInfo.Id,
                        Replies = replies
                    });
                }


                return course;
            }
        }
    }
}