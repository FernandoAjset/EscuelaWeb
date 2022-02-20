using EscuelaWeb.Servicios;
using Microsoft.EntityFrameworkCore;

namespace EscuelaWeb.Models
{
    public class EscuelaContext : DbContext
    {
        public DbSet<Escuela> Escuelas { get; set; }
        public DbSet<Asignatura> Asignaturas { get; set; }
        public DbSet<Curso> Cursos { get; set; }
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
            escuela.AñoDeCreacion = 2020;
            escuela.Id = Guid.NewGuid().ToString();
            escuela.Nombre = "Example School";
            escuela.Direccion = "Edificio 6-25";
            escuela.Ciudad = "Bogota";
            escuela.Pais = "Colombia";
            escuela.TipoEscuela = TiposEscuela.Secundaria;


            //Cargar cursos de la escuela
            var cursos = CargarCursos(escuela);

            //por cada curso cargar asignatura
            var asignaturas = CargarAsignaturas(cursos);

            //por cada curso cargar alumnos
            var alumnos = CargarAlumnos(cursos);

            //Sembrando datos a la BD en memoria
            modelBuilder.Entity<Escuela>().HasData(escuela);
            modelBuilder.Entity<Curso>().HasData(cursos.ToArray());
            modelBuilder.Entity<Asignatura>().HasData(asignaturas.ToArray());
            modelBuilder.Entity<Alumno>().HasData(alumnos.ToArray());
        }
        public static List<Curso> CargarCursos(Escuela escuela)
        {
            var cursosEscuela = new List<Curso>() {
                new Curso(){Id=Guid.NewGuid().ToString(), EscuelaId=escuela.Id, Nombre="101", Jornada=TiposJornada.Mañana},
                new Curso(){Id=Guid.NewGuid().ToString(), EscuelaId=escuela.Id, Nombre="201", Jornada=TiposJornada.Tarde},
                new Curso(){Id=Guid.NewGuid().ToString(), EscuelaId=escuela.Id, Nombre="301", Jornada=TiposJornada.Mañana},
                new Curso(){Id=Guid.NewGuid().ToString(), EscuelaId=escuela.Id, Nombre="401", Jornada=TiposJornada.Tarde},
                new Curso(){Id=Guid.NewGuid().ToString(), EscuelaId=escuela.Id, Nombre="501", Jornada=TiposJornada.Noche},

                new Curso(){Id=Guid.NewGuid().ToString(), EscuelaId=escuela.Id, Nombre="Programación I", Jornada=TiposJornada.Mañana},
                new Curso(){Id=Guid.NewGuid().ToString(), EscuelaId=escuela.Id, Nombre="Programación II", Jornada=TiposJornada.Tarde},
                new Curso(){Id=Guid.NewGuid().ToString(), EscuelaId=escuela.Id, Nombre="Programación III", Jornada=TiposJornada.Mañana},
                new Curso(){Id=Guid.NewGuid().ToString(), EscuelaId=escuela.Id, Nombre="Bases de datos I", Jornada=TiposJornada.Tarde},
                new Curso(){Id=Guid.NewGuid().ToString(), EscuelaId=escuela.Id, Nombre="Bases de datos II", Jornada=TiposJornada.Noche},
                new Curso(){Id=Guid.NewGuid().ToString(), EscuelaId=escuela.Id, Nombre="Sistemas operativos I", Jornada=TiposJornada.Mañana},
                new Curso(){Id=Guid.NewGuid().ToString(), EscuelaId=escuela.Id, Nombre="Calculo I", Jornada=TiposJornada.Tarde},
                new Curso(){Id=Guid.NewGuid().ToString(), EscuelaId=escuela.Id, Nombre="Calculo II", Jornada=TiposJornada.Mañana},
                new Curso(){Id=Guid.NewGuid().ToString(), EscuelaId=escuela.Id, Nombre="Autómatas", Jornada=TiposJornada.Tarde},
                new Curso(){Id=Guid.NewGuid().ToString(), EscuelaId=escuela.Id, Nombre="Electrónica Analógica", Jornada=TiposJornada.Noche},
            };
            return cursosEscuela;
        }

        public static List<Asignatura> CargarAsignaturas(List<Curso> cursos)
        {
            var listaAsignaturas = new List<Asignatura>();
            foreach (var curso in cursos)
            {
                var listaTemporal = new List<Asignatura>() {
                            new Asignatura
                            {
                                Nombre = "Matemáticas",
                                CursoId=curso.Id,
                                CursoNombre=curso.Nombre,
                                Id = Guid.NewGuid().ToString()
                            },
                            new Asignatura
                            {
                                Nombre = "Educación Física",
                                CursoId=curso.Id,
                                CursoNombre=curso.Nombre,
                                Id = Guid.NewGuid().ToString()
                            },
                            new Asignatura
                            {
                                Nombre = "Castellano",
                                CursoId=curso.Id,
                                CursoNombre=curso.Nombre,
                                Id = Guid.NewGuid().ToString()
                            },
                            new Asignatura
                            {
                                Nombre = "Ciencias Naturales",
                                CursoId=curso.Id,
                                CursoNombre=curso.Nombre,
                                Id = Guid.NewGuid().ToString()
                            },
                            new Asignatura
                            {
                                Nombre = "Programación",
                                CursoId=curso.Id,
                                CursoNombre=curso.Nombre,
                                Id = Guid.NewGuid().ToString()
                            }
                };
                listaAsignaturas.AddRange(listaTemporal);

            }
            return listaAsignaturas;
        }

        private List<Alumno> CargarAlumnos(List<Curso> cursos)
        {
            var listaAlumnos = new List<Alumno>();
            Random rnd = new Random();

            foreach (var curso in cursos)
            {
                int cantidadRandom = rnd.Next(5, 20);
                var listaTemporal = Alumno_DAO.GenerarAlumnosAlAzar(curso, cantidadRandom);
                listaAlumnos.AddRange(listaTemporal);
            }
            return listaAlumnos;
        }
    }
}
