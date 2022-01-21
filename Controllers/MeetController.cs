using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using MeetService.Data;
using MeetService.Dtos;
using MeetService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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

        private readonly IConfiguration _configuration;

        public MeetController(IMapper mapper, IMeetRepo repository, HttpClient HttpClient, IConfiguration configuration)
        {
            _repository = repository;
            _mapper = mapper;
            _HttpClient = HttpClient;
            _configuration = configuration;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ReadMeetDTO>> GetAllMeet()
        {
            var meetItems = _repository.GetAllMeet();

            return Ok(_mapper.Map<IEnumerable<ReadMeetDTO>>(meetItems));
        }

        [HttpGet("users", Name = "GetAllUser")]
        public ActionResult<IEnumerable<UserReadDto>> GetAllUser()
        {
            var userItems = _repository.GetAllUser();

            return Ok(_mapper.Map<IEnumerable<UserReadDto>>(userItems));
        }

        [HttpGet("clients", Name = "GetAllClient")]
        public ActionResult<IEnumerable<ReadClientDTO>> GetAllClient()
        {
            var clientItems = _repository.GetAllClient();

            return Ok(_mapper.Map<IEnumerable<ReadClientDTO>>(clientItems));
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
        public async Task<ActionResult<CreateMeetDTO>> CreateMeet(CreateMeetDTO meetDTO)
        {
            var meetModel = _mapper.Map<Meet>(meetDTO);

            //var getUser = await _HttpClient.GetAsync("https://localhost:2001/User/" + meetModel.UserId);
            var getClient = await _HttpClient.GetAsync($"{_configuration["ClientService"]}" + meetModel.ClientId);

            //var deserializeUser = JsonConvert.DeserializeObject<UserCreateDto>(
                    //await getUser.Content.ReadAsStringAsync());

            var deserializeClient = JsonConvert.DeserializeObject<CreateClientDTO>(
                    await getClient.Content.ReadAsStringAsync());

            //var UserDTO = _mapper.Map<User>(deserializeUser);
            var ClientDTO = _mapper.Map<Client>(deserializeClient);

            var client = _repository.GetClientById(ClientDTO.Id);
            //var user = _repository.GetUserById(UserDTO.Id);

            if (client == null) meetModel.Client = ClientDTO; else meetModel.Client = client;

            //if (user == null) meetModel.User = UserDTO; else meetModel.User = user;

            _repository.CreateMeet(meetModel);
            _repository.SaveChanges();

            var readMeet = _mapper.Map<ReadMeetDTO>(meetModel);

            return CreatedAtRoute(nameof(GetMeetById), new { id = readMeet.Id }, readMeet);
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

        [HttpPut("client/{id}", Name = "UpdateClientById")]
        public ActionResult<UpdateClientDTO> UpdateClientById(int id, UpdateClientDTO clientDTO)
        {
            var meetItem = _repository.GetClientById(id);

            _mapper.Map(clientDTO, meetItem);

            if (meetItem == null)
            {
                return NotFound();
            }

            _repository.UpdateClientById(id);
            _repository.SaveChanges();

            return CreatedAtRoute(nameof(GetMeetById), new {id = clientDTO.Id }, clientDTO);
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