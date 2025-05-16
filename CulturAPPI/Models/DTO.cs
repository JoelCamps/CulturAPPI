using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CulturAPPI.Models
{
    public class RoomDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class TypeEventDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class EventDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Start_datetime { get; set; }
        public string End_datetime { get; set; }
        public int Capacity { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public bool? Active { get; set; }
        public int? Room_id { get; set; }
        public int? Type_id { get; set; }
        public RoomDTO Rooms { get; set; }
        public TypeEventDTO Type_event { get; set; }
    }

    public class BookingDTO
    {
        public int Event_id { get; set; }
        public int User_id { get; set; }
        public bool? Active { get; set; }
        public int Quantity { get; set; }
        public EventDTO Events { get; set; }
    }

    public class UserDTO
    {
        public int id { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string type { get; set; }
        public bool? active { get; set; }
    }
}