namespace Estudio.WebService.Static
{
    public class CatalogoErrores
    {
        public const string Cotizacion00 = "00 -  Los datos enviados no coinciden con nuestra base de datos.";
        //Asegurado
        public const string Cotizacion01 = "01 - No se puede consumir el método, el Token ha expirado";
        public const string Cotizacion02 = "02 - El campo “Tipo de Documento” no puede estar vacío.";
        public const string Cotizacion03 = "03 - El campo “Numero de Documento” no puede estar vacío.";
        public const string Cotizacion04 = "04 - El campo “Apellido Paterno” no puede estar vacío.";
        public const string Cotizacion05 = "05 - El campo “Apellido Materno” no puede estar vacío.";
        public const string Cotizacion06 = "06 - El campo “Nombres” no puede estar vacío.";
        public const string Cotizacion07 = "07 - El campo “Genero” no puede estar vacío.";
        public const string Cotizacion08 = "08 - El campo “Fecha de Nacimiento” no puede estar vacío.";
        public const string Cotizacion09 = "09 - El campo “Tipo de invalidez” no puede estar vacío.";

        public const string Cotizacion46 = "46 - El Departamento ingresado del Asegurado no existe.";
        public const string Cotizacion47 = "47 - El Estado Civil ingresado del Asegurado no existe.";
        public const string Cotizacion48 = "48 - La Nacionalidad ingresada del Asegurado no existe.";
        public const string Cotizacion49 = "49 - El País de Residencia ingresado del Asegurado no existe.";
        public const string Cotizacion50 = "50 - El Nombre del Documento del Asegurado ingresado no existe.";
        public const string Cotizacion51 = "51 - El Genero ingresado del Asegurado no existe.";
        public const string Cotizacion52 = "52 - La Provincia ingresada del Asegurado no existe.";
        public const string Cotizacion53 = "53 - El Distrito ingresado del Asegurado no existe.";
        public const string Cotizacion62 = "62 - El Asesor ingresado no existe.";

        public const string Cotizacion54 = "54 - El Departamento ingresado en Datos Laborales no existe.";
        public const string Cotizacion55 = "55 - La Provincia ingresada en Datos Laborales no existe.";
        public const string Cotizacion56 = "56 - El Distrito ingresado en Datos Laborales no existe.";

        public const string Cotizacion77 = "77 -  El campo “Tipo de AFP” no puede estar vacío.";//add
        public const string Cotizacion78 = "78 -  El campo “Tipo de Pensión” no puede estar vacío.";//add
        public const string Cotizacion79 = "79 -  El campo “Asesor” no puede estar vacío.";//add
        public const string Cotizacion80 = "80 -  El campo “Porcentaje de Pensión del Beneficiario” no puede estar vacío.";//add
        public const string Cotizacion82 = "82 -  El campo “Porcentaje de Renta Temporal” no puede estar vacío.";//add
        public const string Cotizacion81 = "81 -  El campo “Tipo de Modalidad” no puede estar";//add

        //Beneficiario
        public const string Cotizacion10 = "10 - El campo “Típo de Invalidez del Beneficiario” no puede estar vacio.";
        public const string Cotizacion11 = "11 - El campo “Fecha de Nacimiento del Beneficiario” no puede estar vacio.";
        public const string Cotizacion12 = "12 - El campo “Genero del Beneficiario” no puede estar vacio.";
        public const string Cotizacion13 = "13 - El campo “Clave Parentesco del Beneficiario” no puede estar vacio.";
        public const string Cotizacion14 = "14 - El campo “Porcentaje de Beneficiario” no puede estar vacío.";
        public const string Cotizacion57 = "57 - El Nombre del Documento ingresado del Beneficiario {0} no existe.";
        public const string Cotizacion58 = "58 - El Genero ingresado del Beneficiario {0} no existe.";
        public const string Cotizacion59 = "59 - El Parentesco ingresado del Beneficiario {0} no existe.";
        public const string Cotizacion73 = "73 - El campo “Clave Tipo del Beneficiario” no puede estar vacio.";
        public const string Cotizacion74 = "74 - La Clave de Tipo Beneficiario del Beneficiario {0} no existe.";

        //Modalidad
        public const string Cotizacion15 = "15 - El campo “Id Modalidad Jubilare de la Modalidad” no puede ser cero.";
        public const string Cotizacion16 = "16 - El campo “Prima Única de la Modalidad” no puede estar vacío.";
        public const string Cotizacion17 = "17 - El campo “Periodo de Vigencia de la Modalidad” no puede ser cero.";
        public const string Cotizacion18 = "18 - El campo “Primer Tramo de la Modalidad” no puede estar vacío.";
        public const string Cotizacion19 = "19 - El campo “Segundo Tramo de la Modalidad” no puede estar vacío.";
        public const string Cotizacion20 = "20 - El campo “Clave Moneda de la Modalidad” no puede estar vacío.";
        public const string Cotizacion21 = "21 - El campo “Tasa de Ajuste de Renta de la Modalidad” no puede estar vacío.";
        public const string Cotizacion22 = "22 - El campo “Periodo Diferido de la Modalidad” no puede estar vacío.";
        public const string Cotizacion23 = "23 - El campo “Periodo Garantizado de la Modalidad” no puede estar vacío.";
        public const string Cotizacion24 = "24 - El campo “Gratificacion de la Modalidad” no puede estar vacío.";
        public const string Cotizacion25 = "25 - El campo “Prima Devuelta de la Modalidad” no puede estar vacío.";
        public const string Cotizacion26 = "26 - El campo “Devolución Garantizada de la Modalidad” no puede estar vacío.";
        public const string Cotizacion27 = "27 - El campo “Gasto Sepelio de la Modalidad” no puede estar vacío.";
        public const string Cotizacion28 = "28 - El campo “Tipo de Renta de la Modalidad” no puede estar vacío.";

        public const string Cotizacion60 = "60 - La Moneda ingresada en la Modalidad {0} no existe.";
        public const string Cotizacion61 = "61 - El Tipo de Renta ingresado en la Modalidad {0} no existe.";
        public const string Cotizacion63 = "63 - El Periodo Diferido debe estar dentro de los siguientes límites {0} - {1}.";
        public const string Cotizacion64 = "64 - El Periodo de Vigencia ingresado no existe o se encuentra inactivo.";
        public const string Cotizacion65 = "65 - El Periodo de Vigencia con decimales debe terminar con .25, .50 o .75.";
        public const string Cotizacion66 = "66 - El Periodo Diferido con decimales debe terminar con .25, .50 o .75.";
        public const string Cotizacion67 = "67 - El Periodo Garantizado con decimales debe terminar con .25, .50 o .75.";

        public const string Cotizacion70 = "70 - El Primer Tramo con decimales debe terminar con .25, .50 o .75.";
        public const string Cotizacion71 = "71 - El periodo diferido debe ser menor al de vigencia.";
        public const string Cotizacion72 = "72 - El campo “Id Cotizacion Renta Privada” no puede estar vacío o ser cero.";

        public const string Cotizacion75 = "75 - Para Beneficiario Contingente, la Modalidad {0} debe tener Devolución Garantizada";
        public const string Cotizacion76 = "76 - Para Beneficiario Contingente, el Periodo Garantizado debe ser igual a Periodo de Vigencia menos el Periodo Diferido en la Modalidad {0}";

        //Renta Elegida
        public const string Cotizacion29 = "29 - Error en al menos no definir una “Renta Elegida”.";
        public const string Cotizacion30 = "30 - El campo “Fecha de Renta Elegida” no puede estar vacío.";
        public const string Cotizacion31 = "31 - El campo “Pensión de Renta Elegida” no puede ser cero";

        //Errores/Cotizacion
        public const string Cotizacion32 = "32 - Error en especificar un “S” en la “Gratificación” seleccionando los tipos de rentas: “Renta Elegida y Única”.";
        public const string Cotizacion33 = "33 - Error las cantidades de “Periodo Diferido” más el “Periodo Garantizado” son mayor que el “Periodo de Vigencia”.";
        public const string Cotizacion34 = "34 - Error al registrar “Renta Elegida” con “Primer Tramo” y “Porcentaje de Segundo Tramo”.";
        public const string Cotizacion35 = "35 - Error al asignar “Porcentaje de Tasa de Ajuste Renta Anual” al seleccionar la moneda VAC.";
        public const string Cotizacion36 = "36 - Error al asignar otra cantidad aparte del 100% en Porcentaje Devolución de Prima seleccionando el tipo de renta: Renta Única.";
        public const string Cotizacion37 = "37 - Error al no asignar la misma cantidad en “Periodo de Vigencia” con el “Periodo Diferido” al seleccionar el tipo de renta: “Renta Única”.";
        public const string Cotizacion38 = "38 - Error de almacenamiento de información en Base de Datos.";
        public const string Cotizacion39 = "39 - Error en la ejecución de calculo."; //Pendiente
        public const string Cotizacion40 = "40 - No se puede consumir el método, el Token no fue recibido";
        public const string Cotizacion41 = "41 - No se puede consumir el método, el Token no existe";
        public const string Cotizacion42 = "42 - El Asegurado Contratante no puede ser menor de 18 años";
        public const string Cotizacion43 = "43 - El Primer Tramo debe ser menor que el Periodo de Vigencia";
        public const string Cotizacion44 = "44 - El límite de la Prima Única está fuera de rango";
        public const string Cotizacion45 = "45 - El campo “Aplicación” no puede estar vacío.";
        public const string Cotizacion68 = "68 - El sistema no puede cotizar un caso con tasa de venta negativa, por favor modifique los datos ingresados en la modalidad {0}. Tasa de Venta: {1}, Renta Particular: {2}";
        public const string Cotizacion69 = "69 - La Situación Laboral ingresada no es válida. DE = Dependiente, IN = Independiente.";
    }
}