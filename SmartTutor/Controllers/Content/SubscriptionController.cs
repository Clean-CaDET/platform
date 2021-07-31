﻿using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SmartTutor.ContentModel;
using SmartTutor.ContentModel.Exceptions;
using SmartTutor.ContentModel.Subscriptions;
using SmartTutor.Controllers.Content.DTOs;

namespace SmartTutor.Controllers.Content
{
    [Route("api/subscriptions/")]
    [ApiController]
    public class SubscriptionController : ControllerBase
    {
        private readonly ISubscriptionService _subscriptionService;
        private readonly IMapper _mapper;

        public SubscriptionController(IMapper mapper, ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
            _mapper = mapper;
        }

        [HttpPost]
        public ActionResult<string> SubscribeTeacher([FromBody] CreateSubscriptionDto dto)
        {
            try
            {
                _subscriptionService.SubscribeTeacher(dto.TeacherId,
                    dto.IndividualPlanId);
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
        }

        [HttpGet("plans")]
        public ActionResult<List<IndividualPlanDto>> GetIndividualPlans()
        {
            List<IndividualPlanDto> dtos = _subscriptionService.GetIndividualPlans();
            return Ok(dtos);
        }
    }
}