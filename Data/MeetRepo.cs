using System;
using System.Collections.Generic;
using System.Linq;
using MeetService.Models;
using Microsoft.EntityFrameworkCore;

namespace MeetService.Data
{
    public class MeetRepo : IMeetRepo
    {

        private readonly AppDbContext _context;

        public MeetRepo(AppDbContext context)
        {
            // On initialise le context 
            _context = context;
        }

        public void CreateMeet(Meet Meet)
        {
            if (Meet == null)
            {
                throw new ArgumentNullException(nameof(Meet));
            }

            _context.Add(Meet);
            _context.SaveChanges();
        }

        public void DeleteMeetById(int id)
        {
            var Meet = _context.Meet.FirstOrDefault(Meet => Meet.Id == id);

            if (Meet != null)
            {
                _context.Meet.Remove(Meet);
            }
        }

        public IEnumerable<Meet> GetAllMeet()
        {
            _context.User.ToList();
            _context.Client.ToList();
            return _context.Meet.ToList();
        }

        public IEnumerable<User> GetAllUser()
        {
            return _context.User.ToList();
        }

        public IEnumerable<Client> GetAllClient()
        {
            return _context.Client.ToList();
        }

        public User GetUserById(int id)
        {
            return _context.User.FirstOrDefault(user => user.Id == id);
        }

        public Client GetClientById(int id)
        {
            return _context.Client.FirstOrDefault(client => client.Id == id);
        }

        public IEnumerable<Meet> GetMeetByUserId(int id)
        {
            _context.User.ToList();
            _context.Client.ToList();
            return _context.Meet.Where(m => m.UserId == id).ToList();
        }

        public IEnumerable<Meet> GetMeetByClientId(int id)
        {
            _context.User.ToList();
            _context.Client.ToList();
            return _context.Meet.Where(m => m.ClientId == id).ToList();
        }

        public Meet GetMeetById(int id)
        {
            _context.User.ToList();
            _context.Client.ToList();
            return _context.Meet.FirstOrDefault(Meet => Meet.Id == id);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >=0 );
        }

        public void UpdateMeetById(int id)
        {
            var Meet = _context.Meet.FirstOrDefault(Meet => Meet.Id == id);

            _context.Entry(Meet).State = EntityState.Modified;
        }
    }
}