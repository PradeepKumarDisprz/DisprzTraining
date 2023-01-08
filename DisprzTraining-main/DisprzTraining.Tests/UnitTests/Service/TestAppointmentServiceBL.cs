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


        //Get All Appointments
        [Fact]
        public void GetAllAppointment_Returns_Paginated_Result_Offset0_and_FetchCount5()
        {
            //Arrange
            var expectedResult = new PaginatedAppointments() { isTruncated = true, appointments = MockData.MockAppointmentList.Skip(0).Take(5).ToList() };
            MockServiceDAL.Setup(service => service.GetAllAppointments(It.IsAny<int>(), It.IsAny<int>(), null, null)).Returns(expectedResult);
            var serviceUnderTest = new AppointmentBL(MockServiceDAL.Object);

            //Act
            var result = serviceUnderTest.GetAllAppointments(0, 5, null, null);
            //Assert
            Assert.IsType<PaginatedAppointments>(result);
            Assert.Equal(expectedResult.isTruncated, result.isTruncated);
            Assert.Equal(expectedResult.appointments.Count(), result.appointments.Count());
        }

        [Fact]
        public void GetAllAppointment_Returns_Paginated_Result_OffSet5_and_FetchCount5()
        {
            //Arrange
            var expectedResult = new PaginatedAppointments() { isTruncated = false, appointments = MockData.MockAppointmentList.Skip(5).Take(5).ToList() };
            MockServiceDAL.Setup(service => service.GetAllAppointments(It.IsAny<int>(), It.IsAny<int>(), null, null)).Returns(expectedResult);
            var serviceUnderTest = new AppointmentBL(MockServiceDAL.Object);

            //Act
            var result = serviceUnderTest.GetAllAppointments(5, 5, null, null);
            //Assert
            Assert.IsType<PaginatedAppointments>(result);
            Assert.Equal(expectedResult.isTruncated, result.isTruncated);
            Assert.Equal(expectedResult.appointments.Count(), result.appointments.Count());
        }

        [Fact]
        public void GetAllAppointment_Returns_Empty_Paginated_Result_OffSet10_and_FetchCount5()
        {
            //Arrange
            var expectedResult = new PaginatedAppointments() { isTruncated = false, appointments = MockData.MockAppointmentList.Skip(10).Take(5).ToList() };
            MockServiceDAL.Setup(service => service.GetAllAppointments(It.IsAny<int>(), It.IsAny<int>(), null, null)).Returns(expectedResult);
            var serviceUnderTest = new AppointmentBL(MockServiceDAL.Object);

            //Act
            var result = serviceUnderTest.GetAllAppointments(10, 5, null, null);
            //Assert
            Assert.IsType<PaginatedAppointments>(result);
            Assert.Equal(expectedResult.isTruncated, result.isTruncated);
            Assert.Equal(expectedResult.appointments.Count(), result.appointments.Count());
        }

        [Fact]
        public void GetAllAppointment_Returns_Paginated_Result_With_SearchDate()
        {
            //Arrange
            var expectedResult = new PaginatedAppointments() { isTruncated = false, appointments = MockData.MockAppointmentWithDate.Skip(0).Take(5).ToList() };
            MockServiceDAL.Setup(service => service.GetAllAppointments(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(), null)).Returns(expectedResult);
            var serviceUnderTest = new AppointmentBL(MockServiceDAL.Object);

            //Act
            var result = serviceUnderTest.GetAllAppointments(0, 5, new DateTime(2023, 01, 06),null);

            //Assert
            Assert.IsType<PaginatedAppointments>(result);
            Assert.Equal(expectedResult.isTruncated, result.isTruncated);
            Assert.Equal(expectedResult.appointments.Count(), result.appointments.Count());
        }

        [Fact]
        public void GetAllAppointment_Returns_200_Success_And_Paginated_Result_With_Search_Title()
        {
            //Arrange
            var expectedResult = new PaginatedAppointments() { isTruncated = false, appointments = MockData.MockAppointmentWithTitle.Skip(0).Take(5).ToList() };
            MockServiceDAL.Setup(service => service.GetAllAppointments(It.IsAny<int>(), It.IsAny<int>(), null, It.IsAny<string>())).Returns(expectedResult);
            var systemUnderTest = new AppointmentBL(MockServiceDAL.Object);

            //Act
            var result = systemUnderTest.GetAllAppointments(0, 5, null, "worldCup Discussion");

            //Assert
            Assert.IsType<PaginatedAppointments>(result);
            Assert.Equal(expectedResult.isTruncated, result.isTruncated);
            Assert.Equal(expectedResult.appointments.Count(), result.appointments.Count());
        }

        [Fact]
        public void GetAllAppointment_Returns_200_Success_And_Paginated_Result_With_Search_Date_And_Title()
        {
            //Arrange
            var expectedResult = new PaginatedAppointments() { isTruncated = false, appointments = MockData.MockAppointmentWithTitle.Skip(0).Take(5).ToList() };
            MockServiceDAL.Setup(service => service.GetAllAppointments(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<string>())).Returns(expectedResult);
            var systemUnderTest = new AppointmentBL(MockServiceDAL.Object);

            //Act
            var result = systemUnderTest.GetAllAppointments(0, 5, new DateTime(2023, 01, 06), "worldCup Discussion");

            //Assert
            Assert.IsType<PaginatedAppointments>(result);
            Assert.Equal(expectedResult.isTruncated, result.isTruncated);
            Assert.Equal(expectedResult.appointments.Count(), result.appointments.Count());
        }
    }
}