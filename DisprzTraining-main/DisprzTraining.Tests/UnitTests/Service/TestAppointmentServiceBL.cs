using DisprzTraining.Business;
using DisprzTraining.DataAccess;
using DisprzTraining.Model;
using DisprzTraining.Tests.MockDatas;
using Moq;

namespace DisprzTraining.Tests.UnitTests.Service
{
    public class TestAppointmentServiceBL
    {
        private readonly Mock<IAppointmentDAL> MockServiceDAL = new();
        private readonly Guid MockGuid = Guid.NewGuid();


        //Get Appointment by Date
        [Fact]
        public void GetAppointmentByDate_Returns_List_Of_Appointments()
        {
            //Arrange
            var expectedResult = MockData.MockAppointmentList;
            MockServiceDAL.Setup(service => service.GetAppointmentByDate(It.IsAny<DateTime>())).Returns(expectedResult);
            var serviceUnderTest = new AppointmentBL(MockServiceDAL.Object);
            //Act
            var result = serviceUnderTest.GetAppointmentByDate(DateTime.Now.Date);
            //Assert
            Assert.Equal(expectedResult.Count(), result.Count());
            Assert.IsType<List<Appointment>>(result);
        }

        [Fact]
        public void GetAppointmentByDate_Returns_Empty_List()
        {
            //Arrange
            var expectedResult = new List<Appointment>();
            MockServiceDAL.Setup(service => service.GetAppointmentByDate(It.IsAny<DateTime>())).Returns(expectedResult);
            var serviceUnderTest = new AppointmentBL(MockServiceDAL.Object);
            //Act
            var result = serviceUnderTest.GetAppointmentByDate(DateTime.Now.Date);
            //Assert
            Assert.Equal(expectedResult.Count(), result.Count());
            Assert.IsType<List<Appointment>>(result);
        }

        //Create an appointment
        [Fact]
        public void AddAppointment_Returns_NewId_On_Succesful_Creation()
        {
            //Arrange
            MockServiceDAL.Setup(service => service.AddAppointment(It.IsAny<Appointment>())).Returns(true);
            var serviceUnderTest = new AppointmentBL(MockServiceDAL.Object);
            //Act
            var result = serviceUnderTest.AddAppointment(MockData.MockAppointmentDTO);
            //Assert
            Assert.IsType<NewAppointmentId>(result);
        }

        [Fact]
        public void AddAppointment_Returns_Null_On_Conflict_Between_Appointments()
        {
            //Arrange
            MockServiceDAL.Setup(service => service.AddAppointment(It.IsAny<Appointment>())).Returns(false);
            var serviceUnderTest = new AppointmentBL(MockServiceDAL.Object);
            //Act
            var result = serviceUnderTest.AddAppointment(MockData.MockAppointmentDTO);
            //Assert
            Assert.True(result == null);
        }


        //Delete appointment
        [Fact]
        public void DeleteAppointment_Returns_True_On_Succesful_Deletion()
        {
            //Arrange
            var expectedResult = true;
            MockServiceDAL.Setup(service => service.DeleteAppointment(It.IsAny<Guid>())).Returns(expectedResult);
            var serviceUnderTest = new AppointmentBL(MockServiceDAL.Object);
            //Act
            var result = serviceUnderTest.DeleteAppointment(MockGuid);
            //Assert
            Assert.Equal(expectedResult, result);
            Assert.IsType<bool>(result);
        }

        [Fact]
        public void DeleteAppointment_Returns_False_On_Not_Found()
        {
            //Arrange
            var expectedResult = false;
            MockServiceDAL.Setup(service => service.DeleteAppointment(It.IsAny<Guid>())).Returns(expectedResult);
            var serviceUnderTest = new AppointmentBL(MockServiceDAL.Object);
            //Act
            var result = serviceUnderTest.DeleteAppointment(MockGuid);
            //Assert
            Assert.Equal(expectedResult, result);
            Assert.IsType<bool>(result);
        }

        //Update an appointment
        [Fact]
        public void UpdateAppointment_Returns_True_On_Succesful_Updation()
        {
            //Arrange
            var expectedResult = true;
            MockServiceDAL.Setup(service => service.UpdateAppointment(It.IsAny<Appointment>())).Returns(expectedResult);
            var serviceUnderTest = new AppointmentBL(MockServiceDAL.Object);
            //Act
            var result = serviceUnderTest.UpdateAppointment(MockGuid, MockData.MockAppointmentDTO);
            //Assert
            // Assert.Equal();
            Assert.Equal(expectedResult, result);
        }


        [Fact]
        public void UpdateAppointment_Returns_False_On_Conflict_Between_Appointments()
        {
            //Arrange
            var expectedResult = false;
            MockServiceDAL.Setup(service => service.UpdateAppointment(It.IsAny<Appointment>())).Returns(false);
            var serviceUnderTest = new AppointmentBL(MockServiceDAL.Object);
            //Act
            var result = serviceUnderTest.UpdateAppointment(MockGuid, MockData.MockAppointmentDTO);
            //Assert
            // Assert.Equal();
            Assert.Equal(expectedResult, result);
        }
    }
}