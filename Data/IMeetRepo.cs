using System.Collections.Generic;
using MeetService.Models;

namespace MeetService.Data
{
    public interface IMeetRepo
    {
        bool SaveChanges();

        void CreateMeet(Meet Meet);

        IEnumerable<Meet> GetAllMeet();

        IEnumerable<Client> GetAllClient();

        IEnumerable<User> GetAllUser();

        Meet GetMeetById(int id);

        IEnumerable<Meet> GetMeetByUserId(int id);

        IEnumerable<Meet> GetMeetByClientId(int id);

        User GetUserById(int id);

        Client GetClientById(int id);

        void UpdateMeetById(int id);

        //méthode update client rabbitMQ
        void UpdateClientById(int id);

        void DeleteMeetById(int id);

        void DeleteClientById(int id);
    }
}