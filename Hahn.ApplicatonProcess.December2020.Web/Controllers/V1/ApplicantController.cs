﻿using AutoMapper;
using Hahn.ApplicatonProcess.December2020.Domain.Contracts.V1.Requests;
using Hahn.ApplicatonProcess.December2020.Domain.Entities;
using Hahn.ApplicatonProcess.December2020.Domain.Models;
using Hahn.ApplicatonProcess.December2020.Domain.Services;
using Hahn.ApplicatonProcess.December2020.Domain.Services.ApplicantService;
using Hahn.ApplicatonProcess.December2020.Web.Helpers.Swagger;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hahn.ApplicatonProcess.December2020.Web.Controllers.V1
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ApplicantController : ControllerBase
    {
        private readonly IApplicantService _applicantService;
        private IMapper _mapper;
        private IUnitOfWork _unitOfWork;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="applicantService"></param>
        /// <param name="mapper"></param>
        /// <param name="unitOfWork"></param>
        public ApplicantController(IApplicantService applicantService, IMapper mapper,
           IUnitOfWork unitOfWork)
        {
            _applicantService = applicantService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Post applicant information
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerRequestExample(typeof(ApplicantRequest), typeof(ApplicantRequestExample))]
        public async Task<IActionResult> Post([FromBody] ApplicantRequest model)
        {

            var applicant = _mapper.Map<Applicant>(model);

            _unitOfWork.Applicant.Add(applicant);
            await _unitOfWork.Complete();

            var request = HttpContext.Request;

            string url = $"{request.Scheme }// { request.Host.ToUriComponent()}";


            return Created(url, new { id = applicant.ID });
        }

        /// <summary>
        /// Get applicant information
        /// </summary>
        /// <returns></returns>
        [HttpGet("{applicantId}")]
        public async Task<IActionResult> Get(int applicantId)
        {

            var response = await _unitOfWork.Applicant.Get(applicantId);

            return Ok(response);
        }
        /// <summary>
        /// Update applicant information
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] ApplicantUpdateRequest model)
        {
            var applicant = _mapper.Map<Applicant>(model);

            _unitOfWork.Applicant.Update(applicant);
            var response = await _unitOfWork.Complete();

            return Ok(response);
        }

        /// <summary>
        /// Delete applicant information
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var applicant = await _unitOfWork.Applicant.Get(id);
            _unitOfWork.Applicant.Remove(applicant);
            var response = await _unitOfWork.Complete();

            return Ok(response);
        }
    }
}
