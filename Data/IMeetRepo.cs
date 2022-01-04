using System.Collections.Generic;
using MeetService.Models;

namespace MeetService.Data
{
    public interface IMeetRepo
    {
        bool SaveChanges();

        void CreateMeet(Meet Meet);

        IEnumerable<Meet> GetAllMeet();

        Meet GetMeetById(int id);

        void UpdateMeetById(int id);

        void DeleteMeetById(int id);
    }
}