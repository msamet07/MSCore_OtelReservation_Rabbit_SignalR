using Core.Entities;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Hubs
{
    public class ReservationHub : Hub
    {
        public async Task SendMessage(Reservation reservation)
        {
            await Clients.All.SendAsync("CreatedReservation", reservation);
        }
        override public async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("UserConnected", $"{Context.ConnectionId} connected");
        }
    }
}
