var dot = "";
var nodo = 0;
var listaNodos = [];

function Formmat() {
    listaNodos.splice(0, listaNodos.length);
    nodo = 0;
    dot = "";
}

function GetDot() {
    return `digraph g {\n ${dot}}`;
}

function GetCurrentNode() {
    return nodo;
}

function Push(root) {
    listaNodos.push(root);
}

function Root() {
    dot += `node${nodo}[label="RAIZ"];\n`;
    nodo++;
}

function InsertRoot(value, root) {
    dot += `node${nodo}[label=${value}];\n`;
    nodo++;
    dot += `node${listaNodos.pop()} -> node${root};\n`;
}

function InsertNode(value, root){
    dot += `node${nodo}[label=${value}];\n`;
    dot += `node${root} -> node${nodo};\n`;
    nodo++; 
}


exports.Push = Push;
exports.InsertNode = InsertNode;
exports.InsertRoot = InsertRoot;
exports.GetNodo = GetCurrentNode;
exports.GetDot = GetDot;
exports.Root = Root;
exports.Clear = Formmat;