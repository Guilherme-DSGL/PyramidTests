using System;
using System.Collections;
using NuGet.Frameworks;
using Pyramid.Core;
using Reqnroll;

namespace Pyramid.Tests.StepDefinitions
{
    [Binding]
    public class ReservaDePassagemStepDefinitions
    {
        private Travel? _travel;
        private Ticket? _lastTicketAdded;
        private Exception? _exception;
        private List<Department> departments;

        private readonly ScenarioContext _scenarioContext;

        public ReservaDePassagemStepDefinitions(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            departments = new List<Department>();
        }

        [Given("a rota a ser percorrida")]
        public void GivenAViagemTemASeguinteRotaDeDepartamentos(DataTable dataTable)
        {
            departments = dataTable.Rows.Select(row =>
                  new Department(int.Parse(row["ID"]), row["Nome"])
                 ).ToList();

            if (_travel != null)
            {
                _travel = new Travel(
                    _travel.Id,
                    _travel.MaxSeatsCount,
                    departments,
                    _travel.TravelDate,
                    _travel.Seats.ToList(),
                    _travel.Tickets.ToList()
                );
            }
        }
       
        [Given("a viagem com ID {int} e {int} assentos máximos")]
        public void GivenViagemComIDEAssentosMaximos(int id, int maxSeats)
        {
            _travel = new Travel(
         id,
            maxSeats,
         departments,
         DateTime.Now,
         null,
         null
     );
        }



        [Given("o assento {int} está disponível do departamento {string} ao {string}")]
        public void GivenOAssentoEstaDisponivelDoDepartamentoAo(int armchairNumber, string startDeptName, string endDeptName)
        {

            var startDepLocation = _travel!.GetBitmapLocationFromDepartmentRoute(startDeptName);
            var endDepLocation = _travel.GetBitmapLocationFromDepartmentRoute(endDeptName);

            var bitmap = new BitArray(_travel!.MaxSeatsCount);
            var seat = new TravelSeat(armchairNumber, bitmap, _travel.Id, armchairNumber);
            _travel.AddSeat(seat);


            Assert.True(seat.IsSeatAvailableFor(startDepLocation, endDepLocation));
        }

        [When("eu reservar uma passagem para o assento {int} do departamento {string} ao {string}")]
        public void WhenEuReservarUmaPassagemParaOAssentoDoDepartamentoAo(int armchairNumber, string startDeptName, string endDeptName)
        {
            var startDeptId = _travel!.DepartmentRoute.First(d => d.Name == startDeptName).Id;
            var endDeptId = _travel!.DepartmentRoute.First(d => d.Name == endDeptName).Id;
            _lastTicketAdded = new Ticket(1, armchairNumber, _travel!.Id, startDeptId, endDeptId);
            _travel.AddTicket(_lastTicketAdded);
        }

        [Then("a passagem deverá ser adicionada com sucesso")]
        public void ThenAPassagemDeveraSerAdicionadaComSucesso()
        {
            Assert.Contains(_lastTicketAdded!, _travel!.Tickets);
        }

        [Then("o assento deverá ser marcado como ocupado do departamento {string} ao {string}")]
        public void ThenOAssentoDeveraSerMarcadoComoOcupadoDoDepartamentoAo(string startDeptName, string endDeptName)
        {

            var startDepLocation = _travel!.GetBitmapLocationFromDepartmentRoute(startDeptName);
            var endDepLocation = _travel.GetBitmapLocationFromDepartmentRoute(endDeptName);


            var seat = _travel!.Seats.First(s => s.ArmchairNumber == _lastTicketAdded!.SeatId);

            Assert.False(seat.IsSeatAvailableFor(startDepLocation, endDepLocation));
        }

        [Given("o assento {int} já está reservado do departamento {string} ao {string}")]
        public void GivenOAssentoJaEstaReservadoDoDepartamentoAo(int armchairNumber, string startDeptName, string endDeptName)
        {
            var startDept = _travel!.DepartmentRoute.First(d => d.Name == startDeptName);
            var endDept = _travel!.DepartmentRoute.First(d => d.Name == endDeptName);

            var startDeptLocation = _travel.GetBitmapLocationFromDepartmentRoute(startDeptName);
            var endDeptLocation = _travel.GetBitmapLocationFromDepartmentRoute(endDeptName);

            var bitmap = new BitArray(_travel!.MaxSeatsCount);
            var seat = new TravelSeat(armchairNumber, bitmap, _travel.Id, armchairNumber);
            _travel.AddSeat(seat);
            _travel.AddTicket(new Ticket(1, armchairNumber, _travel.Id, startDept.Id, endDept.Id));
           

            Assert.False(seat.IsSeatAvailableFor(startDeptLocation, endDeptLocation));
        }

        [When("eu tento reservar uma passagem para o assento {int} do departamento {string} ao {string}")]
        public void WhenEuTentoReservarUmaPassagemParaOAssentoDoDepartamentoAo(int armchairNumber, string startDeptName, string endDeptName)
        {
            try
            {
                var startDept = _travel!.DepartmentRoute.FirstOrDefault(d => d.Name == startDeptName)?.Id;
                var endDept = _travel!.DepartmentRoute.FirstOrDefault(d => d.Name == endDeptName)?.Id;


                TravelSeat? seat = _travel.Seats.FirstOrDefault((e) => e.ArmchairNumber == armchairNumber);

                if (seat == null)
                {
                    seat = new TravelSeat(armchairNumber, new BitArray(_travel.DepartmentRoute.Count), _travel.Id, armchairNumber);
                    _travel.AddSeat(seat);
                }
                
                var ticket = new Ticket(1, seat.Id, _travel!.Id, startDept ?? -1, endDept ?? - 1);
                _travel.AddTicket(ticket);
            }
            catch (Exception ex)
            {
                _exception = ex;
            }
        }

        [Then("sistema deve rejeitar a reserva com {string}")]
        public void ThenSistemaDeveRejeitarAReservaCom(string expectedMessage)
        {
            Assert.NotNull(_exception);
            Assert.Equal(expectedMessage, _exception.Message);

        }

        [Given("os assentos já estão reservado do departamento {string} ao {string}")]
        public void GivenOsAssentosJaEstaoReservadoDoDepartamentoAo(string startDeptName, string endDeptName)
        {
            var startDepId = _travel!.DepartmentRoute.First((e) => e.Name == startDeptName).Id;
            var endDepId = _travel!.DepartmentRoute.First((e) => e.Name == endDeptName).Id;
            for (int i = 1; i <= _travel!.MaxSeatsCount; i++)
            {
                var seat = new TravelSeat(i, new BitArray(_travel.DepartmentRoute.Count), _travel.Id, i);
                _travel.AddSeat(seat);
                var ticket = new Ticket(i, seat.Id, _travel.Id, startDepId, endDepId);

                _travel.AddTicket(ticket);
            }
        }

    }
}
