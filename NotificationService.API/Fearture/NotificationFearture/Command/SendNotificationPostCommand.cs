﻿using CourseService.API.Common.Mapping;
using EventBus.Message.Event;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NotificationService.API.Models;

namespace NotificationService.API.Fearture.NotificationFearture.Command
{
    public class SendNotificationPostCommand : IRequest<IActionResult>, IMapFrom<NotificationEvent>
    {
        public int NotificationsId { get; set; }
        public int RecipientId { get; set; }
        public string? NotificationContent { get; set; }
        public int Post_Id { get; set; }
        public DateTime SendDate { get; set; }

        public bool IsSeen { get; set; }
        public int Course_Id { get; set; }

        public class SendNotificationPostCommandHandle : IRequestHandler<SendNotificationCommand, IActionResult>
        {
            private readonly NotificationContext _context;

            public SendNotificationPostCommandHandle(NotificationContext context)
            {
                _context = context;
            }
            public async Task<IActionResult> Handle(SendNotificationCommand request, CancellationToken cancellationToken)
            {
                var notification = new Notification
                {
                    IsSeen=false,
                    NotificationContent = request.NotificationContent,
                   RecipientId=request.RecipientId,
                   SendDate=DateTime.UtcNow,
                   PostId=request.Post_Id,
                };
                 _context.Notifications.Add(notification);    
                await _context.SaveChangesAsync();
                return new OkObjectResult(notification);
            }
        }
    }
}