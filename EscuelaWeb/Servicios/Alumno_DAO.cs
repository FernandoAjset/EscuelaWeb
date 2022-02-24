using EscuelaWeb.Models;

namespace EscuelaWeb.Servicios
{
    public class Alumno_DAO
    {
        public static List<Alumno> GenerarAlumnosAlAzar(Carrera Carrera,int cantidad)
        {
            string[] nombre1 = { "Alba", "Felipa", "Eusebio", "Farid", "Donald", "Alvaro", "Nicolás" };
            string[] apellido1 = { "Ruiz", "Sarmiento", "Uribe", "Maduro", "Trump", "Toledo", "Herrera" };
            string[] nombre2 = { "Freddy", "Anabel", "Rick", "Murty", "Silvana", "Diomedes", "Nicomedes", "Teodoro" };

            var listaAlumnos = from n1 in nombre1
                               from n2 in nombre2
                               from a1 in apellido1
                               select new Alumno
                               {
                               CarreraId=Carrera.Id,
                               Nombre = $"{n1} {n2} {a1}", 
                               Id=Guid.NewGuid().ToString() 
                               };

            return listaAlumnos.OrderBy((al) => al.Id).Take(cantidad).ToList();
        }
    }
}
