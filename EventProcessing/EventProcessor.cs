using System;
using System.Text.Json;
using MeetService.Data;
using MeetService.Dtos;
using MeetService.Models;
using Microsoft.Extensions.DependencyInjection;

namespace MeetService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public EventProcessor(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
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
                    //Mise à jour du client en setant un new client
                    // var client = new Client {
                    //     Id = clientUpdatedDto.Id,
                    //     Name = clientUpdatedDto.Name,
                    //     LastName = clientUpdatedDto.LastName,
                    //     Address = clientUpdatedDto.Address,
                    //     Phone = clientUpdatedDto.Phone,
                    //     Email = clientUpdatedDto.Email,
                    //     Company = clientUpdatedDto.Company,
                    //     MeetId = clientUpdatedDto.MeetId
                        
                    // };

                    // Console.WriteLine(client.Name);

                    var clientRepo = repo.GetClientById(clientUpdatedDto.Id);
                    // SI le client existe bien on l'update sinon rien
                    if(clientRepo != null)
                    {
                        repo.UpdateClientById(clientUpdatedDto);
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
    }

    //Type d'event
    enum EventType
    {
        ClientUpdated,
        Undetermined
    }
}