using System;
using System.Text.Json;
using AutoMapper;
using MeetService.Data;
using MeetService.Dtos;
using Microsoft.Extensions.DependencyInjection;

namespace MeetService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _scopeFactory;

        private readonly IMapper _mapper;

        public EventProcessor(IServiceScopeFactory scopeFactory, IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }

        public void ProcessEvent(string message)
        {
            var eventType = DetermineEvent(message);

            switch(eventType)
            {
                // On souscrit à la méthode UpdateClient() si la valeur retournée est bien EventType
                case EventType.ClientUpdated:
                    UpdateClient(message);
                    break;
                case EventType.UserUpdated:
                    UpdateUser(message);
                    break;
                default:
                    break;
            }
        }

        private EventType DetermineEvent(string notificationMessage)
        {
            Console.WriteLine("--> Determining Event");

            // On déserialise les données pour retourner un objet (texte vers objet json)
            var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);
            Console.WriteLine($"--> Event Type: {eventType.Event}");

            switch(eventType.Event)
            {
                /* "Client_Updated" est la valeur attribuée dans le controller de ClientService
                lors de l'envoi de données 
                Dans le cas ou la valeur de notre attribue Event est bien "Client_Updated",
                nous retournons notre objet */
                case "Client_Updated":
                    Console.WriteLine("--> Platform Updated Event Detected");
                    return EventType.ClientUpdated;
                //Dans le cas d'un user
                case "User_Updated":
                    Console.WriteLine("--> Platform Updated Event Detected");
                    return EventType.UserUpdated;
                // Sinon nous retournons que l'objet est indeterminé
                default:
                    Console.WriteLine("-> Could not determine the event type");
                    return EventType.Undetermined;
            }
        }

        private void UpdateClient(string clientUpdatedMessage)
        {
            using(var scope = _scopeFactory.CreateScope())
            {
                // Recuperation du scope de meetRepo
                var repo = scope.ServiceProvider.GetRequiredService<IMeetRepo>();

                //On deserialize le clientUpdatedMessage
                var clientUpdatedDto = JsonSerializer.Deserialize<ClientUpdatedDto>(clientUpdatedMessage);
                Console.WriteLine($"--> Client Updated: {clientUpdatedDto}");

                try
                {

                    //Console.WriteLine(clientUpdatedDto.Name);

                    var clientRepo = repo.GetClientById(clientUpdatedDto.Id);
                    
                    _mapper.Map(clientUpdatedDto, clientRepo);
                    
                    // SI le client existe bien on l'update sinon rien
                    if(clientRepo != null)
                    {
                        //Console.WriteLine(clientRepo.Name);
                        repo.SaveChanges();
                        Console.WriteLine("--> Client mis à jour");
                    }
                    else{
                        Console.WriteLine("--> Client non existant");
                    }
                }
                // Si une erreur survient, on affiche un message
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not update Client to DB {ex.Message}");
                }
            }
        }

        private void UpdateUser(string userUpdatedMessage)
        {
            using(var scope = _scopeFactory.CreateScope())
            {
                // Recuperation du scope de meetRepo
                var repo = scope.ServiceProvider.GetRequiredService<IMeetRepo>();

                //On deserialize le userUpdatedMessage
                var userUpdatedDto = JsonSerializer.Deserialize<UserUpdatedDto>(userUpdatedMessage);
                Console.WriteLine($"--> User Updated: {userUpdatedDto}");

                try
                {
                    var userRepo = repo.GetUserById(userUpdatedDto.Id);
                    Console.WriteLine(userRepo.Name);
                    _mapper.Map(userUpdatedDto, userRepo);
                    
                    // SI le user existe bien on l'update sinon rien
                    if(userRepo != null)
                    {

                        repo.UpdateUserById(userRepo.Id);                     
                        repo.SaveChanges();
                        Console.WriteLine("--> User mis à jour");
                    }
                    else{
                        Console.WriteLine("--> User non existant");
                    }
                }
                // Si une erreur survient, on affiche un message
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not update User to DB {ex.Message}");
                }
            }
        }
    }

    //Type d'event
    enum EventType
    {
        ClientUpdated,
        UserUpdated,
        Undetermined
    }
}