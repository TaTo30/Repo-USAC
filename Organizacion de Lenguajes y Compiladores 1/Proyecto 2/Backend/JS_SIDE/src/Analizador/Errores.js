let Errores = [];

function AgregarError(token, linea, columna, tipo) {
    //console.log("se ingreso un serror");
    var error = {
        "Token": token,
        "Linea": linea,
        "Columna": columna,
        "Tipo": tipo
    }
    Errores.push(error);
}

function LimpiarErrores() {
    Errores.splice(0,Errores.length);
}

exports.AgregarError = AgregarError;
exports.Limpiar = LimpiarErrores;
exports.ObtenerErrores = Errores;