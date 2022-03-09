const urlObtenerAsignaturas = '@Url.Action("ObtenerAsignaturas")';

$(function() {
    $("#alumnos").change(async function () {
        const valorSeleccionado = $(this).val();

        const respuesta = await fetch(urlObtenerAsignaturas, {

            method: 'POST',
            body: valorSeleccionado,
            headers: {
                'Content-Type': 'application/json'
            }
        });

        const json = await respuesta.json();
        console.log(json);
    })
})