namespace EscuelaWeb.Servicios
{
    public class ConexionBD
    {
        public static string ConnectionString;

        public ConexionBD()
        {

        }
        public ConexionBD(IConfiguration configuration)
        {
            ConnectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public static string GetConnectionString()
        {
            return ConnectionString;
        }
    }
}
