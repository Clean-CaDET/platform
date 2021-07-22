using System;
using AutoMapper;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SmartTutor.ContentModel;
using SmartTutor.ContentModel.DTOs;
using SmartTutor.ContentModel.Exceptions;

namespace SmartTutor.Controllers.Content
{
    [Route("api/subscriptions/")]
    [ApiController]
    public class SubscriptionController:ControllerBase
    {
        private readonly ISubscriptionService _subscriptionService;
        private readonly IMapper _mapper;

        public SubscriptionController(ISubscriptionService subscriptionService, IMapper mapper)
        {
            _subscriptionService = subscriptionService;
            _mapper = mapper;
        }
        
        [HttpPost]
        public ActionResult<string> SubscribeTeacher([FromBody] SubscriptionDto dto)
        {
            try
            {
                _subscriptionService.SubscribeTeacher(dto);
                return Ok("Teacher successfully subscribed!");
            }
            catch (NumberOfDaysNotSupportedException e)
            {
                return BadRequest(e.Message);
            }
            catch (TeacherAlreadySubscribedException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return Problem("Sorry, there has been an problem on server side.");
            }
        }
    }
}