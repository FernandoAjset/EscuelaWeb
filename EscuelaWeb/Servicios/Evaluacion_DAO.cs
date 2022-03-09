using EscuelaWeb.Models;

namespace EscuelaWeb.Servicios
{
    public class Evaluacion_DAO
    {
        public static bool NotaValida(TipoEvaluacion tipoEvaluacion, float Nota)
        {
            if (tipoEvaluacion == TipoEvaluacion.PP && Nota > 10)
            {
                return false;
            }
            if (tipoEvaluacion == TipoEvaluacion.SP && Nota > 20)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
