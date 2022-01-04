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
            return _context.Meet.ToList();
        }

        public Meet GetMeetById(int id)
        {
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