let Tokens = [];

function AgregarToken(value, tipo, linea, columna) {
    var token = {
        "Valor": value,
        "Tipo": tipo,
        "Linea": linea,
        "Columna": columna
    }
    Tokens.push(token);
}

function LimpiarTokens() {
    Tokens.splice(0, Tokens.length);
}

function GetTokens() {
    return Tokens
}

exports.AgregarToken = AgregarToken;
exports.Limpiar = LimpiarTokens;
exports.ObtenerTokens = GetTokens;