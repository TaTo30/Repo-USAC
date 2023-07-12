import { Button } from "primereact/button";
import { FileUpload } from "primereact/fileupload";
import { ListBox } from "primereact/listbox";
import { Toast } from "primereact/toast";
import { confirmPopup } from "primereact/confirmpopup";
import React, { useEffect, useState, useRef, useDebugValue } from "react";
import Axios from "axios";

const EditarAlbum = () => {
    const toast = useRef();

    const [baseUrl, setBaseUrl] = useState("http://application-load-balancer-1846011889.us-east-2.elb.amazonaws.com");
    const [listboxValue, setListboxValue] = useState(null);
    const [buttonEnabled, setButtonEnabled] = useState(false);
    const [showFilePanel, setShowFilePanel] = useState(false);
    const [selectedItem, setSelectedItem] = useState("");
    const [listboxValues, setListboxValues] = useState([{ name: "No albums" }]);
    const [nameKeyListBox, setNameKeyListBox] = useState("name");
    const [listaFotos, setListaFotos] = useState([]);

    const [userData, setUserData] = useState(() => {
        // getting stored value
        const saved = window.localStorage.getItem("key");
        const initialValue = JSON.parse(saved);
        return initialValue || {};
    });

    const handleListBoxValueSelected = (e) => {
        setListboxValue(e.value);
    };

    useEffect(() => {
        if (listboxValue !== null) {
            setSelectedItem(listboxValue.album);
            setButtonEnabled(true);
        } else {
            setSelectedItem("");
            setButtonEnabled(false);
        }
    }, [listboxValue]);

    const handleClickAddPicture = () => {
        setShowFilePanel((flag) => !flag);
    };

    /**
     * Procesa los archivos seleccionados previamente, los convierte en un <String> base 64 para
     * ser guardado en la base de datos
     * @param {*} event : Evento accionado al 'cargar' y 'subir' archivos de fotos seleccionadas
     */
    const onUpload = async (event) => {
        console.log(files);
        let files = event.files;
        //let cont = 1;
        let lista = [];
        let foto = "";
        let base64 = "";
        files.forEach((file) => {
            const fileReader = new FileReader();
            fileReader.readAsDataURL(file);
            fileReader.onload = (e) => {
                foto = e.target.result;
                base64 = foto.split(",")[1];
                lista.push(base64);
            };
        });

        if (lista) {
            showSuccess("Fotos para Album", "Se han agregado y transformado las fotos");
            updateListaFotos(lista);
        }
    };

    const handleClickOnSave = () => {
        modifyAlbum();
        console.log("We are here! now");
    };

    const confirm = (event) => {
        confirmPopup({
            target: event.currentTarget,
            message: "Are you sure you want to proceed?",
            icon: "pi pi-exclamation-triangle",
            accept,
            reject,
        });
    };

    const reject = () => {
        setListboxValue(null);
        toast.current.show({ severity: "warn", summary: "Abortado", detail: `La eliminacion del album <${selectedItem}> se ha abortado con exito!`, life: 3000 });
    };

    const accept = () => {
        eliminarAlbum();
        setListboxValue(null);
    };

    const eliminarAlbum = async () => {
        console.dir(userData);
        if ((listboxValue !== null) & (listboxValue !== undefined) & (listboxValue !== {})) {
            if ((selectedItem !== "") & (selectedItem !== "perfil")) {
                let body = {
                    idUsuario: userData.id,
                    nameAlbum: selectedItem.toString(),
                };
                console.log("Eliminando album:", selectedItem);

                try {
                    await Axios.delete(`${baseUrl}/album`, { data: body })
                        .then((res) => {
                            if (res.data !== undefined) {
                                showSuccess("Eliminar Album", `El album <${body.nameAlbum}> ha sido eliminado correctamente`);
                            }
                            getListaAlbums();
                        })
                        .catch((err) => {
                            showWarn("Eliminar Album", `La eliminacion del album <${selectedItem}> ha fallado, ${err}`);
                            getListaAlbums();
                        });
                } catch (error) {
                    console.error(error);
                    getListaAlbums();
                    showWarn("API Connection", "Ha ocurrido un error al hacer la peticion, " + error);
                }
            } else {
                showWarn("Elimar Album", "¡Atencion el album <perfil> no puede ser eliminado!");
            }
        } else {
            showWarn("Elimar Album", "¡Algo inesperado ha pasado, seleciona de nuevo el album que deseas eliminar!");
        }
    };

    /**
     * Se acciona cuando se hace click en el boton "Guardar Cambios"
     * modifica un album y agrega las fotos que se han cargado previamente
     * @returns void
     */
    const modifyAlbum = async () => {
        console.log("Myfotoslist:", listaFotos);
        if (listaFotos.length === 0) {
            showWarn("Modificar Album", "No se han agregado fotos, asegurese de que ha seleccionado y procesado las fotos correctamente!");
            setListaFotos([]);
            return;
        }
        let body = {};
        console.log("Agregando fotos a Album...");
        await listaFotos.forEach((newFoto) => {
            console.log("current:", selectedItem);
            body = {
                idUsuario: userData.id,
                usuario: userData.Usuario,
                nameAlbum: selectedItem,
                foto: newFoto,
            };
            try {
                Axios.post(`${baseUrl}/addFoto`, body)
                    .then((res) => {
                        if (res.data.Auth) {
                            showSuccess("Modificacion Album", `El album ${selectedItem}, la foto han sido agregada correctamente.`);
                        } else {
                            showWarn("Archivo de Foto", "Algo ha sucedido al agregar la imagen al album");
                            console.log(res.data.msg);
                        }
                    })
                    .catch((err) => {
                        showWarn("API Connection", "Algo ha ocurrido al hacer la peticion," + err);
                    });
            } catch (error) {
                console.error(error);
            }
        });
    };

    const getListaAlbums = async () => {
        try {
            await Axios.get(`${baseUrl}/albums/${userData.id}`)
                .then((res) => {
                    //console.log(res.data)
                    let albums = res.data.albums.map((element) => {
                        return element[0];
                    });
                    console.dir(fixAlbumsList(albums));
                    let fixedAlbumsList = fixAlbumsList(albums);
                    setNameKeyListBox("album");
                    setListboxValues(fixedAlbumsList);
                })
                .catch((err) => {
                    console.log(err);
                });
        } catch (error) {
            console.error(error);
            showWarn("API Connection", "Ha ocurrido un error al hacer la peticion, " + error);
        }
    };

    const fixAlbumsList = (list) => {
        let fixedList = [];
        list.forEach((element) => {
            fixedList.push({ album: element });
        });
        return fixedList;
    };

    const updateListaFotos = (list) => {
        setListaFotos(list);
    };

    const showWarn = (causa, mensaje) => {
        toast.current.show({ severity: "warn", summary: causa, detail: mensaje, life: 6000 });
    };

    const showSuccess = (causa, mensaje) => {
        toast.current.show({ severity: "success", summary: causa, detail: mensaje, life: 6000 });
    };

    useEffect(() => {
        getListaAlbums();
    }, []);

    return (
        <div className="grid">
            <div className="col-12">
                <div className="card">
                    <div className="grid p-fluid">
                        <Toast ref={toast} />
                        <h5>Modificar un Album Existente</h5>
                        <div className="col-12">
                            <ListBox value={listboxValue} onChange={handleListBoxValueSelected} options={listboxValues} optionLabel={nameKeyListBox} filter />
                        </div>
                        <div className="col-12 mb-2 lg:col-4 lg:mb-0">
                            <Button onClick={handleClickAddPicture} disabled={!buttonEnabled} label="Agregar Fotos"></Button>
                        </div>
                        <div className="col-12 mb-2 lg:col-4 lg:mb-0">
                            <Button className="mr-2" onClick={confirm} disabled={!buttonEnabled} label="Eliminar"></Button>
                        </div>
                    </div>
                    {showFilePanel ? (
                        <div className="card">
                            <h5>Escoge tu archivo</h5>
                            <FileUpload customUpload uploadHandler={onUpload} onUpload={onUpload} multiple accept="image/*" maxFileSize={1000000} />
                            <div className="card">
                                <Button onClick={handleClickOnSave} label="Guardar Cambios"></Button>
                            </div>
                        </div>
                    ) : (
                        <div />
                    )}
                </div>
            </div>
        </div>
    );
};

export default EditarAlbum;
