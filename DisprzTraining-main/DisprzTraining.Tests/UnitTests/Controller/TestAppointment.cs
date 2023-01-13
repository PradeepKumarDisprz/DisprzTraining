using DisprzTraining.Business;
using DisprzTraining.Controllers;
using DisprzTraining.Model;
using DisprzTraining.Tests.MockDatas;
using Microsoft.AspNetCore.Mvc;
using Moq;


namespace DisprzTraining.Tests.UnitTests.Controller
{
    public class TestAppointment
    {

        private readonly Mock<IAppointmentBL> MockServiceBL = new();
        private readonly Guid MockGuid = Guid.NewGuid();

        // // Get Appointment by Date//
        [Fact]
        public void GetAppointmentByDate_Returns_200_Success_And_List_Of_Appointments()
        {
            //Arrange
            var expectedResult =MockData.MockAppointmentList;
            MockServiceBL.Setup(service => service.GetAppointmentByDate(It.IsAny<DateTime>())).Returns(expectedResult);
            var systemUnderTest = new AppointmentsController(MockServiceBL.Object);

            //Act
            var result = systemUnderTest.GetAppointmentByDate(DateTime.Now.Date);
            var ObjectResult = (OkObjectResult)result;

            //Assert
            Assert.IsType<List<Appointment>>(ObjectResult.Value);
            Assert.True(ObjectResult.StatusCode.Equals(200));
            Assert.True(ObjectResult.Value.Equals(expectedResult));
        }

        [Fact]
        public void GetAppointmentByDate_Returns_200_Success_And_Empty_List()
        {
            //Arrange
            var expectedResult = new List<Appointment>();
            MockServiceBL.Setup(service => service.GetAppointmentByDate(It.IsAny<DateTime>())).Returns(expectedResult);
            var systemUnderTest = new AppointmentsController(MockServiceBL.Object);

            //Act
            var result = systemUnderTest.GetAppointmentByDate(DateTime.Now.Date);
            var ObjectResult = (OkObjectResult)result;

            //Assert
            Assert.IsType<List<Appointment>>(ObjectResult.Value);
            Assert.True(ObjectResult.StatusCode.Equals(200));
            Assert.True(ObjectResult.Value.Equals(expectedResult));
        }



        // //Create new appointment //
        [Fact]
        public void AddNewAppointment_Returns_201_Created_And_New_Appointment_Id_On_Successful_Creation()
        {
            //Arrange
            var expectedResult = new NewAppointmentId()
            {
                Id = MockGuid
            };
            MockServiceBL.Setup(service => service.AddAppointment(It.IsAny<AppointmentDTO>())).Returns(expectedResult);
            var systemUnderTest = new AppointmentsController(MockServiceBL.Object);

            //Act
            var result = systemUnderTest.AddAppointment(MockData.MockAppointmentDTO);
            var ObjectResult = (CreatedResult)result;

            //Assert
            Assert.True(ObjectResult.StatusCode.Equals(201));
            Assert.True(ObjectResult?.Value?.Equals(expectedResult));
        }

        [Fact]
        public void AddNewAppointment_Returns_409_Conflict_Between_Different_Meetings()
        {
            //Arrange
            MockServiceBL.Setup(service => service.AddAppointment(It.IsAny<AppointmentDTO>())).Returns(() => null);
            var systemUnderTest = new AppointmentsController(MockServiceBL.Object);

            //Act
            var result = systemUnderTest.AddAppointment(MockData.MockAppointmentDTO);
            var ObjectResult = (ConflictObjectResult)result;

            //Assert
            Assert.True(ObjectResult.StatusCode.Equals(409));
        }

        [Fact]
        public void AddNewAppointment_Returns_400_BadRequest_On_EndTime_LessThan_StartTime()
        {
            //Arrange  
            var MockAppointment = new AppointmentDTO()
            {
                appointmentStartTime = DateTime.Now.AddHours(4),
                appointmentEndTime = DateTime.Now.AddHours(3),
                appointmentTitle = "worldCup Discussion",
                appointmentDescription = "will messi win the WC"
            };
            var systemUnderTest = new AppointmentsController(MockServiceBL.Object);

            //Act
            var result = systemUnderTest.AddAppointment(MockAppointment) as BadRequestObjectResult;

            //Assert
            Assert.True(result?.StatusCode.Equals(400));
        }

        [Fact]
        public void AddNewAppointment_Returns_400_BadRequest_On_PastTime()
        {
            //Arrange  

            var MockAppointment = new AppointmentDTO()
            {
                appointmentStartTime = DateTime.Now.AddHours(-2),
                appointmentEndTime = DateTime.Now.AddHours(3),
                appointmentTitle = "worldCup Discussion",
                appointmentDescription = "will messi win the WC"
            };
            var systemUnderTest = new AppointmentsController(MockServiceBL.Object);

            //Act
            var result = systemUnderTest.AddAppointment(MockAppointment) as BadRequestObjectResult;

            //Assert
            Assert.True(result?.StatusCode.Equals(400));
        }
        [Fact]
        public void AddNewAppointment_Returns_400_BadRequest_On_PastDate()
        {
            //Arrange  

            var MockAppointment = new AppointmentDTO()
            {
                appointmentStartTime = new DateTime(2023, 01, 06, 13, 30, 00),
                appointmentEndTime = new DateTime(2023, 01, 06, 14, 30, 00),
                appointmentTitle = "worldCup Discussion",
                appointmentDescription = "will messi win the WC"
            };
            var systemUnderTest = new AppointmentsController(MockServiceBL.Object);

            //Act
            var result = systemUnderTest.AddAppointment(MockAppointment) as BadRequestObjectResult;

            //Assert
            Assert.True(result?.StatusCode.Equals(400));
        }

        [Fact]
        public void AddNewAppointment_Returns_400_BadRequest_On_Same_StartTime_And_EndTime()
        {
            //Arrange  
            var MockAppointment = new AppointmentDTO()
            {
                appointmentStartTime = new DateTime(2023, 01, 08, 13, 30, 00),
                appointmentEndTime = new DateTime(2023, 01, 08, 13, 30, 00),
                appointmentTitle = "worldCup Discussion",
                appointmentDescription = "will messi win the WC"
            };
            var systemUnderTest = new AppointmentsController(MockServiceBL.Object);

            //Act
            var result = systemUnderTest.AddAppointment(MockAppointment) as BadRequestObjectResult;

            //Assert
            Assert.True(result?.StatusCode.Equals(400));
        }



        // //Delete appointment tests //
        [Fact]
        public void DeleteAppointment_Returns_204_NoContent_On_Succesful_Deletion()
        {
            //Arrange
            MockServiceBL.Setup(service => service.DeleteAppointment(It.IsAny<Guid>())).Returns(true);
            var controllerUnderTest = new AppointmentsController(MockServiceBL.Object);

            //Act
            var result = controllerUnderTest.DeleteAppointment(MockGuid) as NoContentResult;

            //Assert
            Assert.True(result?.StatusCode.Equals(204));
        }

        [Fact]
        public void DeleteAppointment_Returns_404_NotFound()
        {
            //Arrange
            MockServiceBL.Setup(service => service.DeleteAppointment(It.IsAny<Guid>())).Returns(false);
            var controllerUnderTest = new AppointmentsController(MockServiceBL.Object);

            //Act
            var result = controllerUnderTest.DeleteAppointment(MockGuid) as NotFoundObjectResult;

            //Assert
            Assert.True(result?.StatusCode.Equals(404));
        }




        // //update Existing Appointment
        [Fact]
        public void UpdateAppointment_Returns_204_NoContent_On_Succesful_Updation()
        {
            //Arrange
            MockServiceBL.Setup(service => service.UpdateAppointment(It.IsAny<Guid>(), It.IsAny<AppointmentDTO>())).Returns(true);
            var controllerUnderTest = new AppointmentsController(MockServiceBL.Object);

            //Act
            var result = controllerUnderTest.UpdateAppointment(MockGuid, MockData.MockAppointmentDTO);
            var ObjectResult = (NoContentResult)result;

            //Assert
            Assert.True(ObjectResult.StatusCode.Equals(204));
        }

        [Fact]
        public void UpdateAppointment_Returns_409_Conflict_Between_Different_Meetings()
        {
            //Arrange
            MockServiceBL.Setup(service => service.UpdateAppointment(It.IsAny<Guid>(), It.IsAny<AppointmentDTO>())).Returns(false);
            var systemUnderTest = new AppointmentsController(MockServiceBL.Object);

            //Act
            var result = systemUnderTest.UpdateAppointment(MockGuid, MockData.MockAppointmentDTO);
            var ObjectResult = (ConflictObjectResult)result;

            //Assert
            Assert.True(ObjectResult.StatusCode.Equals(409));
        }

        [Fact]
        public void UpdateExistingAppointment_Returns_400_BadRequest_On_EndTime_LessThan_StartTime()
        {
            //Arrange  
            var MockAppointment = new AppointmentDTO()
            {
                appointmentStartTime = DateTime.Now.AddHours(4),
                appointmentEndTime = DateTime.Now.AddHours(3),
                appointmentTitle = "worldCup Discussion",
                appointmentDescription = "will messi win the WC"
            };
            var systemUnderTest = new AppointmentsController(MockServiceBL.Object);

            //Act
            var result = systemUnderTest.UpdateAppointment(MockGuid, MockAppointment) as BadRequestObjectResult;

            //Assert
            Assert.True(result?.StatusCode.Equals(400));
        }

        [Fact]
        public void UpdateExistingAppointment_Returns_400_BadRequest_On_Same_StartTime_And_EndTime()
        {
            //Arrange  
            var MockAppointment = new AppointmentDTO()
            {
                appointmentStartTime = new DateTime(2023, 01, 08, 13, 30, 00),
                appointmentEndTime = new DateTime(2023, 01, 08, 13, 30, 00),
                appointmentTitle = "worldCup Discussion",
                appointmentDescription = "will messi win the WC"
            };
            var systemUnderTest = new AppointmentsController(MockServiceBL.Object);

            //Act
            var result = systemUnderTest.UpdateAppointment(MockGuid, MockAppointment) as BadRequestObjectResult;

            //Assert
            Assert.True(result?.StatusCode.Equals(400));
        }

        [Fact]
        public void UpdateExistingAppointment_Returns_400_BadRequest_On_PastTime()
        {
            //Arrange   
            var MockAppointment = new AppointmentDTO()
            {
                appointmentStartTime = DateTime.Now.AddHours(-2),
                appointmentEndTime = DateTime.Now.AddHours(3),
                appointmentTitle = "worldCup Discussion",
                appointmentDescription = "will messi win the WC"
            };
            var systemUnderTest = new AppointmentsController(MockServiceBL.Object);

            //Act
            var result = systemUnderTest.UpdateAppointment(MockGuid, MockAppointment) as BadRequestObjectResult;

            //Assert
            Assert.True(result?.StatusCode.Equals(400));
        }
        [Fact]
        public void UpdateExistingAppointment_Returns_400_BadRequest_On_PastDate()
        {
            //Arrange      
            var MockAppointment = new AppointmentDTO()
            {
                appointmentStartTime = new DateTime(2023, 01, 06, 13, 30, 00),
                appointmentEndTime = new DateTime(2023, 01, 06, 14, 30, 00),
                appointmentTitle = "worldCup Discussion",
                appointmentDescription = "will messi win the WC"
            };
            var systemUnderTest = new AppointmentsController(MockServiceBL.Object);

            //Act
            var result = systemUnderTest.UpdateAppointment(MockGuid, MockAppointment) as BadRequestObjectResult;

            //Assert
            Assert.True(result?.StatusCode.Equals(400));
        }
        
    }
}