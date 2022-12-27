using DisprzTraining.Business;
using DisprzTraining.DataAccess;


namespace DisprzTraining.Tests.UnitTests.Service
{
    public class TestAppointmentServiceBL
    {
      private static IAppointmentDAL _appointmentDAL=new AppointmentDAL();
      AppointmentBL _appointmentBL=new AppointmentBL(_appointmentDAL);


      [Fact]
      public async Task DeleteAppointment_Returns_True_On_Deletion()
      {
        //Arrange
        
        //Act

        //Assert
      }


    }
}