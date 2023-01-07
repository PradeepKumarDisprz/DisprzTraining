// using DisprzTraining.DataAccess;
// using DisprzTraining.Model;


// namespace DisprzTraining.Tests.UnitTests.Service
// {
//     public class TestAppointmentServiceDAL
//     {
//         AppointmentDAL _appointmentDAL=new AppointmentDAL();

//         [Fact]
//         public async Task GetAppointments_Returns_AllAppointments()
//         {
//             //Act
//             var result= await _appointmentDAL.GetAppointments();
            
//             //Assert
//             Assert.IsType<List<Appointment>>(result);
//             Assert.True(result.Count()>0);
//         }

//          [Fact]
//         public async Task GetAppointments_Returns_EmptyList()
//         {
//             //Act
//             var result= await _appointmentDAL.GetAppointments();
            
//             //Assert
//             Assert.IsType<List<Appointment>>(result);
//             Assert.True(result.Count()==0);
//         }

//     }
// }