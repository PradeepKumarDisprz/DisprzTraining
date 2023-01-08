using DisprzTraining.DataAccess;
using DisprzTraining.Model;
using DisprzTraining.Tests.MockDatas;

namespace DisprzTraining.Tests.UnitTests.Service
{
    public class TestAppointmentServiceDAL
    {

        //Get Appointments By Date
        [Fact]
        public void GetAppointmentByDate_Returns_List_Of_Appointments()
        {
            //Arrange
            var serviceUnderTest = new AppointmentDAL();
            //Act
            var result = serviceUnderTest.GetAppointmentByDate(new DateTime(2023, 01, 06));
            //Assert
            Assert.IsType<List<Appointment>>(result);
            Assert.True(result.Count() == 2);
        }

        [Fact]
        public void GetAppointmentByDate_Returns_Empty_List()
        {
            //Arrange
            var serviceUnderTest = new AppointmentDAL();
            //Act
            var result = serviceUnderTest.GetAppointmentByDate(new DateTime(2023, 01, 02));
            //Assert
            Assert.IsType<List<Appointment>>(result);
            Assert.True(result.Count() == 0);
        }

        //Create an appointment
        [Fact]
        public void AddAppointment_Returns_True_On_Succesful_Creation()
        {
            //Arrange
            var MockAppointment = new Appointment()
            {
                appointmentId = Guid.Parse("34981518-90f1-4580-946b-f47eb379453e"),
                appointmentStartTime = DateTime.Now.AddHours(1),
                appointmentEndTime = DateTime.Now.AddHours(2),
                appointmentTitle = "worldCup Discussion",
                appointmentDescription = "will messi win the WC"
            };
            var serviceUnderTest = new AppointmentDAL();
            //Act
            var result = serviceUnderTest.AddAppointment(MockAppointment);
            //Assert
            Assert.IsType<bool>(result);
            Assert.True(result == true);
        }

        [Fact]
        public void AddAppointment_Returns_False_On_Conflict_Between_Appointments()
        {
            //Arrange
            var MockAppointment = new Appointment()
            {
                appointmentId = Guid.Parse("24981518-90f1-4580-946b-f47eb379453e"),
                appointmentStartTime = DateTime.Now.AddHours(1).AddMinutes(30),
                appointmentEndTime = DateTime.Now.AddHours(6),
                appointmentTitle = "worldCup Discussion",
                appointmentDescription = "will messi win the WC"
            };
            var serviceUnderTest = new AppointmentDAL();
            //Act
            var result = serviceUnderTest.AddAppointment(MockAppointment);
            //Assert
            Assert.IsType<bool>(result);
            Assert.True(result == false);
        }

        //Delete appointment
        [Fact]
        public void DeleteAppointment_Returns_True_On_Succesful_Deletion()
        {
            //Arrange
            var appointmentId = Guid.Parse("37981518-40f1-4580-946b-d47eb379453e");
            var serviceUnderTest = new AppointmentDAL();
            //Act
            var result = serviceUnderTest.DeleteAppointment(appointmentId);
            //Assert
            Assert.IsType<bool>(result);
            Assert.True(result == true);
        }

        [Fact]
        public void DeleteAppointment_Returns_False_On_Not_Found()
        {
            //Arrange
            var appointmentId = Guid.Parse("94981518-40f1-4580-946b-d47eb379453e");
            var serviceUnderTest = new AppointmentDAL();
            //Act
            var result = serviceUnderTest.DeleteAppointment(appointmentId);
            //Assert
            Assert.IsType<bool>(result);
            Assert.True(result == false);
        }


        //update an existing appointment
        [Fact]
        public void UpdateAppointment_Returns_True_On_Succesful_Updation()
        {
            //Arrange
            var MockAppointment = new Appointment()
            {
                appointmentId = Guid.Parse("37981518-40f1-4580-946b-d47eb379453e"),
                appointmentStartTime = DateTime.Now.AddHours(8),
                appointmentEndTime = DateTime.Now.AddHours(9),
                appointmentTitle = "worldCup Discussion",
                appointmentDescription = "will messi win the WC"
            };
            var serviceUnderTest = new AppointmentDAL();
            //Act
            var result = serviceUnderTest.AddAppointment(MockAppointment);
            //Assert
            Assert.IsType<bool>(result);
            Assert.True(result == true);
        }

        [Fact]
        public void UpdateAppointment_Returns_False_On_Conflict_Between_Appointments()
        {
            //Arrange
            var MockAppointment = new Appointment()
            {
                appointmentId = Guid.Parse("37981518-40f1-4580-946b-d47eb379453e"),
                appointmentStartTime = DateTime.Now.AddHours(1).AddMinutes(30),
                appointmentEndTime = DateTime.Now.AddHours(6),
                appointmentTitle = "worldCup Discussion",
                appointmentDescription = "will messi win the WC"
            };
            var serviceUnderTest = new AppointmentDAL();
            //Act
            var result = serviceUnderTest.AddAppointment(MockAppointment);
            //Assert
            Assert.IsType<bool>(result);
            Assert.True(result == false);
        }
    }
}