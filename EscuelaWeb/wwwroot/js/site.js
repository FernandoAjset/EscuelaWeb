var saludo = "hola mundo";

function cambio() {
    const selecAlumno = document.getElementById("alumnos");
    const alumnoValue = selecAlumno.value;

    const r = document.getElementById("resultado");
    r.innerText = alumnoValue;
}