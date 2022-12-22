using DisprzTraining.Business;
using DisprzTraining.Controllers;
using DisprzTraining.Model;
using DisprzTraining.Tests.MockDatas;
using Microsoft.AspNetCore.Mvc;
using Moq;


namespace DisprzTraining.Tests.Controller
{
    public class TestAppointment
    {

        private readonly Mock<IAppointmentBL> MockServiceBL = new();
        private readonly Guid MockGuid = Guid.NewGuid();

        [Fact]
        public async Task GetAllAppointment_Returns_200_Success_And_AllAppointmentsOfDateandTitle()
        {
            //Arrange
            var mockTruncated = new AllAppointments() { count = 2, isTruncated = true, appointments = MockData.MockAppointments };
            MockServiceBL.Setup(service => service.GetAllAppointments(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<string>())).ReturnsAsync(mockTruncated);
            var systemUnderTest = new AppointmentsController(MockServiceBL.Object);

            //Act
            var result = await systemUnderTest.GetAllAppointments(0, 1, DateTime.Now, "standup");
            var ObjectResult = (OkObjectResult)result;

            //Assert
            Assert.IsType<AllAppointments>(ObjectResult.Value);
            Assert.True(ObjectResult.StatusCode.Equals(200));
            Assert.True(ObjectResult.Value.Equals(mockTruncated));
        }

        [Fact]
        public async Task GetAllAppointment_Returns_200_Success_And_EmptyList()
        {
            //Arrange
            var mockTruncated = new AllAppointments() { count = 0, isTruncated = false, appointments =new List<Appointment>() };
            MockServiceBL.Setup(service => service.GetAllAppointments(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<string>())).ReturnsAsync(mockTruncated);
            var systemUnderTest = new AppointmentsController(MockServiceBL.Object);

            //Act
            var result = await systemUnderTest.GetAllAppointments(0, 3, DateTime.Now, "standup");
            var ObjectResult = (OkObjectResult)result;

            //Assert
            Assert.IsType<AllAppointments>(ObjectResult.Value);
            Assert.True(ObjectResult.StatusCode.Equals(200));
            Assert.True(ObjectResult.Value.Equals(mockTruncated));
        }
 
        [Fact]
        public async Task GetAppointmentById_Returns_200_Success_And_AppointmentById()
        {
            //Arrange
            MockServiceBL.Setup(service => service.GetAppointmentById(It.IsAny<Guid>())).ReturnsAsync(MockData.MockAppointmentByID);
            var systemUnderTest = new AppointmentsController(MockServiceBL.Object);

            //Act
            var result = await systemUnderTest.GetappointmentById(MockGuid);
            var ObjectResult = (OkObjectResult)result;

            //Assert
            Assert.IsType<List<Appointment>>(ObjectResult.Value);
            Assert.True(ObjectResult.StatusCode.Equals(200));
            Assert.True(ObjectResult.Value.Equals(MockData.MockAppointmentByID));
        }

        [Fact]
        public async Task GetAppointmentById_Returns_200_Success_And_EmptyList()
        {
            //Arrange
            MockServiceBL.Setup(service => service.GetAppointmentById(It.IsAny<Guid>())).ReturnsAsync(new List<Appointment>());
            var systemUnderTest = new AppointmentsController(MockServiceBL.Object);

            //Act
            var result = await systemUnderTest.GetappointmentById(MockGuid) as OkObjectResult;


            //Assert
            Assert.IsType<List<Appointment>>(result?.Value);
            Assert.True(result.StatusCode.Equals(200));
        }

        // //Create new appointment
        [Fact]
        public async Task AddNewAppointment_Returns_201_Created_And_NewAppointment()
        {
            //Arrange
            var MockNewIdCreated = new NewAppointmentId()
            {
                appointmentId = Guid.NewGuid()
            };
            MockServiceBL.Setup(service => service.AddNewAppointment(It.IsAny<AppointmentDetail>())).ReturnsAsync(MockNewIdCreated);
            var systemUnderTest = new AppointmentsController(MockServiceBL.Object);

            //Act
            var result = await systemUnderTest.AddNewAppointment(MockData.MockAppointment);
            var ObjectResult = (CreatedResult)result;

            //Assert
            Assert.True(ObjectResult.StatusCode.Equals(201));
            Assert.True(ObjectResult?.Value?.Equals(MockNewIdCreated));
        }

        [Fact]
        public async Task AddNewAppointment_Returns_409_Conflict()
        {
            //Arrange
            var MockNewIdCreated = new NewAppointmentId();
            MockServiceBL.Setup(service => service.AddNewAppointment(It.IsAny<AppointmentDetail>())).ReturnsAsync(()=>null);
            var systemUnderTest = new AppointmentsController(MockServiceBL.Object);

            //Act
            var result = await systemUnderTest.AddNewAppointment(MockData.MockAppointment);
            var ObjectResult = (ConflictObjectResult)result;

            //Assert
            Assert.True(ObjectResult.StatusCode.Equals(409));
        }

        [Fact]
        public async Task AddNewAppointment_Returns_400_BadRequest()
        {
            //Arrange
            var MockNewIdCreated = new NewAppointmentId();
            MockServiceBL.Setup(service => service.AddNewAppointment(It.IsAny<AppointmentDetail>())).ThrowsAsync(new Exception());
            var systemUnderTest = new AppointmentsController(MockServiceBL.Object);

            //Act
            var result = await systemUnderTest.AddNewAppointment(MockData.MockAppointment);
            var ObjectResult = (BadRequestObjectResult)result;

            //Assert
            Assert.True(ObjectResult.StatusCode.Equals(400));
        }

        //update Existing Appointment
        [Fact]
        public async Task UpdateExistingAppointment_Returns_204_NoContent_On_Updation()
        {
            //Arrange
            var MockAppointmentId = Guid.Parse("37981518-40f1-4580-946b-d47eb379453e");
            MockServiceBL.Setup(service => service.GetAppointmentById(It.IsAny<Guid>())).ReturnsAsync(MockData.MockAppointmentByID);
            MockServiceBL.Setup(service => service.UpdateExistingAppointment(MockAppointmentId, MockData.MockAppointment)).ReturnsAsync(true);
            var controllerUnderTest = new AppointmentsController(MockServiceBL.Object);

            //Act
            var result = await controllerUnderTest.UpdateExistingAppointment(MockAppointmentId, MockData.MockAppointment);
            var ObjectResult = (NoContentResult)result;

            //Assert
            Assert.True(ObjectResult.StatusCode.Equals(204));
        }

        [Fact]
        public async Task UpdateExistingAppointment_Returns_409_Conflict()
        {
            //Arrange
            MockServiceBL.Setup(service => service.GetAppointmentById(MockGuid)).ReturnsAsync(MockData.MockAppointmentByID);
            var systemUnderTest = new AppointmentsController(MockServiceBL.Object);

            //Act
            var result = await systemUnderTest.UpdateExistingAppointment(MockGuid, MockData.MockAppointment);
            var ObjectResult = (ConflictObjectResult)result;

            //Assert
            Assert.True(ObjectResult.StatusCode.Equals(409));
        }

        [Fact]
        public async Task UpdateExistingAppointment_Returns_404_NotFound_On_No_AppointmentExists()
        {
            //Arrange
            MockServiceBL.Setup(service => service.GetAppointmentById(It.IsAny<Guid>())).ReturnsAsync(new List<Appointment>());
            var systemUnderTest = new AppointmentsController(MockServiceBL.Object);

            //Act
            var result = await systemUnderTest.UpdateExistingAppointment(MockGuid, MockData.MockAppointment);
            var ObjectResult = (NotFoundObjectResult)result;

            //Assert
            Assert.True(ObjectResult.StatusCode.Equals(404));
        }

        // [Fact]
        // public async Task UpdateExistingAppointment_Returns_400_BadRequest_On_InvalidDateTime()
        // {
        //     //Arrange
        //     var MockServiceBL = new Mock<IAppointmentBL>();
        //     var MockId = Guid.Parse("37981518-40f1-4580-946b-d47eb379453e");
        //     var MockUpdateAppointment = new AppointmentDetail()
        //     {
        //         appointmentStartTime = new DateTime(2022,12,21,10,0,0),
        //         appointmentEndTime = new DateTime(2022,12,21,9,0,0),
        //         appointmentTitle = "worldCup Discussion",
        //         appointmentDescription = "will messi win the WC"
        //     };
        //     MockServiceBL.Setup(service => service.GetAppointmentById(It.IsAny<Guid>())).ReturnsAsync(MockData.MockAppointmentByID);
        //     MockServiceBL.Setup(service => service.UpdateExistingAppointment(It.IsAny<Guid>(),It.IsAny<AppointmentDetail>())).ThrowsAsync(new Exception());
        //     var systemUnderTest = new AppointmentsController(MockServiceBL.Object);
        //     //Act
        //     var result = await systemUnderTest.UpdateExistingAppointment(MockId,MockUpdateAppointment);
        //     //Assert
        //     await Assert.ThrowsAnyAsync<Exception>(()=>systemUnderTest.UpdateExistingAppointment(MockId,MockUpdateAppointment));
        //     // Assert.True(result?.StatusCode.Equals(400));
        // }

        //delete appointment tests
        [Fact]
        public async Task DeleteAppointment_Returns_204_NoContent_On_Deletion()
        {
            //Arrange
            MockServiceBL.Setup(service => service.DeleteAppointment(It.IsAny<Guid>())).ReturnsAsync(true);
            var controllerUnderTest = new AppointmentsController(MockServiceBL.Object);

            //Act
            var result = await controllerUnderTest.DeleteAppointment(MockGuid);
            var ObjectResult = (NoContentResult)result;

            //Assert
            Assert.True(ObjectResult.StatusCode.Equals(204));
        }

        [Fact]
        public async Task DeleteAppointment_Returns_404_NotFound()
        {
            //Arrange
            MockServiceBL.Setup(service => service.DeleteAppointment(It.IsAny<Guid>())).ReturnsAsync(false);
            var controllerUnderTest = new AppointmentsController(MockServiceBL.Object);

            //Act
            var result = await controllerUnderTest.DeleteAppointment(MockGuid);
            var ObjectResult = (NotFoundObjectResult)result;

            //Assert
            Assert.True(ObjectResult.StatusCode.Equals(404));
            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}