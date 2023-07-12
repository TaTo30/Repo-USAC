let Errores = [];

function AgregarError(token, linea, columna, tipo) {
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

function GetErrores() {
    return Errores;
}

exports.AgregarError = AgregarError;
exports.Limpiar = LimpiarErrores;
exports.ObtenerErrores = GetErrores;