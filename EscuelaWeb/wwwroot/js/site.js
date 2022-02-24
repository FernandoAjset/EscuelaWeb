var saludo = "hola mundo";

function cambio() {
    const selecAlumno = document.getElementById("alumnos");
    const alumnoValue = selecAlumno.value;

    const r = document.getElementById("IdCarrera");
    r.innerText = alumnoValue;
}

function cambioCarrera() {
    const selecAlumno = document.getElementById("Carreras");
    const alumnoValue = selecAlumno.value;

    const r = document.getElementById("IdCarrera");
    r.innerText = alumnoValue;
}