using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using MeetService.Data;
using MeetService.Dtos;
using MeetService.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MeetService.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class MeetController : ControllerBase
    {
        private readonly IMeetRepo _repository;
        private readonly IMapper _mapper;
        private readonly HttpClient _HttpClient;

        public MeetController(IMapper mapper, IMeetRepo repository, HttpClient HttpClient)
        {
            _repository = repository;
            _mapper = mapper;
            _HttpClient = HttpClient;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReadMeetDTO>>> GetAllMeet()
        {
            var meetItems = _repository.GetAllMeet();

            foreach (var meetItem in meetItems)
            {
                var getUser = await _HttpClient.GetAsync("https://localhost:2001/User/" + meetItem.UserId);
                var getClient = await _HttpClient.GetAsync("https://localhost:1001/Client/" + meetItem.ClientId);

                var user = JsonConvert.DeserializeObject<User>(
                        await getUser.Content.ReadAsStringAsync());

                var client = JsonConvert.DeserializeObject<Client>(
                        await getClient.Content.ReadAsStringAsync());

                var userModel = new User();
                userModel.Id = user.Id;
                userModel.Name = user.Name;
                userModel.LastName = user.LastName;
                meetItem.User = userModel;

                var clientModel = new Client();
                clientModel.Id = client.Id;
                clientModel.Name = client.Name;
                clientModel.LastName = client.LastName;
                clientModel.Email = client.Email;
                clientModel.Phone = client.Phone;
                meetItem.Client = clientModel;
            }
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

        [HttpGet("user/{id}", Name = "GetMeetByUserId")]
        public ActionResult<IEnumerable<ReadMeetDTO>> GetMeetByUserId(int id)
        {
            var meetItem = _repository.GetMeetByUserId(id);

            if (meetItem == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<IEnumerable<ReadMeetDTO>>(meetItem));
        }

        [HttpGet("client/{id}", Name = "GetMeetByClientId")]
        public ActionResult<IEnumerable<ReadMeetDTO>> GetMeetByClientId(int id)
        {
            var meetItem = _repository.GetMeetByClientId(id);

            if (meetItem == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<IEnumerable<ReadMeetDTO>>(meetItem));
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