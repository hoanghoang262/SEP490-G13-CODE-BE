﻿using EventBus.Message.Event;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModerationService.API.Models;

namespace ModerationService.API.Fearture.Command.Moderations
{
    public class RejectCourseCommand : IRequest<IActionResult>
    {
        public int ModerationId { get; set; }

        public string ReasonWhyReject { get; set; }

        public class RejectCourseCommandHandler : IRequestHandler<RejectCourseCommand, IActionResult>
        {
            private readonly Content_ModerationContext _moderationContext;
            private readonly IPublishEndpoint _publish;

            public RejectCourseCommandHandler(Content_ModerationContext moderationContext, IPublishEndpoint publish)
            {
                _moderationContext = moderationContext;
                _publish = publish;
            }
            public async Task<IActionResult> Handle(RejectCourseCommand request, CancellationToken cancellationToken)
            {
                var moder = await _moderationContext.Moderations.FirstOrDefaultAsync(c => c.Id.Equals(request.ModerationId));
                if (moder == null)
                {
                    return new BadRequestObjectResult("Not found");

                }
                moder.Status = "Reject";
                _moderationContext.Moderations.Update(moder);
                await _moderationContext.SaveChangesAsync();

                var notificationForAdminBussiness = new NotificationEvent
                {
                    RecipientId = moder.CreatedBy,
                    IsSeen = false,
                    NotificationContent = request.ReasonWhyReject,
                    SendDate = DateTime.Now,
                    Course_Id = moder.CourseId,
                };
                await _publish.Publish(notificationForAdminBussiness);

                return new OkObjectResult(notificationForAdminBussiness);
            }
        }
    }
}