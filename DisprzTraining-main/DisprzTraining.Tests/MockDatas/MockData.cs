using DisprzTraining.Model;

namespace DisprzTraining.Tests.MockDatas
{
    public static class MockData
    {

        public static AppointmentDTO MockAppointment = new AppointmentDTO()
        {
            appointmentStartTime = DateTime.Now.AddHours(1),
            appointmentEndTime = DateTime.Now.AddHours(2),
            appointmentTitle = "worldCup Discussion",
            appointmentDescription = "will messi win the WC"
        };

        //appointments matched with the searchDate
        public static List<Appointment> MockAppointmentWithDate = new List<Appointment>()
        {
         new Appointment{
            appointmentId=Guid.Parse("37981518-40f1-4580-946b-d47eb379453e"),
            appointmentStartTime = new DateTime(2023, 01, 06, 13, 30, 00),
            appointmentEndTime = new DateTime(2023, 01, 06, 14, 30, 00),
            appointmentTitle = "worldCup Discussion",
            appointmentDescription = "will messi win the WC"
        },
         new Appointment{
            appointmentId=Guid.Parse("27921518-40f1-4580-946b-d47eb379453e"),
            appointmentStartTime = new DateTime(2023, 01, 06, 12, 30, 00),
            appointmentEndTime = new DateTime(2023, 01, 06, 13, 30, 00),
            appointmentTitle = "worldCup Discussion",
            appointmentDescription = "will messi win the WC"
        },
        new Appointment{
            appointmentId=Guid.Parse("17981518-40f1-4580-946b-d47eb379453e"),
            appointmentStartTime = new DateTime(2023, 01, 06, 14, 30, 00),
            appointmentEndTime = new DateTime(2023, 01, 06, 15, 30, 00),
            appointmentTitle = "worldCup Discussion",
            appointmentDescription = "will messi win the WC"
        },
         new Appointment{
            appointmentId=Guid.Parse("47981518-40f1-4580-946b-d47eb379453e"),
            appointmentStartTime = new DateTime(2023, 01, 06, 15, 30, 00),
            appointmentEndTime = new DateTime(2023, 01, 06, 16, 30, 00),
            appointmentTitle = "StandUp",
            appointmentDescription = "Discussion on CalendarUI"
        }
        };

        //appointments matched with the searchTitle
        public static List<Appointment> MockAppointmentWithTitle = new List<Appointment>()
        {
         new Appointment{
            appointmentId=Guid.Parse("37981518-40f1-4580-946b-d47eb379453e"),
            appointmentStartTime = new DateTime(2023, 01, 06, 13, 30, 00),
            appointmentEndTime = new DateTime(2023, 01, 06, 14, 30, 00),
            appointmentTitle = "worldCup Discussion",
            appointmentDescription = "will messi win the WC"
        },
         new Appointment{
            appointmentId=Guid.Parse("27921518-40f1-4580-946b-d47eb379453e"),
            appointmentStartTime = new DateTime(2023, 01, 06, 12, 30, 00),
            appointmentEndTime = new DateTime(2023, 01, 06, 13, 30, 00),
            appointmentTitle = "worldCup Discussion",
            appointmentDescription = "will messi win the WC"
        },
        new Appointment{
            appointmentId=Guid.Parse("17981518-40f1-4580-946b-d47eb379453e"),
            appointmentStartTime = new DateTime(2023, 01, 07, 14, 30, 00),
            appointmentEndTime = new DateTime(2023, 01, 07, 15, 30, 00),
            appointmentTitle = "worldCup Discussion",
            appointmentDescription = "will messi win the WC"
        }
        };

        //appointments matched with the searchDateAndTitle
        public static List<Appointment> MockAppointmentWithDateAndTitle = new List<Appointment>()
        {
         new Appointment{
            appointmentId=Guid.Parse("37981518-40f1-4580-946b-d47eb379453e"),
            appointmentStartTime = new DateTime(2023, 01, 06, 13, 30, 00),
            appointmentEndTime = new DateTime(2023, 01, 06, 14, 30, 00),
            appointmentTitle = "worldCup Discussion",
            appointmentDescription = "will messi win the WC"
        },
         new Appointment{
            appointmentId=Guid.Parse("27921518-40f1-4580-946b-d47eb379453e"),
            appointmentStartTime = new DateTime(2023, 01, 06, 12, 30, 00),
            appointmentEndTime = new DateTime(2023, 01, 06, 13, 30, 00),
            appointmentTitle = "worldCup Discussion",
            appointmentDescription = "will messi win the WC"
        }
        };

        //TotalAppointments
        public static List<Appointment> MockAppointmentList = new List<Appointment>()
        {
         new Appointment{
            appointmentId=Guid.Parse("37981518-40f1-4580-946b-d47eb379453e"),
                appointmentStartTime = new DateTime(2023, 01, 06, 13, 30, 00),
                appointmentEndTime = new DateTime(2023, 01, 06, 14, 30, 00),
            appointmentTitle = "worldCup Discussion",
            appointmentDescription = "will messi win the WC"
        },
         new Appointment{
            appointmentId=Guid.Parse("27921518-40f1-4580-946b-d47eb379453e"),
            appointmentStartTime = new DateTime(2023, 01, 06, 13, 30, 00),
            appointmentEndTime = new DateTime(2023, 01, 06, 14, 30, 00),
            appointmentTitle = "worldCup Discussion",
            appointmentDescription = "will messi win the WC"
        },
                 new Appointment{
            appointmentId=Guid.Parse("17981518-40f1-4580-946b-d47eb379453e"),
            appointmentStartTime = DateTime.Now,
            appointmentEndTime = DateTime.Now.AddHours(1),
            appointmentTitle = "worldCup Discussion",
            appointmentDescription = "will messi win the WC"
        },
         new Appointment{
            appointmentId=Guid.Parse("47981518-40f1-4580-946b-d47eb379453e"),
            appointmentStartTime = DateTime.Now,
            appointmentEndTime = DateTime.Now.AddHours(1),
            appointmentTitle = "worldCup Discussion",
            appointmentDescription = "will messi win the WC"
        },
                 new Appointment{
            appointmentId=Guid.Parse("57981518-40f1-4580-946b-d47eb379453e"),
            appointmentStartTime = DateTime.Now,
            appointmentEndTime = DateTime.Now.AddHours(1),
            appointmentTitle = "worldCup Discussion",
            appointmentDescription = "will messi win the WC"
        },
         new Appointment{
            appointmentId=Guid.Parse("67981518-40f1-4580-946b-d47eb379453e"),
            appointmentStartTime = DateTime.Now,
            appointmentEndTime = DateTime.Now.AddHours(1),
            appointmentTitle = "worldCup Discussion",
            appointmentDescription = "will messi win the WC"
        },
                 new Appointment{
            appointmentId=Guid.Parse("77981518-40f1-4580-946b-d47eb379453e"),
            appointmentStartTime = new DateTime(2023, 01, 06, 13, 30, 00),
            appointmentEndTime = new DateTime(2023, 01, 06, 14, 30, 00),
            appointmentTitle = "worldCup Discussion",
            appointmentDescription = "will messi win the WC"
        },
         new Appointment{
            appointmentId=Guid.Parse("87981518-40f1-4580-946b-d47eb379453e"),
            appointmentStartTime = new DateTime(2023, 01, 06, 13, 30, 00),
            appointmentEndTime = new DateTime(2023, 01, 06, 14, 30, 00),
            appointmentTitle = "worldCup Discussion",
            appointmentDescription = "will messi win the WC"
        },
                 new Appointment{
            appointmentId=Guid.Parse("97981518-40f1-4580-946b-d47eb379453e"),
            appointmentStartTime = DateTime.Now,
            appointmentEndTime = DateTime.Now.AddHours(1),
            appointmentTitle = "worldCup Discussion",
            appointmentDescription = "will messi win the WC"
        },
         new Appointment{
            appointmentId=Guid.Parse("10981518-40f1-4580-946b-d47eb379453e"),
            appointmentStartTime = DateTime.Now,
            appointmentEndTime = DateTime.Now.AddHours(1),
            appointmentTitle = "worldCup Discussion",
            appointmentDescription = "will messi win the WC"
        }
        };
    }
}