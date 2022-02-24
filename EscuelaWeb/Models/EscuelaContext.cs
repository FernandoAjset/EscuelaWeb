using EscuelaWeb.Servicios;
using Microsoft.EntityFrameworkCore;

namespace EscuelaWeb.Models
{
    public class EscuelaContext : DbContext
    {
        public DbSet<Escuela> Escuelas { get; set; }
        public DbSet<Asignatura> Asignaturas { get; set; }
        public DbSet<Carrera> Carreras { get; set; }
        public DbSet<Evaluacion> Evalucaiones { get; set; }
        public DbSet<Alumno> Alumnos { get; set; }

        private string connectionString;
        public IConfiguration Configuration { get; }
        public EscuelaContext(IConfiguration configuration)
        {
            Configuration = configuration;
            connectionString = Configuration.GetConnectionString("DefaultConnection");
        }

        //Sobreescritura de método para configurar la conexión, extrayendo la cadena del Configuration
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer(
                    connectionString,
                    providerOptions => { providerOptions.EnableRetryOnFailure(); });
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var escuela = new Escuela();
            escuela.AñoDeCreacion = 2021;
            escuela.Id = Guid.NewGuid().ToString();
            escuela.Nombre = "Universidad GT";
            escuela.Direccion = "6ta av. Ed GuateEdu Torre, Zona 10";
            escuela.Ciudad = "Ciudad Capital";
            escuela.Pais = "Guatemala";
            escuela.TipoEscuela = TiposEscuela.Universidad;


            //Cargar Carreras de la escuela
            var Carreras = CargarCarreras(escuela);

            //por cada Carrera cargar asignatura
            var asignaturas = CargarAsignaturas(Carreras);

            //por cada Carrera cargar alumnos
            var alumnos = CargarAlumnos(Carreras);

            //Sembrando datos a la BD en memoria
            modelBuilder.Entity<Escuela>().HasData(escuela);
            modelBuilder.Entity<Carrera>().HasData(Carreras.ToArray());
            modelBuilder.Entity<Asignatura>().HasData(asignaturas.ToArray());
            modelBuilder.Entity<Alumno>().HasData(alumnos.ToArray());
        }
        public static List<Carrera> CargarCarreras(Escuela escuela)
        {
            var CarrerasEscuela = new List<Carrera>() {
                new Carrera(){Id=Guid.NewGuid().ToString(), EscuelaId=escuela.Id, Nombre="Derecho", Jornada=TiposJornada.Mañana},
                new Carrera(){Id=Guid.NewGuid().ToString(), EscuelaId=escuela.Id, Nombre="Psicología", Jornada=TiposJornada.Tarde},
                new Carrera(){Id=Guid.NewGuid().ToString(), EscuelaId=escuela.Id, Nombre="Administración", Jornada=TiposJornada.Mañana},
                new Carrera(){Id=Guid.NewGuid().ToString(), EscuelaId=escuela.Id, Nombre="Auditoría", Jornada=TiposJornada.Tarde},
                new Carrera(){Id=Guid.NewGuid().ToString(), EscuelaId=escuela.Id, Nombre="Criminología", Jornada=TiposJornada.Noche},

                new Carrera(){Id=Guid.NewGuid().ToString(), EscuelaId=escuela.Id, Nombre="Ingeniería Civil", Jornada=TiposJornada.Mañana},
                new Carrera(){Id=Guid.NewGuid().ToString(), EscuelaId=escuela.Id, Nombre="Ingeniería en Agronomía", Jornada=TiposJornada.Tarde},
                new Carrera(){Id=Guid.NewGuid().ToString(), EscuelaId=escuela.Id, Nombre="Ingeniería en Sistemas", Jornada=TiposJornada.Mañana},
                new Carrera(){Id=Guid.NewGuid().ToString(), EscuelaId=escuela.Id, Nombre="Medicina", Jornada=TiposJornada.Tarde},
                new Carrera(){Id=Guid.NewGuid().ToString(), EscuelaId=escuela.Id, Nombre="Diseño Grafico", Jornada=TiposJornada.Noche},
                new Carrera(){Id=Guid.NewGuid().ToString(), EscuelaId=escuela.Id, Nombre="Pedagogía", Jornada=TiposJornada.Mañana},
                new Carrera(){Id=Guid.NewGuid().ToString(), EscuelaId=escuela.Id, Nombre="Ingeniería Eléctrica", Jornada=TiposJornada.Tarde},
                new Carrera(){Id=Guid.NewGuid().ToString(), EscuelaId=escuela.Id, Nombre="Física", Jornada=TiposJornada.Mañana},
                new Carrera(){Id=Guid.NewGuid().ToString(), EscuelaId=escuela.Id, Nombre="Producción Audiovisual", Jornada=TiposJornada.Tarde},
            };
            return CarrerasEscuela;
        }

        public static List<Asignatura> CargarAsignaturas(List<Carrera> Carreras)
        {
            var listaAsignaturas = new List<Asignatura>();
            foreach (var Carrera in Carreras)
            {
                var listaTemporal = new List<Asignatura>() {
                            new Asignatura
                            {
                                Nombre = "Matemáticas",
                                CarreraId=Carrera.Id,
                                CarreraNombre=Carrera.Nombre,
                                Id = Guid.NewGuid().ToString()
                            },
                            new Asignatura
                            {
                                Nombre = "Etica",
                                CarreraId=Carrera.Id,
                                CarreraNombre=Carrera.Nombre,
                                Id = Guid.NewGuid().ToString()
                            },
                            new Asignatura
                            {
                                Nombre = "Literatura",
                                CarreraId=Carrera.Id,
                                CarreraNombre=Carrera.Nombre,
                                Id = Guid.NewGuid().ToString()
                            },
                            new Asignatura
                            {
                                Nombre = "Historia",
                                CarreraId=Carrera.Id,
                                CarreraNombre=Carrera.Nombre,
                                Id = Guid.NewGuid().ToString()
                            },
                            new Asignatura
                            {
                                Nombre = "Programación",
                                CarreraId=Carrera.Id,
                                CarreraNombre=Carrera.Nombre,
                                Id = Guid.NewGuid().ToString()
                            }
                };
                listaAsignaturas.AddRange(listaTemporal);

            }
            return listaAsignaturas;
        }

        private List<Alumno> CargarAlumnos(List<Carrera> Carreras)
        {
            var listaAlumnos = new List<Alumno>();
            Random rnd = new Random();

            foreach (var Carrera in Carreras)
            {
                int cantidadRandom = rnd.Next(2, 5);
                var listaTemporal = Alumno_DAO.GenerarAlumnosAlAzar(Carrera, cantidadRandom);
                listaAlumnos.AddRange(listaTemporal);
            }
            return listaAlumnos;
        }
    }
}
