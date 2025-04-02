using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pyramid.Core;

class Program
{
    static void Main(string[] args)
    {
        var departments = new List<Department>
        {
            new Department(1, "São Paulo"),
            new Department(2, "Rio de Janeiro"),
            new Department(3, "Belo Horizonte"),
            new Department(4, "Vitória"),
            new Department(5, "Curitiba")
        };

        var route = new List<Department>
        {
            departments[0], 
            departments[1],
            departments[2], 
            departments[3]  
        };
        var travel = new Travel(
            id: 1,
            maxSeatsCount: 20,
            departmentRoute: route,
            travelDate: DateTime.Now.AddDays(7),
            seats: null,
            ticket: null
        );

        Console.WriteLine("Bem-vindo ao sistema de reservas de passagens!");

        bool running = true;
        while (running)
        {
            Console.WriteLine("\nMenu:");
            Console.WriteLine("1. Ver detalhes da viagem");
            Console.WriteLine("2. Reservar passagem");
            Console.WriteLine("3. Sair");
            Console.Write("Escolha uma opção: ");

            switch (Console.ReadLine())
            {
                case "1":
                    ShowTravelDetails(travel);
                    break;
                case "2":
                    BookTicket(travel, route);
                    break;
                case "3":
                    running = false;
                    Console.WriteLine("Obrigado por usar nosso sistema!");
                    break;
                default:
                    Console.WriteLine("Opção inválida. Tente novamente.");
                    break;
            }
        }
    }

    static void ShowTravelDetails(Travel travel)
    {
        Console.WriteLine($"\nDetalhes da Viagem:");
        Console.WriteLine($"Rota: {string.Join(" -> ", travel.DepartmentRoute.Select(d => d.Name))}");
        Console.WriteLine($"Data: {travel.TravelDate:dd/MM/yyyy}");
        Console.WriteLine($"Assentos reservados: {travel.ReservedSeats()}/{travel.MaxSeatsCount}");

        if (travel.Tickets.Count > 0)
        {
            Console.WriteLine("\nPassagens reservadas:");
            foreach (var ticket in travel.Tickets)
            {
                var startDept = travel.DepartmentRoute.First(d => d.Id == ticket.StartDepartmentId);
                var endDept = travel.DepartmentRoute.First(d => d.Id == ticket.EndDepartmentId);
                Console.WriteLine($"Assento {travel.Seats.First(s => s.Id == ticket.SeatId).ArmchairNumber}: " +
                                $"{startDept.Name} -> {endDept.Name}");
            }
        }
    }

     static void BookTicket(Travel travel, List<Department> route)
    {
   
        int startDeptId, endDeptId;

        startDeptId = SelectStartDepartment(route);
        endDeptId = SelectEndDepartment(route, startDeptId);

        while (true)
        {
            List<TravelSeat> availableSeats;
        try
            {
                 availableSeats = travel.GetAllAvailableSeats(startDeptId, endDeptId);
            }
            catch (Exception e)
            {
                Console.WriteLine("Ocorreu um erro, intervalo inválido");
                continue;
            }
      

        if (availableSeats.Count == 0)
        {
            Console.WriteLine("Não há assentos disponíveis para esta rota.");
            return;
        }

        Console.WriteLine("\nAssentos disponíveis:");
        foreach (var seat in availableSeats)
        {
            Console.WriteLine($"Assento {seat.ArmchairNumber}");
        }

        
            Console.Write("\nEscolha o número do assento que deseja reservar: ");
            if (int.TryParse(Console.ReadLine(), out int seatNumber) &&
                availableSeats.Any(s => s.ArmchairNumber == seatNumber))
            {
                var selectedSeat = availableSeats.First(s => s.ArmchairNumber == seatNumber);

                var ticket = new Ticket(
                    id: travel.Tickets.Count + 1,
                    seatId: selectedSeat.Id,
                    travelId: travel.Id,
                    startDepartmentId: startDeptId,
                    endDepartmentId: endDeptId
                );

                try
                {
                    travel.ReserveSeat(ticket, selectedSeat);
                    Console.WriteLine($"Reserva confirmada! Assento {seatNumber} reservado para viagem de " +
                                      $"{route.First(d => d.Id == startDeptId).Name} para " +
                                      $"{route.First(d => d.Id == endDeptId).Name}.");
                    break;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Erro ao reservar assento: {e.Message}");
                    continue;
                }
            }
            Console.WriteLine("Assento inválido. Tente novamente.");
        }
    }

    public static int SelectStartDepartment(List<Department> route)
    {
        while (true)
        {
            Console.WriteLine("\nDepartamentos disponíveis para ORIGEM:");
            for (int i = 0; i < route.Count - 1; i++)
            {
                Console.WriteLine($"{i + 1}. {route[i].Name}");
            }

            Console.Write("Escolha o número do departamento de origem: ");
            if (int.TryParse(Console.ReadLine(), out int startChoice) && startChoice > 0 && startChoice <= route.Count)
            {
                return route[startChoice - 1].Id;

            }
            Console.WriteLine("Opção inválida. Tente novamente.");
        }
    }

    public static int SelectEndDepartment(List<Department> route, int startChoiceId)
    {
        while (true)
        {
            var index = route.FindIndex((e) =>  e.Id == startChoiceId);
            Console.WriteLine("\nDepartamentos disponíveis para DESTINO:");
            for (int i = index + 1; i < route.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {route[i].Name}");
            }

            Console.Write("Escolha o número do departamento de destino: ");
            if (int.TryParse(Console.ReadLine(), out int endChoice) && endChoice > 0 && endChoice <= route.Count)
            {
                return route[endChoice - 1].Id;

            }
            Console.WriteLine("Opção inválida. Tente novamente.");
        }
    }
}