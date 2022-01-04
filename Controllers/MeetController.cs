using System;
using System.Collections.Generic;
using AutoMapper;
using MeetService.Data;
using MeetService.Dtos;
using MeetService.Models;
using Microsoft.AspNetCore.Mvc;

namespace MeetService.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class MeetController : ControllerBase
    {
        private readonly IMeetRepo _repository;
        private readonly IMapper _mapper;

        public MeetController(IMapper mapper, IMeetRepo repository)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ReadMeetDTO>> GetAllMeet()
        {
            var meetItems = _repository.GetAllMeet();

            return Ok(_mapper.Map<IEnumerable<ReadMeetDTO>>(meetItems));
        }

        [HttpGet("{id}", Name = "GetMeetById")]
        public ActionResult<ReadMeetDTO> GetMeetById(int id)
        {
            var meetItem = _repository.GetMeetById(id);

            if (meetItem == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<ReadMeetDTO>(meetItem));
        }

        [HttpPost]
        public ActionResult<CreateMeetDTO> CreateMeet(CreateMeetDTO meetDTO)
        {
            var meetModel = _mapper.Map<Meet>(meetDTO);

            _repository.CreateMeet(meetModel);
            _repository.SaveChanges();

            var readMeet = _mapper.Map<ReadMeetDTO>(meetModel);

            return CreatedAtRoute(nameof(GetMeetById), new {id = readMeet.Id }, readMeet);
        }

        [HttpPut("{id}", Name = "UpdateMeetById")]
        public ActionResult<UpdateMeetDTO> UpdateMeetById(int id, UpdateMeetDTO meetDTO)
        {
            var meetItem = _repository.GetMeetById(id);

            _mapper.Map(meetDTO, meetItem);

            if (meetItem == null)
            {
                return NotFound();
            }

            _repository.UpdateMeetById(id);
            _repository.SaveChanges();

            return CreatedAtRoute(nameof(GetMeetById), new {id = meetDTO.Id }, meetDTO);
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteMeetById(int id)
        {
            var meetItem = _repository.GetMeetById(id);

            if (meetItem == null)
            {
                return NotFound();
            }

            _repository.DeleteMeetById(meetItem.Id);
            _repository.SaveChanges();

            return NoContent();
        }
    }
}